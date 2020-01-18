using AmarothLauncher.Core;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AmarothLauncher.GUI
{
    public partial class ChangelogBrowser : Form
    {
        OutputWriter o = OutputWriter.Instance;
        Config c = Config.Instance;
        public MainWindow mainWindow;

        Changelog changelog;

        public ChangelogBrowser()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 将窗口元素的值设置为在配置中设置的值。
        /// </summary>
        private void LoadConfig()
        {
            Icon = new Icon("LauncherIcon.ico");
            Text = c.SubElText("ChangelogBrowser", "WindowName");
            descriptionBox.Text = c.SubElText("ChangelogBrowser", "InfoText");
            panelPicture.Text = c.SubElText("ChangelogBrowser", "Picture");

            panelList.Text = c.SubElText("ChangelogEditor", "ChangelogEntries");
            listBox.Columns[0].Text = c.SubElText("ChangelogEditor", "DateColumn");
            listBox.Columns[1].Text = c.SubElText("ChangelogEditor", "HeadingColumn");
            panelDate.Text = c.SubElText("ChangelogEditor", "Date");
            dateBox.CustomFormat = c.SubElText("ChangelogEditor", "DateFormat");
            panelHeading.Text = c.SubElText("ChangelogEditor", "Heading");
            panelDescription.Text = c.SubElText("ChangelogEditor", "Description");
        }

        /// <summary>
        /// 通过获取新版本的changelog更新列表。
        /// 希望它不会对网速慢的用户太慢，请随意评论它在listBox_ItemCheck event method中的调用。
        /// </summary>
        private void UpdateList()
        {
            changelog = new Changelog();
            listBox.Items.Clear();
            for (int i = 0; i < changelog.GetAmount(); i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = changelog.GetDate(i);
                lvi.ToolTipText = changelog.GetText(i);
                lvi.SubItems.Add(new ListViewItem.ListViewSubItem(lvi, changelog.GetHeading(i)));
                listBox.Items.Add(lvi);
            }
        }

        #region Event handlers...
        private void ChangelogBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            mainWindow.changelogBrowser = null;
        }

        /// <summary>
        /// 如果选择了任何条目，则显示有关选定条目的详细信息。
        /// </summary>
        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count == 1)
            {
                dateBox.Value = DateTime.ParseExact(changelog.GetDate(listBox.SelectedItems[0].Index), dateBox.CustomFormat, null);
                try
                {
                    pictureBox.CancelAsync();
                    pictureBox.LoadAsync(changelog.GetPicture(listBox.SelectedItems[0].Index));
                }
                catch { }
                descriptionBox.Text = changelog.GetText(listBox.SelectedItems[0].Index);
                headingBox.Text = changelog.GetHeading(listBox.SelectedItems[0].Index);
            }
        }

        private void descriptionBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void listBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpdateList();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItems.Count != 0)
                try { Process.Start(changelog.GetPicture(listBox.SelectedItems[0].Index)); }
                catch { }
        }

        private void ChangelogBrowser_Load(object sender, EventArgs e)
        {
            LoadConfig();
            UpdateList();
        }
        #endregion
    }
}
