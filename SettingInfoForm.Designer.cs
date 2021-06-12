
namespace MoneyTrackerAPP
{
    partial class SettingInfoForm
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
            this.settingInfo_border = new System.Windows.Forms.Panel();
            this.settingInfo_closeApp = new System.Windows.Forms.Button();
            this.main_closeApp = new System.Windows.Forms.Button();
            this.settingInfo_name = new System.Windows.Forms.Label();
            this.main_applogo = new System.Windows.Forms.Label();
            this.settingInfo_text = new System.Windows.Forms.TextBox();
            this.settingInfo_OK = new System.Windows.Forms.Button();
            this.settingInfo_Delete = new System.Windows.Forms.Button();
            this.settingInfo_Modify = new System.Windows.Forms.Button();
            this.settingInfo_border.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingInfo_border
            // 
            this.settingInfo_border.BackColor = System.Drawing.Color.LightSeaGreen;
            this.settingInfo_border.Controls.Add(this.settingInfo_closeApp);
            this.settingInfo_border.Controls.Add(this.main_closeApp);
            this.settingInfo_border.Controls.Add(this.settingInfo_name);
            this.settingInfo_border.Controls.Add(this.main_applogo);
            this.settingInfo_border.Location = new System.Drawing.Point(0, 0);
            this.settingInfo_border.Margin = new System.Windows.Forms.Padding(0);
            this.settingInfo_border.Name = "settingInfo_border";
            this.settingInfo_border.Size = new System.Drawing.Size(250, 40);
            this.settingInfo_border.TabIndex = 1;
            // 
            // settingInfo_closeApp
            // 
            this.settingInfo_closeApp.BackColor = System.Drawing.Color.LightSeaGreen;
            this.settingInfo_closeApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.settingInfo_closeApp.FlatAppearance.BorderSize = 0;
            this.settingInfo_closeApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingInfo_closeApp.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingInfo_closeApp.Location = new System.Drawing.Point(210, 0);
            this.settingInfo_closeApp.Margin = new System.Windows.Forms.Padding(0);
            this.settingInfo_closeApp.Name = "settingInfo_closeApp";
            this.settingInfo_closeApp.Size = new System.Drawing.Size(40, 40);
            this.settingInfo_closeApp.TabIndex = 4;
            this.settingInfo_closeApp.Text = "X";
            this.settingInfo_closeApp.UseVisualStyleBackColor = false;
            this.settingInfo_closeApp.Click += new System.EventHandler(this.settingInfo_closeApp_Click);
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
            // 
            // settingInfo_name
            // 
            this.settingInfo_name.Font = new System.Drawing.Font("Consolas", 13.5F, System.Drawing.FontStyle.Bold);
            this.settingInfo_name.Location = new System.Drawing.Point(12, 5);
            this.settingInfo_name.Name = "settingInfo_name";
            this.settingInfo_name.Size = new System.Drawing.Size(58, 30);
            this.settingInfo_name.TabIndex = 2;
            this.settingInfo_name.Text = "Info";
            this.settingInfo_name.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // main_applogo
            // 
            this.main_applogo.Location = new System.Drawing.Point(5, 5);
            this.main_applogo.Name = "main_applogo";
            this.main_applogo.Size = new System.Drawing.Size(30, 30);
            this.main_applogo.TabIndex = 1;
            // 
            // settingInfo_text
            // 
            this.settingInfo_text.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.settingInfo_text.Location = new System.Drawing.Point(75, 100);
            this.settingInfo_text.Name = "settingInfo_text";
            this.settingInfo_text.Size = new System.Drawing.Size(100, 22);
            this.settingInfo_text.TabIndex = 2;
            // 
            // settingInfo_OK
            // 
            this.settingInfo_OK.Location = new System.Drawing.Point(88, 150);
            this.settingInfo_OK.Name = "settingInfo_OK";
            this.settingInfo_OK.Size = new System.Drawing.Size(75, 23);
            this.settingInfo_OK.TabIndex = 3;
            this.settingInfo_OK.Text = "新增";
            this.settingInfo_OK.UseVisualStyleBackColor = true;
            this.settingInfo_OK.Click += new System.EventHandler(this.settingInfo_OK_Click);
            // 
            // settingInfo_Delete
            // 
            this.settingInfo_Delete.Location = new System.Drawing.Point(135, 150);
            this.settingInfo_Delete.Name = "settingInfo_Delete";
            this.settingInfo_Delete.Size = new System.Drawing.Size(75, 23);
            this.settingInfo_Delete.TabIndex = 4;
            this.settingInfo_Delete.Text = "刪除";
            this.settingInfo_Delete.UseVisualStyleBackColor = true;
            this.settingInfo_Delete.Click += new System.EventHandler(this.settingInfo_Delete_Click);
            // 
            // settingInfo_Modify
            // 
            this.settingInfo_Modify.Location = new System.Drawing.Point(35, 150);
            this.settingInfo_Modify.Name = "settingInfo_Modify";
            this.settingInfo_Modify.Size = new System.Drawing.Size(75, 23);
            this.settingInfo_Modify.TabIndex = 5;
            this.settingInfo_Modify.Text = "修改";
            this.settingInfo_Modify.UseVisualStyleBackColor = true;
            this.settingInfo_Modify.Click += new System.EventHandler(this.settingInfo_Modify_Click);
            // 
            // SettingInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(250, 200);
            this.Controls.Add(this.settingInfo_Modify);
            this.Controls.Add(this.settingInfo_Delete);
            this.Controls.Add(this.settingInfo_OK);
            this.Controls.Add(this.settingInfo_text);
            this.Controls.Add(this.settingInfo_border);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SettingInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info";
            this.settingInfo_border.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel settingInfo_border;
        private System.Windows.Forms.Button main_closeApp;
        private System.Windows.Forms.Label settingInfo_name;
        private System.Windows.Forms.Label main_applogo;
        private System.Windows.Forms.Button settingInfo_closeApp;
        private System.Windows.Forms.TextBox settingInfo_text;
        private System.Windows.Forms.Button settingInfo_OK;
        private System.Windows.Forms.Button settingInfo_Delete;
        private System.Windows.Forms.Button settingInfo_Modify;
    }
}