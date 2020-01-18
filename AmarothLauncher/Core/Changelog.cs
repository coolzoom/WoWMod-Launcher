using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace AmarothLauncher.Core
{
    public class Changelog
    {
        string path;
        Config c = Config.Instance;
        OutputWriter o = OutputWriter.Instance;
        XmlDocument xml = new XmlDocument();

        /// <summary>
        /// 创建新的变更日志实例，并从web获取变更日志的最新版本。
        /// </summary>
        public Changelog()
        {
            path = c.SubElText("Paths", "ChangelogPath");
            Initialize();
        }

        /// <summary>
        /// 初始化。尝试从web打开变更日志并将其加载为XML。
        /// </summary>
        private void Initialize()
        {
            string xmlString = "";

            try { xmlString = (new AmWebClient(3000)).DownloadString(path); }
            catch (WebException e)
            {
                o.Messagebox(c.SubElText("Messages", "ChangelogNotOpened"), e);
                xml = null;
                return;
            }

            try { xml.LoadXml(xmlString); }
            catch (Exception e)
            {
                o.Messagebox(c.SubElText("Messages", "ChangelogNotLoaded"), e);
                xml = null;
                return;
            }
        }

        #region Data getters...
        /// <summary>
        /// 返回给定项的说明文本。
        /// </summary>
        public string GetText(int id)
        {
            return xml.DocumentElement.ChildNodes[id].InnerText;
        }

        /// <summary>
        /// 返回给定项的标题文本。
        /// </summary>
        public string GetHeading(int id)
        {
            return xml.DocumentElement.ChildNodes[id].Attributes["heading"].Value;
        }

        /// <summary>
        /// 返回给定项的图片URL。
        /// </summary>
        public string GetPicture(int id)
        {
            return xml.DocumentElement.ChildNodes[id].Attributes["pictureURL"].Value;
        }

        /// <summary>
        /// 返回给定项的日期字符串。
        /// </summary>
        public string GetDate(int id)
        {
            return xml.DocumentElement.ChildNodes[id].Attributes["date"].Value;
        }

        /// <summary>
        /// 返回变更日志中的条目数。
        /// </summary>
        public int GetAmount()
        {
            if (xml != null)
                return xml.DocumentElement.ChildNodes.Count;
            else
                return 0;
        }
        #endregion

        #region Methods for modifying changelog...
        /// <summary>
        /// 更新给定元素的描述、图片URL、日期字符串和标题。
        /// </summary>
        public void UpdateElement(int id, string description, string pictureURL, string date, string heading)
        {
            xml.DocumentElement.ChildNodes[id].InnerText = description;
            xml.DocumentElement.ChildNodes[id].Attributes["pictureURL"].Value = pictureURL;
            xml.DocumentElement.ChildNodes[id].Attributes["date"].Value = date;
            xml.DocumentElement.ChildNodes[id].Attributes["heading"].Value = heading;
        }

        /// <summary>
        /// 创建具有给定说明、图片URL、日期字符串和标题的新条目。
        /// </summary>
        public void AddElement(string description, string pictureURL, string date, string heading)
        {
            if (xml == null)
            {
                o.Messagebox(c.SubElText("Messages", "ChangelogEmpty"));
                NewXml();
            }
            XmlNode node = xml.CreateElement("release");

            XmlAttribute head = xml.CreateAttribute("heading");
            head.Value = heading;
            node.Attributes.Append(head);

            XmlAttribute entry = xml.CreateAttribute("date");
            entry.Value = date;
            node.Attributes.Append(entry);

            XmlAttribute url = xml.CreateAttribute("pictureURL");
            url.Value = pictureURL;
            node.Attributes.Append(url);

            node.InnerText = description;

            xml.DocumentElement.AppendChild(node);

        }

        /// <summary>
        /// 从更改日志中删除给定项。
        /// </summary>
        public void RemoveElement(int id)
        {
            if (xml.ChildNodes.Count > id)
                xml.DocumentElement.RemoveChild(xml.DocumentElement.ChildNodes[id]);
        }
        #endregion

        #region Misc stuff...
        /// <summary>
        /// 按日期对变更日志排序（降序）。
        /// </summary>
        public void SortXml(string dateFormat)
        {
            if (xml.ChildNodes.Count > 0)
            {
                List<ChangelogEntry> releases = new List<ChangelogEntry>();
                foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                {
                    releases.Add(new ChangelogEntry(node.Attributes["date"].Value, dateFormat, node.Attributes["heading"].Value, node.Attributes["pictureURL"].Value, node.InnerText));
                }
                releases.Sort((x, y) => x.date.CompareTo(y.date));
                releases.Reverse();
                NewXml();
                foreach (ChangelogEntry rel in releases)
                {
                    AddElement(rel.text, rel.pictureURL, rel.date.ToString(dateFormat), rel.heading);
                }
            }
        }

        /// <summary>
        /// 将变更日志另存为XML文件。
        /// </summary>
        public void SaveXml()
        {
            TextWriter sw = new StreamWriter("changelog.xml", false, Encoding.UTF8);
            if (xml != null)
                xml.Save(sw);
            else
            {
                NewXml();
                xml.Save(sw);
            }
            sw.Close();
        }

        /// <summary>
        /// 重新创建空白的新变更日志。
        /// </summary>
        private void NewXml()
        {
            XmlDocument newXml = new XmlDocument();

            XmlDeclaration declaration = newXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = newXml.DocumentElement;
            newXml.InsertBefore(declaration, root);

            XmlElement releases = newXml.CreateElement("releases");
            newXml.AppendChild(releases);

            xml = newXml;
        }
        #endregion
    }

    /// <summary>
    ///容器，以便在对更改日志项进行排序时对其进行更轻松的管理。
    /// </summary>
    class ChangelogEntry
    {
        public DateTime date;
        public string heading;
        public string pictureURL;
        public string text;

        public ChangelogEntry(string date, string dateFormat, string heading, string pictureURL, string text)
        {
            this.date = DateTime.ParseExact(date, dateFormat, null);
            this.heading = heading;
            this.pictureURL = pictureURL;
            this.text = text;
        }
    }
}
