using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AmarothLauncher.Core;
using AmarothLauncher.GUI;
using System.IO;
using System.Globalization;

namespace AmarothLauncher
{
    public partial class MainWindow : Form
    {
        //用于发送输出和获取配置设置。
        OutputWriter o = OutputWriter.Instance;
        Config c = Config.Instance;
        FileHandler handler;

        public bool ftpCredsCorrect = false;
        public string login;
        public string password;

        public ChangelogBrowser changelogBrowser;
        public ChangelogEditor changelogEditor;
        public FTPLoginWindow ftpLoginWindow;

        string cwd = Directory.GetCurrentDirectory();

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 将窗口元素的值设置为在配置中设置的值。
        /// </summary>
        private void LoadConfig()
        {
            try { Icon = new Icon("LauncherIcon.ico"); } catch { }
            Text = c.SubElText("MainWindow", "WindowName");
            panelOutput.Text = c.SubElText("MainWindow", "OutputBox");
            panelOptional.Text = c.SubElText("MainWindow", "OptionalBox");
            checkUpdatesButt.Text = c.SubElText("MainWindow", "CheckForUpdatesButton");
            updateButt.Text = c.SubElText("MainWindow", "UpdateButton");
            webButt.Text = c.SubElText("MainWindow", "WebpageButton");
            regButt.Text = c.SubElText("MainWindow", "RegistrationButton");
            changelogEditButt.Text = c.SubElText("MainWindow", "ChangelogEditorButton");
            changelogBrowserButt.Text = c.SubElText("MainWindow", "ChangelogBrowserButton");
            launchButt.Text = c.SubElText("MainWindow", "LaunchButton");
            panelTotalSize.Text = c.SubElText("MainWindow", "PanelTotalSize");
            try { newsPictureBox.LoadAsync(c.SubElText("Paths", "HelloImage")); }
            catch { o.Output(c.SubElText("Messages", "HelloImageNotLoaded")); }
            progressLabel.Text = "";
            speedLabel.Text = "";
            percentLabel.Text = "";
            totalSizeLabel.Text = "";
        }

        #region Self-update methods...
        /// <summary>
        /// 检查Output.cs中的版本是否与VersionPath文件中的版本相同/更新。同时删除旧启动程序版本的可能备份。
        /// </summary>
        private bool IsUpToDate()
        {
            string versionOnWeb = null;
            try
            {
                using (var client = new AmWebClient(3000))
                    versionOnWeb = client.DownloadString(c.SubElText("Paths", "VersionPath"));
            }
            catch
            {
                o.Messagebox(c.SubElText("Messages", "VersionNotVerified"));
                return true;
            }
            double webVersion;
            if (Double.TryParse(versionOnWeb.Trim(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out webVersion))
                return webVersion <= c.version;
            else
            {
                o.Messagebox(c.SubElText("Messages", "VersionNotVerified"));
                return true;
            }
        }

        /// <summary>
        /// 重命名登录器的文件并下载新文件。解压缩并重启登录器。
        /// </summary>
        private void UpdateSelf()
        {
            string cwd = Directory.GetCurrentDirectory();
            string exeName = cwd + "\\" + AppDomain.CurrentDomain.FriendlyName;
            MessageBox.Show(c.SubElText("Messages", "OutdatedLauncher"));

            //清理可能出现的乱七八糟的东西。
            if (File.Exists(cwd + "\\OldLauncher.exe"))
                File.Delete(cwd + "\\OldLauncher.exe");
            if (File.Exists(cwd + "\\OldLauncherConfig.xml"))
                File.Delete(cwd + "\\OldLauncherConfig.xml");
            if (File.Exists(cwd + "\\OldLauncherIcon.ico"))
                File.Delete(cwd + "\\OldLauncherIcon.ico");

            // 备份当前文件。
            if (File.Exists(exeName))
                File.Move(exeName, cwd + "\\OldLauncher.exe");
            if (File.Exists(cwd + "\\LauncherConfig.xml"))
                File.Move(cwd + "\\LauncherConfig.xml", cwd + "\\OldLauncherConfig.xml");
            if (File.Exists(cwd + "\\LauncherIcon.ico"))
                File.Move(cwd + "\\LauncherIcon.ico", cwd + "\\OldLauncherIcon.ico");
            try
            {
                if (File.Exists(cwd + "\\Launcher.zip"))
                    File.Delete(cwd + "\\Launcher.zip");
                // 下载新登录器.
                using (var client = new AmWebClient(3000))
                {
                    client.DownloadFile(c.SubElText("Paths", "LauncherPath"), cwd + "\\Launcher.zip");
                }

                // 解压新登录器.
                Shell32.Shell objShell = new Shell32.Shell();
                Shell32.Folder destinationFolder = objShell.NameSpace(cwd);
                Shell32.Folder sourceFile = objShell.NameSpace(cwd + "\\Launcher.zip");
                foreach (var zipFile in sourceFile.Items())
                {
                    destinationFolder.CopyHere(zipFile, 4 | 16);
                }

                // 删除压缩包文件.
                File.Delete(cwd + "\\Launcher.zip");

                // 启动新登录器.
                Process.Start(exeName);
                newsPictureBox.CancelAsync();
                Close();
            }
            catch (Exception e)
            {
                // 改回原来的状态
                if (File.Exists(cwd + "\\OldLauncher.exe"))
                    File.Move(cwd + "\\OldLauncher.exe", exeName);
                if (File.Exists(cwd + "\\OldLauncherConfig.xml"))
                    File.Move(cwd + "\\OldLauncherConfig.xml", cwd + "\\LauncherConfig.xml");
                if (File.Exists(cwd + "\\OldLauncherIcon.ico"))
                    File.Move(cwd + "\\OldLauncherIcon.ico", cwd + "\\LauncherIcon.ico");
                o.Messagebox("CouldNotBeUpdated", e);
                updateButt.Enabled = false;
                launchButt.Enabled = false;
            }
        }
        #endregion

        #region Progress and size labels updates...
        /// <summary>
        /// 下载运行时，调用UI更新速度和进度。
        /// </summary>
        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateProgressLabel();
            UpdateSpeedAndTime();
            UpdatePercentage();
        }

        /// <summary>
        /// 将进度标签设置为与已下载的文件量匹配。
        /// </summary>
        private void UpdateProgressLabel()
        {
            progressLabel.Text = c.SubElText("MainWindow", "ProgressText") + (handler.filesDownloaded + 1) +
                c.SubElText("MainWindow", "ProgressSeparator") + handler.toBeDownloaded.Count;
        }

        long previousSize;
        long actualSize;
        long neededSize;
        long downloadSpeed;
        /// <summary>
        /// 将速度标签设置为当前下载速度和估计剩余时间。
        /// 下载速度测量并不是非常有效和准确的方法。
        /// </summary>
        private void UpdateSpeedAndTime()
        {
            previousSize = actualSize;
            actualSize = 0;
            foreach (HandledFile hf in handler.toBeDownloaded)
                if (File.Exists(hf.fullLocalPath))
                    actualSize += new FileInfo(hf.fullLocalPath).Length;
            downloadSpeed = actualSize - previousSize;
            speedLabel.Text = handler.Size(downloadSpeed) + c.SubElText("MainWindow", "DownloadSpeedUnits")
                + TimeRemaining();
        }

        /// <summary>
        /// 使用已下载百分比、已下载大小和剩余大小更新标签。
        /// </summary>
        private void UpdatePercentage()
        {
            if (neededSize > 0)
            {
                percentLabel.Text = (int)(100.0 * actualSize / neededSize) + "% (" + handler.Size(actualSize) + " " +
                    c.SubElText("MainWindow", "downloaded") + handler.Size(neededSize - actualSize) + " " +
                    c.SubElText("MainWindow", "remaining") + ")";
                if (100.0 * actualSize / neededSize <= 100)
                    progressBar.Value = (int)(100.0 * actualSize / neededSize);
            }
        }

        /// <summary>
        /// 以最合理的单位返回剩余时间的字符串版本。
        /// </summary>
        private string TimeRemaining()
        {
            if (downloadSpeed > 0)
            {
                string result = "";
                long remainingTime = (neededSize - actualSize) / downloadSpeed;
                if (remainingTime < 60)
                    result += remainingTime + c.SubElText("MainWindow", "second");
                else if (remainingTime < 3600)
                    result += (remainingTime / 60) + c.SubElText("MainWindow", "minute") + (remainingTime % 60) + c.SubElText("MainWindow", "second");
                else
                    result += (remainingTime / 3600) + c.SubElText("MainWindow", "hour") + (remainingTime % 3600) + c.SubElText("MainWindow", "minute");

                return result + c.SubElText("MainWindow", "remaining");
            }
            else
                return "? " + c.SubElText("MainWindow", "remaining");
        }

        /// <summary>
        /// 使用过时/丢失的尺寸更新标签，并选择可选尺寸和非可选尺寸。
        /// </summary>
        private void UpdateOptionalSizeLabel()
        {
            long totalOptionalSize = 0;
            foreach (OptionalGroup og in handler.optionalGroups)
            {
                if (og.isChecked)
                    totalOptionalSize += og.GetSizeForDownload(handler.toBeDownloaded);
            }
            totalSizeLabel.Text = c.SubElText("MainWindow", "LabelTotalSizeOpt") + handler.Size(totalOptionalSize);
            totalSizeLabel.Text += "\n" + c.SubElText("MainWindow", "LabelTotalSizeNonOpt") + handler.nonOptionalOutdatedSize;
        }
        #endregion

        #region Event handlers...
        private void checkUpdatesButt_Click(object sender, EventArgs e)
        {
            if (File.Exists(cwd + "\\OldLauncher.exe"))
                File.Delete(cwd + "\\OldLauncher.exe");
            if (File.Exists(cwd + "\\OldLauncherConfig.xml"))
                File.Delete(cwd + "\\OldLauncherConfig.xml");
            if (File.Exists(cwd + "\\OldLauncherIcon.ico"))
                File.Delete(cwd + "\\OldLauncherIcon.ico");
            progressLabel.Text = "";
            speedLabel.Text = "";
            percentLabel.Text = "";
            totalSizeLabel.Text = "";
            o.Reset();
            optionalsListView.Clear();
            handler = new FileHandler();
            handler.optionalsListView = optionalsListView;
            updateButt.Enabled = false;
            launchButt.Enabled = false;
            if (handler.IsInWowDir())
            {
                if (!c.isDefaultConfigUsed)
                    if (handler.CheckForUpdates())
                    {
                        updateButt.Enabled = true;
                        UpdateOptionalSizeLabel();
                    }
                // 用于调试目的的输出。在发行版中保留他们的评论。
                // handler.OutputServerFilelist();
                // handler.OutputOptionalGroups();
            }
            else
                o.Messagebox(c.SubElText("Messages", "LauncherNotInWowDir"));
        }

        private async void updateButt_Click(object sender, EventArgs e)
        {
            neededSize = 0;
            actualSize = 0;
            previousSize = 0;
            downloadSpeed = 0;
            updateButt.Enabled = false;
            checkUpdatesButt.Enabled = false;
            handler.PrepareUpdate();
            neededSize = handler.SizeToBeDownloaded();
            timer.Start();
            await handler.Update();
            progressBar.Value = 100;
            timer.Stop();
            if (handler.filesDownloaded == handler.toBeDownloaded.Count)
            {
                launchButt.Enabled = true;
                progressLabel.Text = c.SubElText("MainWindow", "Complete");
            }
            else
                progressLabel.Text = c.SubElText("MainWindow", "Errors");
            checkUpdatesButt.Enabled = true;
            speedLabel.Text = "";
            percentLabel.Text = "";
            totalSizeLabel.Text = "";
        }

        private void launchButt_Click(object sender, EventArgs e)
        {
            if (File.Exists("wow.exe"))
            {
                Process.Start("wow.exe");
                Close();
            }
            else
                o.Messagebox(c.SubElText("Messages", "WowExeMissing"));
        }

        private void webButt_Click(object sender, EventArgs e)
        {
            Process.Start(c.SubElText("Paths", "Webpage"));
        }

        private void regButt_Click(object sender, EventArgs e)
        {
            Process.Start(c.SubElText("Paths", "Registration"));
        }

        private void launcherInfoButt_Click(object sender, EventArgs e)
        {
            Process.Start(c.SubElText("Paths", "Instructions"));
        }

        private void delBackButt_Click(object sender, EventArgs e)
        {
            handler.DeleteBackups();
        }

        /// <summary>
        /// 检查是否已成功输入FTP的登录名和密码。
        /// 如果是，则打开更改日志编辑器。否则将创建/显示登录对话框。
        /// </summary>
        public void changelogEditButt_Click(object sender, EventArgs e)
        {
            if (ftpCredsCorrect)
            {
                if (changelogEditor == null)
                    changelogEditor = new ChangelogEditor();
                if (changelogEditor.WindowState == FormWindowState.Minimized)
                    changelogEditor.WindowState = FormWindowState.Normal;
                else
                    changelogEditor.Show();
                changelogEditor.BringToFront();
                changelogEditor.mainWindow = this;
            }
            else
            {
                if (ftpLoginWindow == null)
                    ftpLoginWindow = new FTPLoginWindow();
                if (ftpLoginWindow.WindowState == FormWindowState.Minimized)
                    ftpLoginWindow.WindowState = FormWindowState.Normal;
                else
                    ftpLoginWindow.Show();
                ftpLoginWindow.BringToFront();
                ftpLoginWindow.mainWindow = this;
            }
        }

        private void changelogBrowserButt_Click(object sender, EventArgs e)
        {
            if (changelogBrowser == null)
                changelogBrowser = new ChangelogBrowser();
            if (changelogBrowser.WindowState == FormWindowState.Maximized)
                changelogBrowser.WindowState = FormWindowState.Normal;
            else
                changelogBrowser.Show();
            changelogBrowser.BringToFront();
            changelogBrowser.mainWindow = this;
        }

        private void output_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void optionalsListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (optionalsListView.GetItemAt(e.X, e.Y) != null && e.X > 14)
            {
                optionalsListView.GetItemAt(e.X, e.Y).Checked = !optionalsListView.GetItemAt(e.X, e.Y).Checked;
                handler.optionalGroups[optionalsListView.GetItemAt(e.X, e.Y).Index].isChecked = optionalsListView.GetItemAt(e.X, e.Y).Checked;
            }
        }

        private void optionalsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            handler.optionalGroups[e.Item.Index].isChecked = optionalsListView.Items[e.Item.Index].Checked;
            UpdateOptionalSizeLabel();
        }

        private void newsPictureBox_Click(object sender, EventArgs e)
        {
            changelogBrowserButt_Click(null, null);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            OutputWriter.outputBox = output;
            LoadConfig();
            if (IsUpToDate())
                checkUpdatesButt_Click(null, null);
            else
                UpdateSelf();
            // c.OutputContent();
        }
        #endregion

        private void MainWindow_DragLeave(object sender, EventArgs e)
        {

        }

        private void panelMain_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void panelMain_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelLeftMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelMain_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
