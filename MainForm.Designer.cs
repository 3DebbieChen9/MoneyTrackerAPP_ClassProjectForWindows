namespace MoneyTrackerAPP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.main_panel = new System.Windows.Forms.Panel();
            this.main_tabControl = new System.Windows.Forms.TabControl();
            this.main_transaction = new System.Windows.Forms.TabPage();
            this.main_accounts = new System.Windows.Forms.TabPage();
            this.main_report = new System.Windows.Forms.TabPage();
            this.main_settings = new System.Windows.Forms.TabPage();
            this.main_iconImages = new System.Windows.Forms.ImageList(this.components);
            this.main_border = new System.Windows.Forms.Panel();
            this.main_closeApp = new System.Windows.Forms.Button();
            this.main_appName = new System.Windows.Forms.Label();
            this.main_applogo = new System.Windows.Forms.Label();
            this.main_panel.SuspendLayout();
            this.main_tabControl.SuspendLayout();
            this.main_border.SuspendLayout();
            this.SuspendLayout();
            // 
            // main_panel
            // 
            this.main_panel.BackColor = System.Drawing.Color.PaleTurquoise;
            this.main_panel.Controls.Add(this.main_tabControl);
            this.main_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.main_panel.Location = new System.Drawing.Point(0, 40);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(1024, 560);
            this.main_panel.TabIndex = 0;
            // 
            // main_tabControl
            // 
            this.main_tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.main_tabControl.Controls.Add(this.main_transaction);
            this.main_tabControl.Controls.Add(this.main_accounts);
            this.main_tabControl.Controls.Add(this.main_report);
            this.main_tabControl.Controls.Add(this.main_settings);
            this.main_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_tabControl.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.main_tabControl.ImageList = this.main_iconImages;
            this.main_tabControl.ImeMode = System.Windows.Forms.ImeMode.On;
            this.main_tabControl.ItemSize = new System.Drawing.Size(100, 100);
            this.main_tabControl.Location = new System.Drawing.Point(0, 0);
            this.main_tabControl.Multiline = true;
            this.main_tabControl.Name = "main_tabControl";
            this.main_tabControl.Padding = new System.Drawing.Point(0, 0);
            this.main_tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.main_tabControl.SelectedIndex = 0;
            this.main_tabControl.Size = new System.Drawing.Size(1024, 560);
            this.main_tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.main_tabControl.TabIndex = 0;
            // 
            // main_transaction
            // 
            this.main_transaction.AutoScroll = true;
            this.main_transaction.BackColor = System.Drawing.Color.PaleTurquoise;
            this.main_transaction.ImageIndex = 0;
            this.main_transaction.Location = new System.Drawing.Point(104, 4);
            this.main_transaction.Name = "main_transaction";
            this.main_transaction.Padding = new System.Windows.Forms.Padding(3);
            this.main_transaction.Size = new System.Drawing.Size(916, 552);
            this.main_transaction.TabIndex = 0;
            this.main_transaction.ToolTipText = "Add transaction";
            // 
            // main_accounts
            // 
            this.main_accounts.BackColor = System.Drawing.Color.Turquoise;
            this.main_accounts.ImageIndex = 1;
            this.main_accounts.Location = new System.Drawing.Point(104, 4);
            this.main_accounts.Name = "main_accounts";
            this.main_accounts.Padding = new System.Windows.Forms.Padding(3);
            this.main_accounts.Size = new System.Drawing.Size(916, 552);
            this.main_accounts.TabIndex = 1;
            this.main_accounts.ToolTipText = "Check Account";
            // 
            // main_report
            // 
            this.main_report.BackColor = System.Drawing.Color.MediumTurquoise;
            this.main_report.ImageIndex = 2;
            this.main_report.Location = new System.Drawing.Point(104, 4);
            this.main_report.Name = "main_report";
            this.main_report.Padding = new System.Windows.Forms.Padding(3);
            this.main_report.Size = new System.Drawing.Size(916, 552);
            this.main_report.TabIndex = 2;
            this.main_report.ToolTipText = "Report";
            // 
            // main_settings
            // 
            this.main_settings.BackColor = System.Drawing.Color.Aquamarine;
            this.main_settings.ImageIndex = 3;
            this.main_settings.Location = new System.Drawing.Point(104, 4);
            this.main_settings.Name = "main_settings";
            this.main_settings.Padding = new System.Windows.Forms.Padding(3);
            this.main_settings.Size = new System.Drawing.Size(916, 552);
            this.main_settings.TabIndex = 3;
            this.main_settings.ToolTipText = "Setting";
            // 
            // main_iconImages
            // 
            this.main_iconImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("main_iconImages.ImageStream")));
            this.main_iconImages.TransparentColor = System.Drawing.Color.Transparent;
            this.main_iconImages.Images.SetKeyName(0, "applogo.png");
            this.main_iconImages.Images.SetKeyName(1, "account.png");
            this.main_iconImages.Images.SetKeyName(2, "report.png");
            this.main_iconImages.Images.SetKeyName(3, "settings.png");
            // 
            // main_border
            // 
            this.main_border.BackColor = System.Drawing.Color.LightSeaGreen;
            this.main_border.Controls.Add(this.main_closeApp);
            this.main_border.Controls.Add(this.main_appName);
            this.main_border.Controls.Add(this.main_applogo);
            this.main_border.Location = new System.Drawing.Point(0, 0);
            this.main_border.Margin = new System.Windows.Forms.Padding(0);
            this.main_border.Name = "main_border";
            this.main_border.Size = new System.Drawing.Size(1024, 40);
            this.main_border.TabIndex = 0;
            // 
            // main_closeApp
            // 
            this.main_closeApp.BackColor = System.Drawing.Color.LightSeaGreen;
            this.main_closeApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.main_closeApp.FlatAppearance.BorderSize = 0;
            this.main_closeApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.main_closeApp.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.main_closeApp.Location = new System.Drawing.Point(983, 0);
            this.main_closeApp.Margin = new System.Windows.Forms.Padding(0);
            this.main_closeApp.Name = "main_closeApp";
            this.main_closeApp.Size = new System.Drawing.Size(40, 40);
            this.main_closeApp.TabIndex = 3;
            this.main_closeApp.Text = "X";
            this.main_closeApp.UseVisualStyleBackColor = false;
            this.main_closeApp.Click += new System.EventHandler(this.main_closeApp_Click);
            // 
            // main_appName
            // 
            this.main_appName.Font = new System.Drawing.Font("Consolas", 13.5F, System.Drawing.FontStyle.Bold);
            this.main_appName.Location = new System.Drawing.Point(41, 5);
            this.main_appName.Name = "main_appName";
            this.main_appName.Size = new System.Drawing.Size(219, 30);
            this.main_appName.TabIndex = 2;
            this.main_appName.Text = "Money Tracker";
            this.main_appName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // main_applogo
            // 
            this.main_applogo.Location = new System.Drawing.Point(5, 5);
            this.main_applogo.Name = "main_applogo";
            this.main_applogo.Size = new System.Drawing.Size(30, 30);
            this.main_applogo.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.main_panel);
            this.Controls.Add(this.main_border);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Money Tracker";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.main_panel.ResumeLayout(false);
            this.main_tabControl.ResumeLayout(false);
            this.main_border.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel main_panel;
        private System.Windows.Forms.Panel main_border;
        private System.Windows.Forms.Label main_applogo;
        private System.Windows.Forms.Label main_appName;
        private System.Windows.Forms.Button main_closeApp;
        private System.Windows.Forms.TabControl main_tabControl;
        private System.Windows.Forms.TabPage main_transaction;
        private System.Windows.Forms.TabPage main_accounts;
        private System.Windows.Forms.ImageList main_iconImages;
        private System.Windows.Forms.TabPage main_report;
        private System.Windows.Forms.TabPage main_settings;
    }
}

