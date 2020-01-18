namespace AmarothLauncher
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// 设计器支持所需的方法-不要使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Test");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.panelMain = new System.Windows.Forms.SplitContainer();
            this.panelLeftMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelBottomLeft = new System.Windows.Forms.TableLayoutPanel();
            this.panelProgress = new System.Windows.Forms.TableLayoutPanel();
            this.percentLabel = new System.Windows.Forms.Label();
            this.speedLabel = new System.Windows.Forms.Label();
            this.progressLabel = new System.Windows.Forms.Label();
            this.updateButt = new System.Windows.Forms.Button();
            this.checkUpdatesButt = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.newsPictureBox = new System.Windows.Forms.PictureBox();
            this.panelOutput = new System.Windows.Forms.GroupBox();
            this.output = new System.Windows.Forms.RichTextBox();
            this.panelRight = new System.Windows.Forms.TableLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.changelogEditButt = new System.Windows.Forms.Button();
            this.regButt = new System.Windows.Forms.Button();
            this.panelOptional = new System.Windows.Forms.GroupBox();
            this.optionalsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.launchButt = new System.Windows.Forms.Button();
            this.changelogBrowserButt = new System.Windows.Forms.Button();
            this.panelTotalSize = new System.Windows.Forms.GroupBox();
            this.totalSizeLabel = new System.Windows.Forms.Label();
            this.webButt = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).BeginInit();
            this.panelMain.Panel1.SuspendLayout();
            this.panelMain.Panel2.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelLeftMain.SuspendLayout();
            this.panelBottomLeft.SuspendLayout();
            this.panelProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.newsPictureBox)).BeginInit();
            this.panelOutput.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelOptional.SuspendLayout();
            this.panelTotalSize.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            // 
            // panelMain.Panel1
            // 
            this.panelMain.Panel1.Controls.Add(this.panelLeftMain);
            this.panelMain.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Panel1_Paint);
            // 
            // panelMain.Panel2
            // 
            this.panelMain.Panel2.BackColor = System.Drawing.Color.White;
            this.panelMain.Panel2.Controls.Add(this.panelRight);
            this.panelMain.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Panel2_Paint);
            this.panelMain.Size = new System.Drawing.Size(884, 519);
            this.panelMain.SplitterDistance = 663;
            this.panelMain.SplitterWidth = 1;
            this.panelMain.TabIndex = 11;
            this.panelMain.TabStop = false;
            this.panelMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.panelMain_SplitterMoved);
            // 
            // panelLeftMain
            // 
            this.panelLeftMain.BackColor = System.Drawing.Color.White;
            this.panelLeftMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panelLeftMain.ColumnCount = 1;
            this.panelLeftMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeftMain.Controls.Add(this.panelBottomLeft, 0, 2);
            this.panelLeftMain.Controls.Add(this.progressBar, 0, 1);
            this.panelLeftMain.Controls.Add(this.splitContainer1, 0, 0);
            this.panelLeftMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeftMain.Location = new System.Drawing.Point(0, 0);
            this.panelLeftMain.Name = "panelLeftMain";
            this.panelLeftMain.RowCount = 3;
            this.panelLeftMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeftMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.panelLeftMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.panelLeftMain.Size = new System.Drawing.Size(659, 515);
            this.panelLeftMain.TabIndex = 0;
            this.panelLeftMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLeftMain_Paint);
            // 
            // panelBottomLeft
            // 
            this.panelBottomLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelBottomLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottomLeft.ColumnCount = 3;
            this.panelBottomLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelBottomLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.panelBottomLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelBottomLeft.Controls.Add(this.panelProgress, 1, 0);
            this.panelBottomLeft.Controls.Add(this.updateButt, 2, 0);
            this.panelBottomLeft.Controls.Add(this.checkUpdatesButt, 0, 0);
            this.panelBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomLeft.Location = new System.Drawing.Point(4, 465);
            this.panelBottomLeft.Name = "panelBottomLeft";
            this.panelBottomLeft.RowCount = 1;
            this.panelBottomLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelBottomLeft.Size = new System.Drawing.Size(651, 46);
            this.panelBottomLeft.TabIndex = 3;
            // 
            // panelProgress
            // 
            this.panelProgress.ColumnCount = 1;
            this.panelProgress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelProgress.Controls.Add(this.percentLabel, 0, 2);
            this.panelProgress.Controls.Add(this.speedLabel, 0, 1);
            this.panelProgress.Controls.Add(this.progressLabel, 0, 0);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProgress.Location = new System.Drawing.Point(198, 3);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.RowCount = 3;
            this.panelProgress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.panelProgress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.panelProgress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.panelProgress.Size = new System.Drawing.Size(254, 40);
            this.panelProgress.TabIndex = 1;
            // 
            // percentLabel
            // 
            this.percentLabel.AutoSize = true;
            this.percentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.percentLabel.Location = new System.Drawing.Point(3, 26);
            this.percentLabel.Name = "percentLabel";
            this.percentLabel.Size = new System.Drawing.Size(248, 14);
            this.percentLabel.TabIndex = 6;
            this.percentLabel.Text = "0% (0 MB downloaded, 0 MB remaining)";
            this.percentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // speedLabel
            // 
            this.speedLabel.AutoSize = true;
            this.speedLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.speedLabel.Location = new System.Drawing.Point(3, 13);
            this.speedLabel.Name = "speedLabel";
            this.speedLabel.Size = new System.Drawing.Size(248, 13);
            this.speedLabel.TabIndex = 5;
            this.speedLabel.Text = "0 KB/s,remaining 0 s ";
            this.speedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressLabel
            // 
            this.progressLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressLabel.Location = new System.Drawing.Point(3, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(248, 13);
            this.progressLabel.TabIndex = 3;
            this.progressLabel.Text = "Downloading: 0 / 0";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updateButt
            // 
            this.updateButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateButt.Font = new System.Drawing.Font("华文细黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.updateButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updateButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.updateButt.Location = new System.Drawing.Point(458, 3);
            this.updateButt.Name = "updateButt";
            this.updateButt.Size = new System.Drawing.Size(190, 40);
            this.updateButt.TabIndex = 1;
            this.updateButt.Text = "更    新";
            this.updateButt.UseVisualStyleBackColor = true;
            this.updateButt.Click += new System.EventHandler(this.updateButt_Click);
            // 
            // checkUpdatesButt
            // 
            this.checkUpdatesButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkUpdatesButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkUpdatesButt.Font = new System.Drawing.Font("华文细黑", 15F);
            this.checkUpdatesButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkUpdatesButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.checkUpdatesButt.Location = new System.Drawing.Point(3, 3);
            this.checkUpdatesButt.Name = "checkUpdatesButt";
            this.checkUpdatesButt.Size = new System.Drawing.Size(189, 40);
            this.checkUpdatesButt.TabIndex = 0;
            this.checkUpdatesButt.Text = "检查更新";
            this.checkUpdatesButt.UseVisualStyleBackColor = true;
            this.checkUpdatesButt.Click += new System.EventHandler(this.checkUpdatesButt_Click);
            // 
            // progressBar
            // 
            this.progressBar.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(7, 431);
            this.progressBar.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(645, 27);
            this.progressBar.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.newsPictureBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelOutput);
            this.splitContainer1.Size = new System.Drawing.Size(651, 420);
            this.splitContainer1.SplitterDistance = 306;
            this.splitContainer1.TabIndex = 12;
            this.splitContainer1.TabStop = false;
            // 
            // newsPictureBox
            // 
            this.newsPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.newsPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newsPictureBox.InitialImage = null;
            this.newsPictureBox.Location = new System.Drawing.Point(0, 0);
            this.newsPictureBox.Name = "newsPictureBox";
            this.newsPictureBox.Size = new System.Drawing.Size(647, 302);
            this.newsPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.newsPictureBox.TabIndex = 0;
            this.newsPictureBox.TabStop = false;
            this.newsPictureBox.Click += new System.EventHandler(this.newsPictureBox_Click);
            // 
            // panelOutput
            // 
            this.panelOutput.BackColor = System.Drawing.Color.Transparent;
            this.panelOutput.Controls.Add(this.output);
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutput.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panelOutput.Location = new System.Drawing.Point(0, 0);
            this.panelOutput.Name = "panelOutput";
            this.panelOutput.Size = new System.Drawing.Size(647, 106);
            this.panelOutput.TabIndex = 14;
            this.panelOutput.TabStop = false;
            this.panelOutput.Text = "温馨提示:";
            // 
            // output
            // 
            this.output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.output.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.output.Location = new System.Drawing.Point(3, 17);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(641, 86);
            this.output.TabIndex = 10;
            this.output.Text = "";
            this.output.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.output_LinkClicked);
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panelRight.ColumnCount = 1;
            this.panelRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.Controls.Add(this.button1, 0, 8);
            this.panelRight.Controls.Add(this.changelogEditButt, 0, 6);
            this.panelRight.Controls.Add(this.regButt, 0, 3);
            this.panelRight.Controls.Add(this.panelOptional, 0, 0);
            this.panelRight.Controls.Add(this.launchButt, 0, 8);
            this.panelRight.Controls.Add(this.changelogBrowserButt, 0, 7);
            this.panelRight.Controls.Add(this.panelTotalSize, 0, 1);
            this.panelRight.Controls.Add(this.webButt, 0, 2);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(0, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.RowCount = 9;
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.panelRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.panelRight.Size = new System.Drawing.Size(216, 515);
            this.panelRight.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("锐字真言体免费商用", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Image = global::AmarothLauncher.Properties.Resources._123;
            this.button1.Location = new System.Drawing.Point(4, 375);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 59);
            this.button1.TabIndex = 16;
            this.button1.Text = "退 出 游 戏";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // changelogEditButt
            // 
            this.changelogEditButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.changelogEditButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changelogEditButt.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.changelogEditButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.changelogEditButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.changelogEditButt.Location = new System.Drawing.Point(4, 295);
            this.changelogEditButt.Name = "changelogEditButt";
            this.changelogEditButt.Size = new System.Drawing.Size(208, 30);
            this.changelogEditButt.TabIndex = 8;
            this.changelogEditButt.Text = "编辑日志";
            this.changelogEditButt.UseVisualStyleBackColor = true;
            this.changelogEditButt.Click += new System.EventHandler(this.changelogEditButt_Click);
            // 
            // regButt
            // 
            this.regButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regButt.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.regButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.regButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.regButt.Location = new System.Drawing.Point(4, 241);
            this.regButt.Name = "regButt";
            this.regButt.Size = new System.Drawing.Size(208, 29);
            this.regButt.TabIndex = 5;
            this.regButt.Text = "注册账号";
            this.regButt.UseVisualStyleBackColor = true;
            this.regButt.Click += new System.EventHandler(this.regButt_Click);
            // 
            // panelOptional
            // 
            this.panelOptional.BackColor = System.Drawing.Color.Transparent;
            this.panelOptional.Controls.Add(this.optionalsListView);
            this.panelOptional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOptional.ForeColor = System.Drawing.Color.Black;
            this.panelOptional.Location = new System.Drawing.Point(4, 4);
            this.panelOptional.Name = "panelOptional";
            this.panelOptional.Size = new System.Drawing.Size(208, 131);
            this.panelOptional.TabIndex = 15;
            this.panelOptional.TabStop = false;
            this.panelOptional.Text = "可选文件:";
            // 
            // optionalsListView
            // 
            this.optionalsListView.BackgroundImage = global::AmarothLauncher.Properties.Resources.tbg205a111;
            this.optionalsListView.BackgroundImageTiled = true;
            this.optionalsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.optionalsListView.CheckBoxes = true;
            this.optionalsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.optionalsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionalsListView.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionalsListView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.optionalsListView.FullRowSelect = true;
            this.optionalsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.optionalsListView.HideSelection = false;
            listViewItem1.StateImageIndex = 0;
            listViewItem1.ToolTipText = "!!!";
            this.optionalsListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.optionalsListView.Location = new System.Drawing.Point(3, 17);
            this.optionalsListView.MultiSelect = false;
            this.optionalsListView.Name = "optionalsListView";
            this.optionalsListView.ShowItemToolTips = true;
            this.optionalsListView.Size = new System.Drawing.Size(202, 111);
            this.optionalsListView.TabIndex = 10;
            this.optionalsListView.UseCompatibleStateImageBehavior = false;
            this.optionalsListView.View = System.Windows.Forms.View.List;
            this.optionalsListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.optionalsListView_ItemChecked);
            this.optionalsListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.optionalsListView_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 1000;
            // 
            // launchButt
            // 
            this.launchButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.launchButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.launchButt.Font = new System.Drawing.Font("锐字真言体免费商用", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.launchButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.launchButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.launchButt.Location = new System.Drawing.Point(4, 441);
            this.launchButt.Name = "launchButt";
            this.launchButt.Size = new System.Drawing.Size(208, 70);
            this.launchButt.TabIndex = 3;
            this.launchButt.Text = "进 入 游 戏";
            this.launchButt.UseVisualStyleBackColor = true;
            this.launchButt.Click += new System.EventHandler(this.launchButt_Click);
            // 
            // changelogBrowserButt
            // 
            this.changelogBrowserButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.changelogBrowserButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.changelogBrowserButt.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.changelogBrowserButt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.changelogBrowserButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.changelogBrowserButt.Location = new System.Drawing.Point(4, 332);
            this.changelogBrowserButt.Name = "changelogBrowserButt";
            this.changelogBrowserButt.Size = new System.Drawing.Size(208, 36);
            this.changelogBrowserButt.TabIndex = 9;
            this.changelogBrowserButt.Text = "浏览日志";
            this.changelogBrowserButt.UseVisualStyleBackColor = true;
            this.changelogBrowserButt.Click += new System.EventHandler(this.changelogBrowserButt_Click);
            // 
            // panelTotalSize
            // 
            this.panelTotalSize.BackColor = System.Drawing.Color.Transparent;
            this.panelTotalSize.Controls.Add(this.totalSizeLabel);
            this.panelTotalSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTotalSize.ForeColor = System.Drawing.Color.Black;
            this.panelTotalSize.Location = new System.Drawing.Point(4, 142);
            this.panelTotalSize.Name = "panelTotalSize";
            this.panelTotalSize.Size = new System.Drawing.Size(208, 55);
            this.panelTotalSize.TabIndex = 13;
            this.panelTotalSize.TabStop = false;
            this.panelTotalSize.Text = "需更新文件大小:";
            // 
            // totalSizeLabel
            // 
            this.totalSizeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalSizeLabel.ForeColor = System.Drawing.Color.Black;
            this.totalSizeLabel.Location = new System.Drawing.Point(3, 17);
            this.totalSizeLabel.Name = "totalSizeLabel";
            this.totalSizeLabel.Size = new System.Drawing.Size(202, 35);
            this.totalSizeLabel.TabIndex = 10;
            this.totalSizeLabel.Text = "已选择: \r\n未选择: ";
            this.totalSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // webButt
            // 
            this.webButt.BackColor = System.Drawing.Color.Transparent;
            this.webButt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webButt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.webButt.Font = new System.Drawing.Font("幼圆", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.webButt.ForeColor = System.Drawing.Color.Transparent;
            this.webButt.Image = global::AmarothLauncher.Properties.Resources._123;
            this.webButt.Location = new System.Drawing.Point(4, 204);
            this.webButt.Name = "webButt";
            this.webButt.Size = new System.Drawing.Size(208, 30);
            this.webButt.TabIndex = 4;
            this.webButt.Text = "游戏主页";
            this.webButt.UseVisualStyleBackColor = false;
            this.webButt.Click += new System.EventHandler(this.webButt_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(884, 516);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 418);
            this.Name = "MainWindow";
            this.Text = "MoonSun登录器";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.DragLeave += new System.EventHandler(this.MainWindow_DragLeave);
            this.panelMain.Panel1.ResumeLayout(false);
            this.panelMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelMain)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelLeftMain.ResumeLayout(false);
            this.panelBottomLeft.ResumeLayout(false);
            this.panelProgress.ResumeLayout(false);
            this.panelProgress.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.newsPictureBox)).EndInit();
            this.panelOutput.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelOptional.ResumeLayout(false);
            this.panelTotalSize.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer panelMain;
        private System.Windows.Forms.TableLayoutPanel panelLeftMain;
        private System.Windows.Forms.GroupBox panelOutput;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TableLayoutPanel panelBottomLeft;
        private System.Windows.Forms.TableLayoutPanel panelProgress;
        private System.Windows.Forms.Label percentLabel;
        private System.Windows.Forms.Label speedLabel;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button updateButt;
        private System.Windows.Forms.Button checkUpdatesButt;
        private System.Windows.Forms.TableLayoutPanel panelRight;
        private System.Windows.Forms.Button changelogBrowserButt;
        private System.Windows.Forms.GroupBox panelOptional;
        private System.Windows.Forms.Button webButt;
        private System.Windows.Forms.Button launchButt;
        private System.Windows.Forms.Button changelogEditButt;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListView optionalsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label totalSizeLabel;
        private System.Windows.Forms.GroupBox panelTotalSize;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox newsPictureBox;
        private System.Windows.Forms.Button regButt;
        private System.Windows.Forms.Button button1;
    }
}

