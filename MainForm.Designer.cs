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
            this.trans_btn_save = new System.Windows.Forms.Button();
            this.trans_remainder = new System.Windows.Forms.Label();
            this.trans_txtbox_note = new System.Windows.Forms.TextBox();
            this.trans_note = new System.Windows.Forms.Label();
            this.trans_txtbox_tag = new System.Windows.Forms.TextBox();
            this.trans_label_tag = new System.Windows.Forms.Label();
            this.trans_txtbox_store = new System.Windows.Forms.TextBox();
            this.trans_label_store = new System.Windows.Forms.Label();
            this.trans_date_picker = new System.Windows.Forms.DateTimePicker();
            this.trans_label_date = new System.Windows.Forms.Label();
            this.trans_cbo_account = new System.Windows.Forms.ComboBox();
            this.trans_label_account = new System.Windows.Forms.Label();
            this.trans_cbo_category = new System.Windows.Forms.ComboBox();
            this.trans_label_category = new System.Windows.Forms.Label();
            this.trans_txtbox_amount = new System.Windows.Forms.TextBox();
            this.trans_label_amount = new System.Windows.Forms.Label();
            this.trans_txtbox_name = new System.Windows.Forms.TextBox();
            this.trans_label_name = new System.Windows.Forms.Label();
            this.trans_rdb_income = new System.Windows.Forms.RadioButton();
            this.trans_rdb_expanse = new System.Windows.Forms.RadioButton();
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
            this.main_transaction.SuspendLayout();
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
            this.main_tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
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
            this.main_tabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.main_tabControl_DrawItem);
            this.main_tabControl.SelectedIndexChanged += new System.EventHandler(this.main_tabControl_SelectedIndexChanged);
            // 
            // main_transaction
            // 
            this.main_transaction.AutoScroll = true;
            this.main_transaction.BackColor = System.Drawing.Color.PaleTurquoise;
            this.main_transaction.Controls.Add(this.trans_btn_save);
            this.main_transaction.Controls.Add(this.trans_remainder);
            this.main_transaction.Controls.Add(this.trans_txtbox_note);
            this.main_transaction.Controls.Add(this.trans_note);
            this.main_transaction.Controls.Add(this.trans_txtbox_tag);
            this.main_transaction.Controls.Add(this.trans_label_tag);
            this.main_transaction.Controls.Add(this.trans_txtbox_store);
            this.main_transaction.Controls.Add(this.trans_label_store);
            this.main_transaction.Controls.Add(this.trans_date_picker);
            this.main_transaction.Controls.Add(this.trans_label_date);
            this.main_transaction.Controls.Add(this.trans_cbo_account);
            this.main_transaction.Controls.Add(this.trans_label_account);
            this.main_transaction.Controls.Add(this.trans_cbo_category);
            this.main_transaction.Controls.Add(this.trans_label_category);
            this.main_transaction.Controls.Add(this.trans_txtbox_amount);
            this.main_transaction.Controls.Add(this.trans_label_amount);
            this.main_transaction.Controls.Add(this.trans_txtbox_name);
            this.main_transaction.Controls.Add(this.trans_label_name);
            this.main_transaction.Controls.Add(this.trans_rdb_income);
            this.main_transaction.Controls.Add(this.trans_rdb_expanse);
            this.main_transaction.ImageIndex = 0;
            this.main_transaction.Location = new System.Drawing.Point(104, 4);
            this.main_transaction.Name = "main_transaction";
            this.main_transaction.Padding = new System.Windows.Forms.Padding(3);
            this.main_transaction.Size = new System.Drawing.Size(916, 552);
            this.main_transaction.TabIndex = 0;
            this.main_transaction.ToolTipText = "Add transaction";
            // 
            // trans_btn_save
            // 
            this.trans_btn_save.Location = new System.Drawing.Point(652, 479);
            this.trans_btn_save.Name = "trans_btn_save";
            this.trans_btn_save.Size = new System.Drawing.Size(195, 41);
            this.trans_btn_save.TabIndex = 19;
            this.trans_btn_save.Text = "儲存";
            this.trans_btn_save.UseVisualStyleBackColor = true;
            // 
            // trans_remainder
            // 
            this.trans_remainder.AutoSize = true;
            this.trans_remainder.Location = new System.Drawing.Point(409, 492);
            this.trans_remainder.Name = "trans_remainder";
            this.trans_remainder.Size = new System.Drawing.Size(194, 28);
            this.trans_remainder.TabIndex = 18;
            this.trans_remainder.Text = "剩餘預算：0 元";
            // 
            // trans_txtbox_note
            // 
            this.trans_txtbox_note.Location = new System.Drawing.Point(207, 358);
            this.trans_txtbox_note.Name = "trans_txtbox_note";
            this.trans_txtbox_note.Size = new System.Drawing.Size(640, 36);
            this.trans_txtbox_note.TabIndex = 17;
            // 
            // trans_note
            // 
            this.trans_note.AutoSize = true;
            this.trans_note.Location = new System.Drawing.Point(65, 361);
            this.trans_note.Name = "trans_note";
            this.trans_note.Size = new System.Drawing.Size(116, 28);
            this.trans_note.TabIndex = 16;
            this.trans_note.Text = "備　　註";
            // 
            // trans_txtbox_tag
            // 
            this.trans_txtbox_tag.Location = new System.Drawing.Point(207, 298);
            this.trans_txtbox_tag.Name = "trans_txtbox_tag";
            this.trans_txtbox_tag.Size = new System.Drawing.Size(241, 36);
            this.trans_txtbox_tag.TabIndex = 15;
            // 
            // trans_label_tag
            // 
            this.trans_label_tag.AutoSize = true;
            this.trans_label_tag.Location = new System.Drawing.Point(65, 301);
            this.trans_label_tag.Name = "trans_label_tag";
            this.trans_label_tag.Size = new System.Drawing.Size(116, 28);
            this.trans_label_tag.TabIndex = 14;
            this.trans_label_tag.Text = "標　　籤";
            // 
            // trans_txtbox_store
            // 
            this.trans_txtbox_store.Location = new System.Drawing.Point(606, 234);
            this.trans_txtbox_store.Name = "trans_txtbox_store";
            this.trans_txtbox_store.Size = new System.Drawing.Size(241, 36);
            this.trans_txtbox_store.TabIndex = 13;
            // 
            // trans_label_store
            // 
            this.trans_label_store.AutoSize = true;
            this.trans_label_store.Location = new System.Drawing.Point(498, 240);
            this.trans_label_store.Name = "trans_label_store";
            this.trans_label_store.Size = new System.Drawing.Size(90, 28);
            this.trans_label_store.TabIndex = 12;
            this.trans_label_store.Text = "商　家";
            // 
            // trans_date_picker
            // 
            this.trans_date_picker.Location = new System.Drawing.Point(207, 234);
            this.trans_date_picker.Name = "trans_date_picker";
            this.trans_date_picker.Size = new System.Drawing.Size(241, 36);
            this.trans_date_picker.TabIndex = 11;
            // 
            // trans_label_date
            // 
            this.trans_label_date.AutoSize = true;
            this.trans_label_date.Location = new System.Drawing.Point(65, 240);
            this.trans_label_date.Name = "trans_label_date";
            this.trans_label_date.Size = new System.Drawing.Size(116, 28);
            this.trans_label_date.TabIndex = 10;
            this.trans_label_date.Text = "記錄時間";
            // 
            // trans_cbo_account
            // 
            this.trans_cbo_account.FormattingEnabled = true;
            this.trans_cbo_account.Location = new System.Drawing.Point(606, 174);
            this.trans_cbo_account.Name = "trans_cbo_account";
            this.trans_cbo_account.Size = new System.Drawing.Size(241, 36);
            this.trans_cbo_account.TabIndex = 9;
            // 
            // trans_label_account
            // 
            this.trans_label_account.AutoSize = true;
            this.trans_label_account.Location = new System.Drawing.Point(498, 177);
            this.trans_label_account.Name = "trans_label_account";
            this.trans_label_account.Size = new System.Drawing.Size(90, 28);
            this.trans_label_account.TabIndex = 8;
            this.trans_label_account.Text = "帳　戶";
            // 
            // trans_cbo_category
            // 
            this.trans_cbo_category.FormattingEnabled = true;
            this.trans_cbo_category.Location = new System.Drawing.Point(207, 174);
            this.trans_cbo_category.Name = "trans_cbo_category";
            this.trans_cbo_category.Size = new System.Drawing.Size(241, 36);
            this.trans_cbo_category.TabIndex = 7;
            // 
            // trans_label_category
            // 
            this.trans_label_category.AutoSize = true;
            this.trans_label_category.Location = new System.Drawing.Point(65, 177);
            this.trans_label_category.Name = "trans_label_category";
            this.trans_label_category.Size = new System.Drawing.Size(116, 28);
            this.trans_label_category.TabIndex = 6;
            this.trans_label_category.Text = "款項分類";
            // 
            // trans_txtbox_amount
            // 
            this.trans_txtbox_amount.Location = new System.Drawing.Point(606, 116);
            this.trans_txtbox_amount.Name = "trans_txtbox_amount";
            this.trans_txtbox_amount.Size = new System.Drawing.Size(241, 36);
            this.trans_txtbox_amount.TabIndex = 5;
            // 
            // trans_label_amount
            // 
            this.trans_label_amount.AutoSize = true;
            this.trans_label_amount.Location = new System.Drawing.Point(498, 119);
            this.trans_label_amount.Name = "trans_label_amount";
            this.trans_label_amount.Size = new System.Drawing.Size(90, 28);
            this.trans_label_amount.TabIndex = 4;
            this.trans_label_amount.Text = "金　額";
            // 
            // trans_txtbox_name
            // 
            this.trans_txtbox_name.Location = new System.Drawing.Point(207, 116);
            this.trans_txtbox_name.Name = "trans_txtbox_name";
            this.trans_txtbox_name.Size = new System.Drawing.Size(241, 36);
            this.trans_txtbox_name.TabIndex = 3;
            // 
            // trans_label_name
            // 
            this.trans_label_name.AutoSize = true;
            this.trans_label_name.Location = new System.Drawing.Point(65, 119);
            this.trans_label_name.Name = "trans_label_name";
            this.trans_label_name.Size = new System.Drawing.Size(116, 28);
            this.trans_label_name.TabIndex = 2;
            this.trans_label_name.Text = "款項名稱";
            // 
            // trans_rdb_income
            // 
            this.trans_rdb_income.AutoSize = true;
            this.trans_rdb_income.Location = new System.Drawing.Point(227, 46);
            this.trans_rdb_income.Name = "trans_rdb_income";
            this.trans_rdb_income.Size = new System.Drawing.Size(89, 32);
            this.trans_rdb_income.TabIndex = 1;
            this.trans_rdb_income.TabStop = true;
            this.trans_rdb_income.Text = "收入";
            this.trans_rdb_income.UseVisualStyleBackColor = true;
            // 
            // trans_rdb_expanse
            // 
            this.trans_rdb_expanse.AutoSize = true;
            this.trans_rdb_expanse.Location = new System.Drawing.Point(70, 46);
            this.trans_rdb_expanse.Name = "trans_rdb_expanse";
            this.trans_rdb_expanse.Size = new System.Drawing.Size(89, 32);
            this.trans_rdb_expanse.TabIndex = 0;
            this.trans_rdb_expanse.TabStop = true;
            this.trans_rdb_expanse.Text = "支出";
            this.trans_rdb_expanse.UseVisualStyleBackColor = true;
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
            this.main_transaction.ResumeLayout(false);
            this.main_transaction.PerformLayout();
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
        private System.Windows.Forms.Label trans_label_name;
        private System.Windows.Forms.RadioButton trans_rdb_income;
        private System.Windows.Forms.RadioButton trans_rdb_expanse;
        private System.Windows.Forms.TextBox trans_txtbox_name;
        private System.Windows.Forms.Label trans_label_amount;
        private System.Windows.Forms.TextBox trans_txtbox_amount;
        private System.Windows.Forms.Label trans_label_date;
        private System.Windows.Forms.ComboBox trans_cbo_account;
        private System.Windows.Forms.Label trans_label_account;
        private System.Windows.Forms.ComboBox trans_cbo_category;
        private System.Windows.Forms.Label trans_label_category;
        private System.Windows.Forms.Button trans_btn_save;
        private System.Windows.Forms.Label trans_remainder;
        private System.Windows.Forms.TextBox trans_txtbox_note;
        private System.Windows.Forms.Label trans_note;
        private System.Windows.Forms.TextBox trans_txtbox_tag;
        private System.Windows.Forms.Label trans_label_tag;
        private System.Windows.Forms.TextBox trans_txtbox_store;
        private System.Windows.Forms.Label trans_label_store;
        private System.Windows.Forms.DateTimePicker trans_date_picker;
    }
}

