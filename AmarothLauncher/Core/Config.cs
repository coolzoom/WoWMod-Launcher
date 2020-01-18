using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace AmarothLauncher.Core
{
    /// <summary>
    /// Singleton负责获取配置设置或生成默认设置。
    /// </summary>
    public class Config
    {
        private static Config instance;

        // 登录器版本
        public double version = 1.1;
        public bool isDefaultConfigUsed { get; private set; }
        XmlDocument xml = new XmlDocument();
        XmlDocument defaultXml = new XmlDocument();
        OutputWriter o = OutputWriter.Instance;

        private Config()
        {
            Initialize();
        }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                    instance = new Config();
                return instance;
            }
        }
        /// <summary>
        /// 生成LauncherConfig.XML的对象XML结构。
        /// 如果读取XML失败，则使用默认设置。
        /// </summary>
        private void Initialize()
        {
            isDefaultConfigUsed = false;
            GenerateDefault();
            if (!File.Exists("LauncherConfig.xml"))
                UseDefault();
            else
            {
                StreamReader sr = new StreamReader("LauncherConfig.xml");
                string xmlString = sr.ReadToEnd();
                sr.Close();
                xml.LoadXml(xmlString);
            }
        }

        /// <summary>
        /// 使用默认配置XML作为当前配置XML。
        /// </summary>
        private void UseDefault()
        {
            xml = defaultXml;
            o.Messagebox(SubElText("Messages", "XmlNotOpened"));

            // 将默认配置另存为新的配置XML。用于生成XML以便以后可以编辑它，不要在发行版中取消注释！
            // SaveDefault();

            isDefaultConfigUsed = true;
        }

        /// <summary>
        ///输出整个配置内容。仅用于调试。
        /// </summary>
        public void OutputContent()
        {
            o.Output("Outputing all values set in Config for debugging purpouses. * marks attributes (followed by their names), \"\" mark values.");
            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                o.Output(node.Name, true);
                foreach (XmlNode att in node.ChildNodes)
                    o.Output("* " + att.Name + " - \"" + att.InnerText + "\"");
            }
        }

        /// <summary>
        /// 返回给定元素的给定子元素的内部文本。如果找不到任何内容，将显示错误消息并返回空字符串。
        /// </summary>
        public string SubElText(string elementName, string subElementName)
        {
            if (xml.GetElementsByTagName(elementName).Count > 0)
            {
                foreach (XmlNode node in xml.GetElementsByTagName(elementName)[0].ChildNodes)
                    if (node.Name == subElementName)
                        return node.InnerText;
            }
            else
                o.Output(elementName + " element was not found in config. This may cause critical errors.");

            o.Output(subElementName + " attribute was not found in config. This may cause critical errors.");
            return "";
        }

        /// <summary>
        ///返回配置XML中给定元素的内部文本。如果找不到，则返回空字符串并输出错误。
        /// </summary>
        public string InnText(string elementName)
        {
            if (xml.GetElementsByTagName(elementName).Count > 0)
                return xml.GetElementsByTagName(elementName)[0].InnerText;
            else
            {
                o.Output(elementName + " element was not found in config. This may cause critical errors.");
                return "";
            }
        }

        #region Default config generation...
        /// <summary>
        /// 生成新的默认配置。仅当找不到配置时才使用。
        /// </summary>
        private void GenerateDefault()
        {
            XmlDeclaration declaration = defaultXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = defaultXml.DocumentElement;
            defaultXml.InsertBefore(declaration, root);
            XmlElement configs = defaultXml.CreateElement("Config");
            XmlComment comment = defaultXml.CreateComment("This is a config file for Launcher. Feel free to edit whatever you want or even translate Launcher to your native language. Pay close attention to comments and documentation.");
            defaultXml.AppendChild(comment);
            defaultXml.AppendChild(configs);

            MainSettingsDefault();
            PathConfigDefault();
            MainWindowDefault();
            ChangelogEditorDefault();
            ChangelogBrowserDefault();
            FTPLoginWindowDefault();
            MessagesDefault();
        }

        /// <summary>
        /// 在节点下添加具有给定名称和内部文本的新子节点。
        /// </summary>
        private void AddSubnodeDefault(XmlNode node, string name, string value)
        {
            AddSubnodeDefault(node, name, value, "");
        }

        /// <summary>
        /// 在节点下添加具有给定名称和内部文本的新子节点。如果不是空的，也创建一个给定的注释。
        /// </summary>
        private void AddSubnodeDefault(XmlNode node, string name, string value, string comment)
        {
            XmlNode newNode = defaultXml.CreateElement(name);
            if (comment != "" && comment != null)
            {
                XmlComment newComment = defaultXml.CreateComment(name + ": " + comment);
                node.AppendChild(newComment);
            }
            newNode.InnerText = value;
            node.AppendChild(newNode);
        }

        /// <summary>
        /// web路径的默认配置。
        /// </summary>
        private void MainSettingsDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Main settings of an application.");
            XmlNode node = defaultXml.CreateElement("Main");

            AddSubnodeDefault(node, "DeleteCache", "1", "1 or 0. If 1, Cache folder is always deleted.");
            AddSubnodeDefault(node, "KeepBackups", "1", "1 or 0. If 1, .ext_ files are being kept as backups. Recommended 1.");
            AddSubnodeDefault(node, "KeepBlizzlikeMPQs", "1", "1 or 0. If 1, blizzlike MPQs in Data are ignored by Launcher. Otherwise they are handled in a same manner as custom one. Recommended 1.");
            AddSubnodeDefault(node, "ForcedRealmlist", "1", "1 or 0. If 1, realmlist.wtf will always be updated to match realmlist.wtf in FilesRootPath.");
            AddSubnodeDefault(node, "FileProcessingOutputs", "1", "1 or 0. If 1, messages about downloading and unziping files will be shown.");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// web路径的默认配置。
        /// </summary>
        private void PathConfigDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Paths to files and folders Launcher will work with. They are commonly all !CASE SENSITIVE!");
            XmlNode node = defaultXml.CreateElement("Paths");

            AddSubnodeDefault(node, "FilelistPath", "http://www.xxx.com/LauncherWebFolder/filelist.conf", "Path to text filelist.");
            AddSubnodeDefault(node, "VersionPath", "http://www.xxx.com//LauncherWebFolder/launcherversion.conf", "Path to text file which contains your Lancher's current version (version is a double value with . as separator!");
            AddSubnodeDefault(node, "LauncherPath", "http://www.xxx.com/LauncherWebFolder/Launcher.zip", "Path to a zip file with Launcher files - used if Launcher finds itself outdated.");
            AddSubnodeDefault(node, "FilesRootPath", "/http://www.xxx.com/LauncherWebFolder/", "Path to folder with files. Paths in filelist are relative to this path.");
            AddSubnodeDefault(node, "ChangelogPath", "http://www.xxx.com/LauncherWebFolder/changelog.xml", "!HTTP! path to changelog XML file.");
            AddSubnodeDefault(node, "ChangelogFTPPath", "ftp://ftp.www.xxx.com/", "!Full! !FTP! path to folder in which changelog is. Notice that //www/ part. You may want to use an IP instead of a domain name.");
            AddSubnodeDefault(node, "Webpage", "http://www.xxx.com/", "URL which is to be opened when user clicks on Project webpage button.");
            AddSubnodeDefault(node, "Registration", "http://www.xxx.com/", "URL which is to be opened when user clicks on Registration button.");
            AddSubnodeDefault(node, "Instructions", "http://www.xxx.com/", "URL which is to be opened when user clicks on Launcher manual button.");
            AddSubnodeDefault(node, "HelloImage", "http://www.xxx.com/LauncherWebFolder/hello.jpg", "URL to image which is to be displayed in Main window (latest news image). Clicking on it opens a changelog browser.");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// 主窗口的默认配置。
        /// </summary>
        private void MainWindowDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Visual settings for Main Window.");
            XmlNode node = defaultXml.CreateElement("MainWindow");

            AddSubnodeDefault(node, "WindowName", "MoonSun登录器 " + version.ToString("F", CultureInfo.InvariantCulture), "MoonSun登录器");
            AddSubnodeDefault(node, "OutputBox", "温馨提示:");
            AddSubnodeDefault(node, "OptionalBox", "可选文件:");
            AddSubnodeDefault(node, "CheckForUpdatesButton", "检查更新");
            AddSubnodeDefault(node, "UpdateButton", "更新");
            AddSubnodeDefault(node, "WebpageButton", "主页");
            AddSubnodeDefault(node, "RegistrationButton", "账号注册");
            AddSubnodeDefault(node, "ChangelogEditorButton", "日志编辑器");
            AddSubnodeDefault(node, "ChangelogBrowserButton", "日志浏览");
            AddSubnodeDefault(node, "LaunchButton", "进入游戏");
            AddSubnodeDefault(node, "ProgressText", "正在下载: ");
            AddSubnodeDefault(node, "ProgressSeparator", " / ");
            AddSubnodeDefault(node, "DownloadSpeedUnits", "/s, ");
            AddSubnodeDefault(node, "remaining", "剩余");
            AddSubnodeDefault(node, "downloaded", "已下载, ");
            AddSubnodeDefault(node, "ToolTipTotalSize", "总大小: ");
            AddSubnodeDefault(node, "PanelTotalSize", "需更新文件大小:");
            AddSubnodeDefault(node, "LabelTotalSizeOpt", "已选择: ");
            AddSubnodeDefault(node, "LabelTotalSizeNonOpt", "未选择: ");
            AddSubnodeDefault(node, "second", " s ");
            AddSubnodeDefault(node, "minute", " m ");
            AddSubnodeDefault(node, "hour", " h ");
            AddSubnodeDefault(node, "Complete", "下载完毕!");
            AddSubnodeDefault(node, "Errors", "发生错误!");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// 更改日志编辑器的默认配置。
        /// </summary>
        private void ChangelogEditorDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Visual settings for Changelog Editor. A lot of those are used by Changelog Browser as well.");
            XmlNode node = defaultXml.CreateElement("ChangelogEditor");

            AddSubnodeDefault(node, "WindowName", "Changelog Editor");
            AddSubnodeDefault(node, "ChangelogEntries", "Changelog entries:");
            AddSubnodeDefault(node, "DateColumn", "Date");
            AddSubnodeDefault(node, "HeadingColumn", "Heading");
            AddSubnodeDefault(node, "Date", "Date:");
            AddSubnodeDefault(node, "DateFormat", "dd.MM.yyyy hh:mm", "Carefully with this. MM for months, mm for minutes. You can use your own format, but it must be correct. Changelog's data must also be compatible with this, if your changelog isn't empty when this is being changed!");
            AddSubnodeDefault(node, "PictureURL", "Picture URL:");
            AddSubnodeDefault(node, "Heading", "Heading:");
            AddSubnodeDefault(node, "PicturePreview", "Picture preview:");
            AddSubnodeDefault(node, "Description", "Description:");
            AddSubnodeDefault(node, "EditEntryButton", "Edit entry");
            AddSubnodeDefault(node, "DeleteEntryButton", "Delete entry");
            AddSubnodeDefault(node, "CreateEntryButton", "Create entry");
            AddSubnodeDefault(node, "SaveEntryButton", "Save entry");
            AddSubnodeDefault(node, "TestPictureButton", "Test picture");
            AddSubnodeDefault(node, "CancelButton", "Cancel changes");
            AddSubnodeDefault(node, "SaveButton", "Save changelog");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// 更改日志浏览器的默认配置。许多设置都是从changelog编辑器中使用的。
        /// </summary>
        private void ChangelogBrowserDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Visual settings for Changelog Browser.");
            XmlNode node = defaultXml.CreateElement("ChangelogBrowser");

            AddSubnodeDefault(node, "WindowName", "Changelog Browser");
            AddSubnodeDefault(node, "InfoText", "Click on an entry in entries list in order to display it.");
            AddSubnodeDefault(node, "Picture", "Picture:");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// FTP登录窗口的默认配置。
        /// </summary>
        private void FTPLoginWindowDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Visual settings for authentization dialog window for Changelog Editor.");
            XmlNode node = defaultXml.CreateElement("FTPLoginWindow");

            AddSubnodeDefault(node, "WindowName", "Login to FTP");
            AddSubnodeDefault(node, "Login", "Login:");
            AddSubnodeDefault(node, "Password", "Password:");
            AddSubnodeDefault(node, "OKButton", "OK");
            AddSubnodeDefault(node, "CancelButton", "Cancel");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// 错误（和其他）消息。如果要在消息后面直接输出某些内容（如文件名），请在消息后面留出空间。
        /// </summary>
        private void MessagesDefault()
        {
            XmlComment comment = defaultXml.CreateComment("Various messages which can be output by Launcher.");
            XmlNode node = defaultXml.CreateElement("Messages");

            AddSubnodeDefault(node, "HelloMessage", "登录器程序由MoonSun编写。如果您想添加任何内容，请联系GM。");
            AddSubnodeDefault(node, "XmlNotOpened", "正在使用默认设置，因为无法加载LauncherConfig.xml，登录器不能继续运行，请添加新的配置文件!\n");
            AddSubnodeDefault(node, "ChangelogNotOpened", "打开更改日志文件失败。");
            AddSubnodeDefault(node, "ChangelogNotLoaded", "加载更改日志数据失败。请联系GM。");
            AddSubnodeDefault(node, "ChangelogEmpty", "警告：更改日志为空。您当前正在创建一个新的。");
            AddSubnodeDefault(node, "InvalidFtpPassword", "不正确的登录密码组合，或不正确的FTP路径改变记录。");
            AddSubnodeDefault(node, "PictureNotOpened", "无法打开给定URL中的图片。URL似乎无效。");
            AddSubnodeDefault(node, "ChangelogNotUploaded", "无法上载更改日志。备份XML文件可以在启动程序的目录中找到。");
            AddSubnodeDefault(node, "ChangelogUploadOK", "已成功更新更改日志。");
            AddSubnodeDefault(node, "UnZipingFileError", "无法解压缩文件： ");
            AddSubnodeDefault(node, "DownloadingFrom", "正在从以下位置下载文件： ");
            AddSubnodeDefault(node, "DownloadingTo", "To: ");
            AddSubnodeDefault(node, "UnzipingFile", "解压文件： ");
            AddSubnodeDefault(node, "FileDeletingError", "未能删除文件： ");
            AddSubnodeDefault(node, "WowExeMissing", "找不到WoW.exe！");
            AddSubnodeDefault(node, "DataDirMissing", "找不到数据目录！");
            AddSubnodeDefault(node, "BlizzlikeMpqMissing", "找不到基本文件： ");
            AddSubnodeDefault(node, "LauncherNotInWowDir", "您客户端中的WoW.exe已损坏，或不在客户端目录中。");
            AddSubnodeDefault(node, "FilelistOpeningFailed", "登录器无法打开服务器的文件列表。请检查您的internet连接，或与GM联系。错误消息： ");
            AddSubnodeDefault(node, "FilelistReadingFailed", "服务器的文件列表无效。联系GM。");
            AddSubnodeDefault(node, "FileOnsWebMissing", "找不到文件容量。服务器上可能缺少文件。 ");
            AddSubnodeDefault(node, "WebRealmlistMissing", "在web上找不到文件realmlist.wtf。无法验证Realmlist。");
            AddSubnodeDefault(node, "RealmlistMissing", "似乎缺少客户端中的realmlist.wtf。如果是的话，创建一个新的。");
            AddSubnodeDefault(node, "OptionalsPresetLoadFailed", "您没有保存可选的组选择，或者组列表已更改。请注意“可选文件”复选框列表，然后单击“更新”按钮。");
            AddSubnodeDefault(node, "DownloadError", "下载下列文件时出错： ");
            AddSubnodeDefault(node, "FileDownloadError", "有些文件显然没有成功下载。重新运行更新和更新检查。");
            AddSubnodeDefault(node, "HelloImageNotLoaded", "无法加载新闻图像。");
            AddSubnodeDefault(node, "VersionNotVerified", "登录器无法验证版本是否为最新，但仍将正常运行，如果存在问题，请联系GM。");
            AddSubnodeDefault(node, "CouldNotBeUpdated", "登录器已尝试更新，但未成功。尝试重新运行登录器，如果问题仍然存在，请与GM联系。");
            AddSubnodeDefault(node, "OutdatedLauncher", "服务器有新版本程序，将尝试更新，然后重新启动。");
            AddSubnodeDefault(node, "LauncherUpdated", "登录器已成功更新，请再次运行。");
            AddSubnodeDefault(node, "Removing", "正在删除文件： ");

            defaultXml.DocumentElement.AppendChild(comment);
            defaultXml.DocumentElement.AppendChild(node);
        }

        /// <summary>
        /// 将默认XML另存为new LauncherConfig.XML，覆盖旧的。
        /// </summary>
        private void SaveDefault()
        {
            TextWriter tw = new StreamWriter("LauncherConfig.xml", false, Encoding.UTF8);
            defaultXml.Save(tw);
            tw.Close();
        }
        #endregion
    }
}