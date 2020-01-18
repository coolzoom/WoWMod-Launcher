namespace AmarothLauncher.GUI
{
    partial class FTPLoginWindow
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
        /// 设计器支持所需的方法-不要修改此方法与代码编辑器的内容
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FTPLoginWindow));
            this.panelLogin = new System.Windows.Forms.GroupBox();
            this.loginBox = new System.Windows.Forms.TextBox();
            this.panelPassword = new System.Windows.Forms.GroupBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.okButt = new System.Windows.Forms.Button();
            this.cancelButt = new System.Windows.Forms.Button();
            this.panelLogin.SuspendLayout();
            this.panelPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.loginBox);
            this.panelLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogin.Location = new System.Drawing.Point(0, 0);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(284, 38);
            this.panelLogin.TabIndex = 0;
            this.panelLogin.TabStop = false;
            this.panelLogin.Text = "FTP用户名:";
            // 
            // loginBox
            // 
            this.loginBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.loginBox.Location = new System.Drawing.Point(3, 17);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(278, 21);
            this.loginBox.TabIndex = 0;
            // 
            // panelPassword
            // 
            this.panelPassword.Controls.Add(this.passwordBox);
            this.panelPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPassword.Location = new System.Drawing.Point(0, 38);
            this.panelPassword.Name = "panelPassword";
            this.panelPassword.Size = new System.Drawing.Size(284, 38);
            this.panelPassword.TabIndex = 1;
            this.panelPassword.TabStop = false;
            this.panelPassword.Text = "FTP密码:";
            this.panelPassword.Enter += new System.EventHandler(this.panelPassword_Enter);
            // 
            // passwordBox
            // 
            this.passwordBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.passwordBox.Location = new System.Drawing.Point(3, 17);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(278, 21);
            this.passwordBox.TabIndex = 0;
            this.passwordBox.UseSystemPasswordChar = true;
            // 
            // okButt
            // 
            this.okButt.Location = new System.Drawing.Point(3, 81);
            this.okButt.Name = "okButt";
            this.okButt.Size = new System.Drawing.Size(138, 28);
            this.okButt.TabIndex = 2;
            this.okButt.Text = "确定";
            this.okButt.UseVisualStyleBackColor = true;
            this.okButt.Click += new System.EventHandler(this.okButt_Click);
            // 
            // cancelButt
            // 
            this.cancelButt.Location = new System.Drawing.Point(143, 81);
            this.cancelButt.Name = "cancelButt";
            this.cancelButt.Size = new System.Drawing.Size(138, 28);
            this.cancelButt.TabIndex = 3;
            this.cancelButt.Text = "取消";
            this.cancelButt.UseVisualStyleBackColor = true;
            this.cancelButt.Click += new System.EventHandler(this.cancelButt_Click);
            // 
            // FTPLoginWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 113);
            this.Controls.Add(this.cancelButt);
            this.Controls.Add(this.okButt);
            this.Controls.Add(this.panelPassword);
            this.Controls.Add(this.panelLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FTPLoginWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FTP登录";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FTPLoginWindow_FormClosing);
            this.panelLogin.ResumeLayout(false);
            this.panelLogin.PerformLayout();
            this.panelPassword.ResumeLayout(false);
            this.panelPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox panelLogin;
        private System.Windows.Forms.TextBox loginBox;
        private System.Windows.Forms.GroupBox panelPassword;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button okButt;
        private System.Windows.Forms.Button cancelButt;
    }
}