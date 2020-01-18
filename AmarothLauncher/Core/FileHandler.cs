using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmarothLauncher.Core
{
    /// <summary>
    ///FileHandler负责获取关于本地和服务器端文件的信息，以创建可选组和创建一个下载列表，最后下载其中的文件。
    /// </summary>
    public class FileHandler
    {
        // 用于输出消息和获取配置设置。
        OutputWriter o = OutputWriter.Instance;
        Config c = Config.Instance;

        List<string> blizzlikeMPQs = new List<string>(); // 放置MPQ类型文件的客户端路径(如果配置这么说，它们将被忽略)
        List<HandledFile> serverSideFiles = new List<HandledFile>(); // 放置服务器中filelist中列出的文件
        public List<OptionalGroup> optionalGroups = new List<OptionalGroup>(); // 所有可选组的列表

        List<string> outdated = new List<string>(); // 数据文件夹中的mpq列表，它们不在filelist中(或者它们的可选组不再被选中)，将被删除
        public List<HandledFile> toBeDownloaded = new List<HandledFile>(); // 缺少或过时的文件列表将被下载

        // 进度输出指令
        public int filesRemaining = 0;
        public int filesDownloaded = 0;
        public string nonOptionalOutdatedSize;

        // 用于与GUI通信的可选列表
        public ListView optionalsListView;

        // 为了避免到处输入这行代码
        string cwd = Directory.GetCurrentDirectory().ToLower();

        #region Control and misc methods...
        /// <summary>
        /// 检查登录器是否在实际的WoW目录中。检查wow.exe、Data和optionaly与MPQs类似
        /// </summary>
        /// <returns>必要的文件可以吗?</returns>
        public bool IsInWowDir()
        {
            // 检查WoW.exe和数据目录是否可以找到，以确保登录器在客户端的根目录。
            bool result = true;
            if (!File.Exists("wow.exe"))
            {
                o.Output(c.SubElText("Messages", "WowExeMissing"));
                result = false;
            }
            if (!Directory.Exists("data"))
            {
                o.Output(c.SubElText("Messages", "DataDirMissing"));
                result = false;
            }
            //如果官方的MPQ文件被保留，检查一下它们是否都在这里。
            else if (c.SubElText("Main", "KeepBlizzlikeMPQs") == "1")
            {
                blizzlikeMPQs.Add(cwd + @"\data\common.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\common-2.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\expansion.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\lichking.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\patch.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\patch-2.mpq");
                blizzlikeMPQs.Add(cwd + @"\data\patch-3.mpq");

                foreach (string mpqFile in blizzlikeMPQs)
                    if (!File.Exists(mpqFile))
                    {
                        o.Output(c.SubElText("Messages", "BlizzlikeMpqMissing") + mpqFile.Substring(mpqFile.LastIndexOf('\\') + 1));
                        result = false;
                    }
            }
            return result;
        }

        /// <summary>
        /// 输出生成的服务器端文件列表的全部内容。用于调试purpouses。
        /// </summary>
        public void OutputServerFilelist()
        {
            foreach (HandledFile hf in serverSideFiles)
            {
                o.Output("-----RelativeServerPath: " + hf.serverPath, true);
                o.Output("FullServerPath: " + hf.fullServerPath);
                if (hf.optional)
                    o.Output("Optional: TRUE");
                else
                    o.Output("Optional: FALSE");
                o.Output("Size: " + hf.size);
                o.Output("RelativeLocalPath: " + hf.localPath);
                o.Output("FullLocalPath: " + hf.fullLocalPath);
                o.Output("First comment: " + hf.firstComment);
                o.Output("LinkedList: " + hf.linked);
                foreach (HandledFile linked in hf.linkedList)
                {
                    o.Output("---LinkedRelativeServerPath: " + linked.serverPath);
                    o.Output("FullServerPath: " + linked.fullServerPath);
                    if (linked.optional)
                        o.Output("Optional: TRUE");
                    else
                        o.Output("Optional: FALSE");
                    o.Output("Size: " + linked.size);
                    o.Output("RelativeLocalPath: " + linked.localPath);
                    o.Output("FullLocalPath: " + linked.fullLocalPath);
                    o.Output("First comment: " + linked.firstComment);
                    o.Output("LinkedList: " + linked.linked);
                }
            }
        }

        /// <summary>
        /// 输出可选组列表的全部内容。用于调试purpouses。
        /// </summary>
        public void OutputOptionalGroups()
        {
            foreach (OptionalGroup og in optionalGroups)
            {
                o.Output("-----GroupName: " + og.name, true);
                foreach (HandledFile linked in og.files)
                {
                    o.Output("---LinkedRelativeServerPath: " + linked.serverPath);
                    o.Output("FullServerPath: " + linked.fullServerPath);
                    if (linked.optional)
                        o.Output("Optional: TRUE");
                    else
                        o.Output("Optional: FALSE");
                    o.Output("Size: " + linked.size);
                    o.Output("RelativeLocalPath: " + linked.localPath);
                    o.Output("FullLocalPath: " + linked.fullLocalPath);
                    o.Output("First comment: " + linked.firstComment);
                    o.Output("LinkedList: " + linked.linked);
                }
            }
        }

        /// <summary>
        /// 检查给定文件是否已完全下载。
        /// </summary>
        private bool IsFileOK(HandledFile hf)
        {
            if (File.Exists(hf.fullLocalPath))
                return (new FileInfo(hf.fullLocalPath).Length == hf.size);
            else
                return false;
        }

        /// <summary>
        /// 以最合适的单位返回给定大小（根据给定大小选择B、KB、MB或GB），四舍五入到小数点后3位。单位是返回字符串的一部分。
        /// </summary>
        public string Size(long size)
        {
            if (size < 1024)
                return "" + size + " B";
            else if (size < 1024 * 1024)
                return "" + Math.Round(size / 1024.0, 3) + " KB";
            else if (size < 1024 * 1024 * 1024)
                return "" + Math.Round(size / 1024.0 / 1024.0, 3) + " MB";
            else
                return "" + Math.Round(size / 1024.0 / 1024.0 / 1024.0, 3) + " GB";
        }

        /// <summary>
        /// 返回当前下载列表中所有文件的确切大小。
        /// </summary>
        public long SizeToBeDownloaded()
        {
            long totalSize = 0;
            foreach (HandledFile hf in toBeDownloaded)
                totalSize += hf.size;
            return totalSize;
        }
        #endregion

        #region Check for updates...
        /// <summary>
        /// 检查本地文件和文件列表以获取需要下载的文件列表，并为用户提供可供选择的可选文件列表。
        /// </summary>
        /// <returns>一切都好吗？</returns>
        public bool CheckForUpdates()
        {
            if (!BuildFilelist())
                return false;
            BuildOptionals();
            PreBuildDownloadList();
            return true;
        }

        /// <summary>
        /// 尝试基于修补程序列表生成服务器端文件列表。
        /// </summary>
        /// <returns>构建成功了吗？</returns>
        private bool BuildFilelist()
        {
            // 尝试下载文件列表的内容。
            string filelistString = null;
            try { filelistString = (new AmWebClient(3000)).DownloadString(c.SubElText("Paths", "FilelistPath")); }
            catch (WebException e)
            {
                o.Messagebox(c.SubElText("Messages", "FilelistOpeningFailed"), e);
                return false;
            }

            // 尝试读取文件列表的内容（不带注释，只保留具有文件列表项的行上的第一个注释，以获取可选的组名）。
            List<string> filelistContent = new List<string>();
            List<string> comments = new List<string>();
            if (filelistString != null)
            {
                // 至少有6个字符的只读行。保存每个正确行的第一个找到的注释。
                foreach (string s in filelistString.Split('\n'))
                {
                    string firstComment = "";
                    if (s.Split('#').Length > 1)
                        firstComment = s.Split('#')[1].Trim();
                    string tmp = Regex.Replace(s, @"\s+", "");
                    if (tmp.Split('#')[0].Length >= 6)
                    {
                        filelistContent.Add(tmp.Split('#')[0]);
                        comments.Add(firstComment);
                    }
                }

                // 验证filelist是否正确且可读，并将其数据保存为对象。否则出于安全原因而终止。
                int index = 0;
                foreach (string s in filelistContent)
                {
                    bool isCorrect = true;
                    var arr = s.Split(';');
                    if (arr.Length == 4                         // 名称；可选？；LocalPath；LinkedList-4条信息。
                        && arr[0].Length > 0                    // 名称不能为空
                        && (arr[1] == "0" || arr[1] == "1")     // 可选？必须是0或1。
                        && arr[2].Length > 0)                   // LocalPath必须至少为“/”。
                        foreach (string a in arr[3].Split(',')) // LinkedList中的元素不应为空。
                            if (a.Length == 0)
                                isCorrect = false;
                    if (arr[3].Length == 0)                     // 但是，如果整个LinkedList为空，则可以。
                        isCorrect = true;

                    if (!isCorrect)
                    {
                        o.Output(c.SubElText("Messages", "FilelistReadingFailed"));
                        return false;
                    }
                    else
                    {
                        string serverPath;
                        if (arr[0][0] == '/')
                            serverPath = arr[0].Substring(1);
                        else
                            serverPath = arr[0];

                        string localPath = "";
                        if (arr[2][0] != '/')
                            localPath += '/';
                        localPath += arr[2];
                        if (localPath[localPath.Length - 1] != '/')
                            localPath += '/';

                        serverSideFiles.Add(new HandledFile(serverPath, 0, arr[1], localPath, arr[3], comments[index]));
                    }
                    index++;
                }

                // 为服务器上的每个文件生成链接文件列表。
                List<HandledFile> linked = new List<HandledFile>();
                foreach (HandledFile hf in serverSideFiles)
                {
                    if (hf.linked != "")
                    {
                        foreach (string s in hf.linked.Split(','))
                        {
                            var namePathArr = s.Split('|');
                            string localPath = "";

                            // 在字符后面使用指定的LocalPath。如果未设置或为空，请使用其链接来源的文件的LocalPath。
                            if (namePathArr.Length == 2)
                            {
                                if (namePathArr[1] != "")
                                {
                                    if (namePathArr[1][0] != '/')
                                        localPath += '/';
                                    localPath += namePathArr[1];
                                }
                                else
                                    localPath = hf.localPath;
                            }
                            else
                            {
                                localPath = hf.localPath;
                            }
                            string serverPath;
                            if (namePathArr[0][0] == '/')
                                serverPath = namePathArr[0].Substring(1);
                            else
                                serverPath = namePathArr[0];

                            HandledFile linkedFile = new HandledFile(serverPath, 0, "1", localPath, "", "");
                            linkedFile.optional = hf.optional; // 不过，LinkedList仍应仅用于可选文件。
                            linked.Add(linkedFile);
                            hf.linkedList.Add(linkedFile);
                        }
                    }
                }

                // 将链接列表中链接的所有文件也添加到服务器端文件列表中。
                // 如果任何链接文件已在服务器端文件列表中，请删除其早期版本。LinkedList优先于filelist。
                foreach (HandledFile link in linked)
                {
                    HandledFile alreadyThere = null;
                    foreach (HandledFile hf in serverSideFiles)
                        if (hf.serverPath == link.serverPath)
                            alreadyThere = hf;
                    if (alreadyThere != null)
                        serverSideFiles.Remove(alreadyThere);
                    serverSideFiles.Add(link);
                }

                // 检查文件中所有文件是否存在并获取它们的大小。
                bool filesOK = true;
                foreach (HandledFile hf in serverSideFiles)
                {
                    if (!SetFileSize(hf))
                        filesOK = false;
                }
                if (!filesOK)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 从Web获取给定文件的大小，并将其值设置为它，并检查文件是否实际存在并可同时访问。
        /// </summary>
        /// /// <returns>是否获得大小？</returns>
        private bool SetFileSize(HandledFile hf)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(hf.fullServerPath);
            req.Method = "HEAD";
            req.Timeout = 3000;
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    long contentLength;
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out contentLength))
                        hf.size = contentLength;
                }
            }
            catch (WebException e)
            {
                o.Output(c.SubElText("Messages", "FileOnsWebMissing") + hf.fullServerPath, e);
                return false;
            }
            return true;
        }
        #endregion

        #region Optional list handling...
        /// <summary>
        /// 创建可选文件组列表并将其放入可选复选框列表中。
        /// </summary>
        private void BuildOptionals()
        {
            // 查看每个服务器端文件。
            foreach (HandledFile hf in serverSideFiles)
            {
                // 忽略非可选的文件。
                if (hf.optional)
                {
                    // 检查是否已从其他组链接了可选文件。
                    bool alreadyInDifferentGroup = false;
                    OptionalGroup group = null;
                    foreach (OptionalGroup og in optionalGroups)
                    {
                        if (og.Contains(hf.fullServerPath))
                        {
                            alreadyInDifferentGroup = true;
                            group = og;
                        }
                    }
                    // 如果可选项还不在任何组中，请为其创建一个新组。使用主文件的第一个注释作为名称。
                    if (!alreadyInDifferentGroup)
                    {
                        group = new OptionalGroup();
                        group.name = hf.firstComment;
                        group.Add(hf);
                        optionalGroups.Add(group);
                    }
                    // 添加到组可选文件已添加到（或在）文件的链接文件中找到（如果它们尚未存在）。
                    // 请注意，虽然可以链接组文件，但我并不推荐这样做。
                    foreach (HandledFile linked in hf.linkedList)
                    {
                        bool found = false;
                        foreach (HandledFile grouped in group.files)
                        {
                            if (grouped.fullServerPath == linked.fullServerPath)
                                found = true;
                        }
                        if (!found)
                            group.Add(linked);
                    }
                }
            }

            // 检查所有可选组。如果任何结果没有名称（没有设置第一个注释），将使用生成的注释。将它们添加到可选列表中。
            foreach (OptionalGroup og in optionalGroups)
            {
                if (og.name == "")
                    og.name = og.GenerateName();
                optionalsListView.Items.Add(og.name);
            }

            // 将具有可选组名、包含的文件名列表和组的总大小的工具提示指定给选项列表。
            foreach (ListViewItem lvi in optionalsListView.Items)
            {
                lvi.ToolTipText = optionalGroups[lvi.Index].name + ":\n\n";
                foreach (HandledFile hf in optionalGroups[lvi.Index].files)
                    lvi.ToolTipText += hf.name + "\n";
                lvi.ToolTipText += "\n" + c.SubElText("MainWindow", "ToolTipTotalSize") + Size(optionalGroups[lvi.Index].GetTotalSize());
            }
            LoadOptionalSettings();
        }

        /// <summary>
        /// 尝试加载用户上次给复选框列表的答案。
        /// </summary>
        private void LoadOptionalSettings()
        {
            if (File.Exists("LauncherOptionalSettings.conf"))
            {
                StreamReader sr = new StreamReader("LauncherOptionalSettings.conf");
                string line;
                int notFound = optionalsListView.Items.Count;

                while ((line = sr.ReadLine()) != null)
                {
                    int id = 0;
                    List<int> toBeChecked = new List<int>();

                    foreach (ListViewItem item in optionalsListView.Items)
                    {
                        if (line.Split('#').Length == 2)
                        {
                            // 请注意（仅）正在保存可选组的名称并与当前列表进行比较。
                            if (line.Split('#')[0] == item.SubItems[0].Text)
                            {
                                notFound--;
                                if (line.Split('#')[1] == "1")
                                    toBeChecked.Add(id);
                            }
                            id++;
                        }
                    }
                    foreach (int index in toBeChecked)
                        optionalsListView.Items[index].Checked = true;
                }

                sr.Close();
                if (notFound != 0)
                    o.Output(c.SubElText("Messages", "OptionalsPresetLoadFailed"));
            }
            else
                o.Output(c.SubElText("Messages", "OptionalsPresetLoadFailed"));
        }

        /// <summary>
        /// 将用户提供给复选框列表的答案保存到LauncherOptionalSettings.conf中
        /// </summary>
        public void SaveOptionalSettings()
        {
            StreamWriter sw = new StreamWriter("LauncherOptionalSettings.conf");
            foreach (ListViewItem item in optionalsListView.Items)
            {
                if (item.Checked)
                    sw.WriteLine(item.SubItems[0].Text + "#1");
                else
                    sw.WriteLine(item.SubItems[0].Text + "#0");
            }
            sw.Close();
        }
        #endregion

        #region Download list buildings methods...
        /// <summary>
        /// 检查需要下载哪些文件。忽略可选标志。
        /// 在FinalizeDownloadList方法中，正在从下载列表中删除未选中的可选文件。
        /// </summary>
        private void PreBuildDownloadList()
        {
            // 获取数据目录中的所有MPQ。
            var MPQFiles = Directory.GetFiles(cwd + @"\data\", "*.mpq", SearchOption.TopDirectoryOnly);
            // 检查数据目录中的所有MPQ。
            foreach (string mpqFile in MPQFiles)
            {
                // 忽略MPQ。注意，如果配置中的KeepBlizzlikeMPQs为0，则邻近MPQs列表为空，因此不会忽略任何文件。
                if (!blizzlikeMPQs.Contains(mpqFile.ToLower()))
                {
                    HandledFile onServer = null;
                    // 查找与此文件匹配的服务器端文件。
                    foreach (HandledFile hf in serverSideFiles)
                        if (hf.fullLocalPath.ToLower() == mpqFile.ToLower())
                            onServer = hf;
                    // 如果找不到匹配的服务器端文件，则应删除该文件、该文件已过期或来自其他项目。
                    if (onServer == null)
                        outdated.Add(mpqFile);
                    // 如果在服务器端找到匹配的文件，但其大小不同，则将下载新版本。
                    else
                        if (onServer.size != new FileInfo(mpqFile).Length)
                        toBeDownloaded.Add(onServer);
                }
            }

            // 检查服务器端的文件。如果它们在本地文件中丢失或过时，请将它们添加到下载列表中。
            foreach (HandledFile hf in serverSideFiles)
            {
                if (File.Exists(hf.fullLocalPath))
                {
                    if (new FileInfo(hf.fullLocalPath).Length != hf.size)
                        toBeDownloaded.Add(hf);
                }
                else
                    toBeDownloaded.Add(hf);
                // 如果没有ZIP名称的目录，也下载一个zip文件。
                if (hf.name.Contains('.'))
                    if (hf.name.Substring(hf.name.LastIndexOf('.')).ToLower() == ".zip")
                        if (!Directory.Exists(hf.fullLocalPath.Substring(0, hf.fullLocalPath.LastIndexOf('.'))))
                            toBeDownloaded.Add(hf);
            }

            // 获得不可选内容的总大小
            long totalSize = 0;
            foreach (HandledFile hf in toBeDownloaded)
            {
                if (!hf.optional)
                    totalSize += hf.size;
            }
            nonOptionalOutdatedSize = Size(totalSize);
        }

        /// <summary>
        /// 完成下载列表。
        /// </summary>
        public void PrepareUpdate()
        {
            CleanDownloadList();
            FinalizeDownloadList();
        }

        /// <summary>
        /// 从下载列表中清除所有可能的重复项。作为第一个添加到列表中的那些将被保留。
        /// </summary>
        private void CleanDownloadList()
        {
            List<HandledFile> duplicates = new List<HandledFile>();
            for (int i = 0; i < toBeDownloaded.Count; i++)
            {
                for (int j = i + 1; j < toBeDownloaded.Count; j++)
                {
                    if (toBeDownloaded[i].fullServerPath == toBeDownloaded[j].fullServerPath && !duplicates.Contains(toBeDownloaded[j]))
                        duplicates.Add(toBeDownloaded[j]);
                }
            }
            foreach (HandledFile hf in duplicates)
                toBeDownloaded.Remove(hf);
        }

        /// <summary>
        /// 根据可选复选框列表，检查需要下载哪些可选文件。
        /// </summary>
        private void FinalizeDownloadList()
        {
            // 如果未选中任何可选组，请从下载列表中删除其元素。
            foreach (OptionalGroup og in optionalGroups)
                if (!og.isChecked)
                    foreach (HandledFile hf in og.files)
                    {
                        HandledFile matching = null;
                        foreach (HandledFile file in toBeDownloaded)
                            if (file.serverPath == hf.serverPath)
                                matching = file;
                        if (matching != null)
                            toBeDownloaded.Remove(matching);
                        // MPQ文件作为可选文件下载，不再需要，也应删除。
                        if (File.Exists(hf.fullLocalPath))
                            if (hf.name.Contains('.'))
                                if (hf.name.Substring(hf.name.LastIndexOf('.')).ToLower() == ".mpq")
                                    outdated.Add(hf.fullLocalPath);
                    }
        }
        #endregion

        #region Update methods...
        /// <summary>
        /// 开始异步下载下载列表中的文件。也调用清理方法。
        /// </summary>
        public async Task Update()
        {
            filesDownloaded = 0;

            // 如果要强制执行realmlist，请尝试执行。
            EnforceRealmlist();
            SaveOptionalSettings();

            // 删除缓存，如果你应该这样做的话。
            if (c.SubElText("Main", "DeleteCache") == "1" && Directory.Exists(cwd + "\\Cache"))
                Directory.Delete(cwd + "\\Cache", true);

            // 删除所有不更新的过期（或外来）文件。但忽略备份。
            foreach (string s in outdated)
            {
                if (s[s.Length - 1] != '_')
                {
                    o.Output(c.SubElText("Messages", "Removing") + s);
                    if (File.Exists(s) && File.Exists(s + "_") && c.SubElText("Main", "KeepBackups") == "1")
                        File.Delete(s + "_");
                    if (File.Exists(s) && c.SubElText("Main", "KeepBackups") == "1")
                        File.Move(s, s + "_");
                    if (File.Exists(s) && c.SubElText("Main", "KeepBackups") != "1")
                        File.Delete(s);
                }
                else if (File.Exists(s) && c.SubElText("Main", "KeepBackups") != "1")
                {
                    o.Output(c.SubElText("Messages", "Removing") + s);
                    File.Delete(s);
                }
            }

            // 下载列表中的所有文件。
            foreach (HandledFile hf in toBeDownloaded)
            {
                await Task.WhenAll(DownloadFile(hf));
            }

            // 检查一切是否正常。
            if (filesDownloaded != toBeDownloaded.Count)
                o.Messagebox(c.SubElText("Messages", "FileDownloadError"));
        }

        /// <summary>
        /// 异步下载给定的文件。
        /// </summary>
        private async Task DownloadFile(HandledFile hf)
        {
            if (c.SubElText("Main", "FileProcessingOutputs") == "1")
            {
                o.Output(c.SubElText("Messages", "DownloadingFrom") + hf.fullServerPath, true);
                o.Output(c.SubElText("Messages", "DownloadingTo") + hf.fullLocalPath);
            }

            try
            {
                using (var client = new AmWebClient(3000))
                {
                    if (!Directory.Exists(cwd + hf.localPath))
                        Directory.CreateDirectory(cwd + hf.localPath);
                    if (File.Exists(hf.fullLocalPath) && File.Exists(hf.fullLocalPath + "_") && c.SubElText("Main", "KeepBackups") == "1")
                        File.Delete(hf.fullLocalPath + "_");
                    if (File.Exists(hf.fullLocalPath))
                        File.Move(hf.fullLocalPath, hf.fullLocalPath + "_");
                    await client.DownloadFileTaskAsync(hf.fullServerPath, hf.fullLocalPath);
                    if (IsFileOK(hf))
                    {
                        UnZip(hf.fullLocalPath);
                        filesDownloaded++;
                    }
                }
            }
            catch (WebException e) { o.Output(c.SubElText("Messages", "DownloadError") + hf.name, e); }
        }

        /// <summary>
        /// 将玩家的服务器登陆信息设置为服务器的设置。可以在配置的ForcedRealmlist属性中关闭。
        /// </summary>
        private void EnforceRealmlist()
        {
            string correctRealmlist = "";
            if (c.SubElText("Main", "ForcedRealmlist") == "1")
            {
                // 尝试从web上的realmlist.wtf获取数据。
                try { correctRealmlist = (new AmWebClient(3000)).DownloadString(c.SubElText("Paths", "FilesRootPath") + "realmlist.wtf"); }
                catch { o.Output(c.SubElText("Messages", "WebRealmlistMissing")); }

                // 尝试查找本地realmlist.wtf。如果找到了，就更新它。
                if (correctRealmlist != "")
                {
                    var realmlists = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\data\", "realmlist.wtf", SearchOption.AllDirectories);
                    if (realmlists.Length != 0)
                    {
                        StreamWriter sw = new StreamWriter(realmlists[0]);
                        sw.Write(correctRealmlist);
                        sw.Close();
                    }
                    else
                        o.Output(c.SubElText("Messages", "RealmlistMissing"));
                }
            }
        }

        /// <summary>
        ///解压给定路径中的文件（如果它是.zip文件）。
        /// 改进空间-实施RAR和7z支持。
        /// </summary>
        private void UnZip(string filePath)
        {
            if (File.Exists(filePath) && filePath.Substring(filePath.LastIndexOf('.')) == ".zip")
            {
                try
                {
                    if (c.SubElText("Main", "FileProcessingOutputs") == "1")
                        o.Output(c.SubElText("Messages", "UnzipingFile") + filePath);
                    Shell32.Shell objShell = new Shell32.Shell();
                    Shell32.Folder destinationFolder = objShell.NameSpace(filePath.Substring(0, filePath.LastIndexOf('\\') + 1));
                    Shell32.Folder sourceFile = objShell.NameSpace(filePath);

                    // 如果您不想看到正在显示的解压窗口，请切换到destinationFolder.CopyHere（zipFile，4 | 16）
                    foreach (var zipFile in sourceFile.Items())
                    {
                        destinationFolder.CopyHere(zipFile, 16);
                    }
                }
                catch (Exception e) { o.Output(c.SubElText("Messages", "UnZipingFileError") + filePath, e); }
            }
        }

        /// <summary>
        /// 删除客户端中的所有扩展备份文件。
        /// </summary>
        public void DeleteBackups()
        {
            var backups = Directory.GetFiles(cwd, "*_", SearchOption.AllDirectories);
            foreach (string s in backups)
                if (File.Exists(s))
                    File.Delete(s);
        }
        #endregion
    }

    /// <summary>
    ///文件列表处理的容器。
    /// </summary>
    public class HandledFile
    {
        // 从文件列表中获取的数据
        public string serverPath;             // 相对于文件根路径的路径
        public long size = 0;                 // 文件大小
        public bool optional = false;         // 文件是可选的吗？
        public string localPath = "";         // 相对于WoW根目录的路径
        public string linked = "";            // LinkedList字符串
        public string firstComment = "";      // 在文件列表中文件后面的行上找到的第一条注释
        // 生成数据以便于工作。
        public List<HandledFile> linkedList = new List<HandledFile>(); // LinkedList中引用的HandledFile实例列表
        public string name;                   // 文件名（前面没有任何相对路径）
        public string fullLocalPath;          // 完整的本地路径。
        public string fullServerPath;         // 文件的完整URL。

        public HandledFile(string serverPath, long size, string optional, string localPath, string linked, string firstComment)
        {
            // 保存从文件列表中获取的数据。
            this.serverPath = serverPath;
            this.size = size;
            if (optional == "0")
                this.optional = false;
            else
                this.optional = true;
            this.localPath = Regex.Replace(localPath, "/", "\\");
            this.linked = linked;
            this.firstComment = firstComment;

            // 根据从文件列表中获得的数据保存其他数据。
            if (serverPath.Contains('/'))
                name = serverPath.Substring(serverPath.LastIndexOf('/') + 1);
            else
                name = serverPath;
            fullLocalPath = Directory.GetCurrentDirectory() + this.localPath + name;
            fullServerPath = Config.Instance.SubElText("Paths", "FilesRootPath") + serverPath;
        }
    }

    /// <summary>
    ///链接在一起的可选文件组。
    /// </summary>
    public class OptionalGroup
    {
        public List<HandledFile> files = new List<HandledFile>(); // 组中包含的HandledFile实例的列表。
        public string name = "";                                  // 可选列表中显示用途的可选组的名称。
        public bool isChecked = false;                            // 可选组是否签入了可选列表？

        /// <summary>
        /// 基于组当前包含的元素生成并返回组的名称。如果未设置名称（来自主文件的第一个注释），则使用。
        /// </summary>
        public string GenerateName()
        {
            string generatedName = "";
            foreach (HandledFile hf in files)
            {
                if (hf.serverPath.Contains('/'))
                    generatedName += hf.serverPath.Substring(hf.serverPath.LastIndexOf('/') + 1) + ", ";
                else
                    generatedName += hf.serverPath + ", ";
            }
            generatedName = generatedName.Substring(0, generatedName.LastIndexOf(','));
            return generatedName;
        }

        /// <summary>
        /// 返回给定文件名在组中的位置。
        /// </summary>
        public bool Contains(string fullServerPath)
        {
            foreach (HandledFile hf in files)
                if (hf.fullServerPath == fullServerPath)
                    return true;
            return false;
        }

        /// <summary>
        /// 将文件添加到可选组中。
        /// </summary>
        public void Add(HandledFile file)
        {
            files.Add(file);
        }

        /// <summary>
        /// 返回可选组中元素的数量。
        /// </summary>
        public int Count()
        {
            return files.Count;
        }

        /// <summary>
        /// 返回可选组的总大小。
        /// </summary>
        public long GetTotalSize()
        {
            long result = 0;
            foreach (HandledFile hf in files)
                result += hf.size;
            return result;
        }

        /// <summary>
        /// 返回可选组中文件的总大小，但仅返回那些不是最新的文件的总大小。
        /// </summary>
        public long GetSizeForDownload(List<HandledFile> toBeDownloaded)
        {
            long result = 0;
            foreach (HandledFile hf in files)
            {
                foreach (HandledFile file in toBeDownloaded)
                    if (file.serverPath == hf.serverPath)
                        result += hf.size;
            }
            return result;
        }
    }
}