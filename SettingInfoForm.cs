using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyTrackerAPP
{
    public partial class SettingInfoForm : Form
    {
        private Control _mainForm = new Control();
        private string origin_text = null;
        private string type;
        public SettingInfoForm(string listType, Control ctrl)
        {
            InitializeComponent();
            _mainForm = ctrl;
            settingInfo_name.Text = "新增";
            type = listType;

            settingInfo_Modify.Hide();
            settingInfo_Delete.Hide();
        }
        public SettingInfoForm(string listType, Control ctrl, string text)
        {
            InitializeComponent();
            _mainForm = ctrl;
            settingInfo_name.Text = "編輯";
            type = listType;

            settingInfo_text.Text = text;
            origin_text = text;
            settingInfo_OK.Hide();
        }
        private void settingInfo_closeApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void settingInfo_OK_Click(object sender, EventArgs e)
        {
            ((MainForm)_mainForm).setting_list_add(type, settingInfo_text.Text);
            ((MainForm)_mainForm).setting_update_List2ListBox(type);
            this.Close();
        }
        private void settingInfo_Modify_Click(object sender, EventArgs e)
        {
            ((MainForm)_mainForm).setting_list_modify(type, origin_text, settingInfo_text.Text);
            ((MainForm)_mainForm).setting_update_List2ListBox(type);
            this.Close();
        }

        private void settingInfo_Delete_Click(object sender, EventArgs e)
        {
            string msgText = "\"" + origin_text + "\"";
            string msgCaption = null;
            switch(type)
            {
                case "Income":
                    msgCaption = "收入種類";
                    break;
                case "Expense":
                    msgCaption = "支出種類";
                    break;
                case "Place":
                    msgCaption = "商店列表";
                    break;
                case "Recipient":
                    msgCaption = "對象列表";
                    break;
            }
            if(MessageBox.Show("確定要刪除 " + msgText + " 嗎?", msgCaption, MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                ((MainForm)_mainForm).setting_list_remove(type, origin_text);
                ((MainForm)_mainForm).setting_update_List2ListBox(type);
                this.Close();
            }
        }
    }
}
