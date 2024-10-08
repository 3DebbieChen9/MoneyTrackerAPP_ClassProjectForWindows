﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;

namespace MoneyTrackerAPP
{
    public partial class MainForm : Form
    {
        static string dbName = "moneyRecord.db";
        static Database db = new Database(dbName);
        static AccountDB accountDB = new AccountDB(dbName);
        static ReportDB reportDB = new ReportDB(dbName);
        static SettingDB settingDB = new SettingDB(dbName);
        static SettingVar settingVar = new SettingVar();
        AccountGlobal account = new AccountGlobal();
        Report report = new Report();
        TransactionGlobal transactionGlobal = new TransactionGlobal();

        #region MainForm
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void main_border_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void main_appName_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void main_applogo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void main_minimizeApp_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void main_closeApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void main_tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            switch (e.Index)
            {
                case 0:
                    e.Graphics.FillRectangle(new SolidBrush(Color.PowderBlue), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(new SolidBrush(Color.LemonChiffon), e.Bounds);
                    break;
                case 2:
                    e.Graphics.FillRectangle(new SolidBrush(Color.PaleTurquoise), e.Bounds);
                    break;
                case 3:
                    e.Graphics.FillRectangle(new SolidBrush(Color.NavajoWhite), e.Bounds);
                    break;
                case 4:
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightCyan), e.Bounds);
                    break;
                default:
                    break;
            }

            Rectangle paddedBounds = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, 100, 100);
            paddedBounds.Inflate(-15, -15);
            e.Graphics.DrawImage(main_iconImages.Images[e.Index], paddedBounds);
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            main_applogo.Image = new Bitmap("icons/applogo_30x30.png");
            #region Account
            accounts_panel_account.Enabled = true;
            accounts_panel_creditcard.Enabled = false;
            accounts_panel_detail.Enabled = false;
            accounts_create.Enabled = true;
            accounts_delete.Enabled = true;
            accounts_ok.Enabled = false;
            accounts_btn_payDebt.Enabled = false;
            string[] account_type = accountDB.get_account_type();
            foreach (string ele in account_type)
            {
                accounts_type.Items.Add(ele);
            }
            #endregion
            #region Report
            report_piechart1.Titles.Add("支出圓餅圖");
            report_piechart2.Titles.Add("收入圓餅圖");
            report_barchart.Titles.Add("收支長條圖");
            report_linechart.Titles.Add("收支折線圖");
            #endregion
            #region Setting
            //  setting
            setting_tabControl.RightToLeft = RightToLeft.Yes;
            setting_tabControl.RightToLeftLayout = true;
            setting_tabControl.SelectedIndex = 3;
            setting_title.Text = "Category";
            //  setting_category
            settingVar.category_inc = settingDB.get_category("Income");
            settingVar.category_exp = settingDB.get_category("Expense");
            //  setting_list
            settingVar.list_place = settingDB.get_list_place();
            settingVar.list_recipient = settingDB.get_list_recipient();
            //  setting_notification
            settingVar.notification_infos = settingDB.get_notification();
            //  setting timer
            setting_notification_timer.Interval = 1000 * 30;
            setting_notification_timer.Enabled = true;
            setting_notification_timer.Start();
            //  update listbox
            SettingFunction.update_NotificationListBox(setting_notification_listbox, settingVar.notification_infos);
            setting_update_List2ListBox("TotalList");
            #endregion

            trans_expense_display();
        }
        private void main_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Trans
            if (main_tabControl.SelectedIndex == 0)
            {
                trans_expense_display();
                trans_rdb_expense.Checked = true;
                trans_rdb_income.Checked = false;
                tran_rdb_debtandloan.Checked = false;
                tran_rdb_transfer.Checked = false;
            }
            // Account
            else if (main_tabControl.SelectedIndex == 1)
            {
                accounts_type.Text = "";
                accounts_name.Text = "";
                accounts_total_balance.Text = "0";
                accounts_panel_detail.Controls.Clear();
                accounts_panel_creditcard.Controls.Clear();
                accounts_panel_debt.Controls.Clear();
                main_accounts.Controls.Add(accounts_panel_detail);
                main_accounts.Controls.Remove(accounts_panel_creditcard);
                main_accounts.Controls.Remove(accounts_panel_debt);

                AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
                myAdd.AddRange(accountDB.get_account_type().Where(val => val != "Debt/Loan").ToArray());
                accounts_txtType.AutoCompleteCustomSource = myAdd;
                accounts_txtType.AutoCompleteMode = AutoCompleteMode.Suggest;
                accounts_txtType.AutoCompleteSource = AutoCompleteSource.CustomSource;
                
            }
            // Report
            else if (main_tabControl.SelectedIndex == 2)
            {
                report_day_rb.Checked = true;
                report_dateTimePicker1.Value = DateTime.Now;
                string[] day_exp_cate;
                int[] day_exp_amount;
                (day_exp_cate, day_exp_amount) = reportDB.get_day_exp_cate(DateTime.Now);
                report_piechart1.Series.Clear();
                report.pie_exp_series.ChartType = SeriesChartType.Pie;
                report.pie_exp_series.IsValueShownAsLabel = true;
                report.pie_exp_series.LegendText = "#AXISLABEL";
                report.pie_exp_series.Label = "#PERCENT{P0}";
                report.pie_exp_series.Points.DataBindXY(day_exp_cate, day_exp_amount);
                report_piechart1.Series.Add(report.pie_exp_series);
                string[] day_inc_cate;
                int[] day_inc_amount;
                (day_inc_cate, day_inc_amount) = reportDB.get_day_inc_cate(DateTime.Now);
                report_piechart2.Series.Clear();
                report.pie_inc_series.ChartType = SeriesChartType.Pie;
                report.pie_inc_series.IsValueShownAsLabel = true;
                report.pie_inc_series.LegendText = "#AXISLABEL";
                report.pie_inc_series.Label = "#PERCENT{P0}";
                report.pie_inc_series.Points.DataBindXY(day_inc_cate, day_inc_amount);
                report_piechart2.Series.Add(report.pie_inc_series);
                report_barchart.Series.Clear();
                for (int index = 0; index < 2; index++)
                {
                    report.bar_series[index] = new Series();
                    report.bar_series[index].Color = report.bar_colors[index];
                    report.bar_series[index].ChartType = SeriesChartType.Bar;
                    report.bar_series[index].Name = report.bar_item[index];
                    report.bar_series[index].IsValueShownAsLabel = true;
                }
                (report.total_expense, report.total_income) = reportDB.get_day_total_amount(DateTime.Now);
                report.bar_series[0].Points.Clear();
                report.bar_series[1].Points.Clear();
                report.bar_series[0].Points.Add(-report.total_expense);
                report.bar_series[1].Points.Add(report.total_income);
                report_barchart.Series.Add(report.bar_series[0]);
                report_barchart.Series.Add(report.bar_series[1]);
                report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                List<Transaction> exp_data;
                List<Transaction> inc_data;
                exp_data = reportDB.get_day_exp_data(DateTime.Now);
                inc_data = reportDB.get_day_inc_data(DateTime.Now);
                report_show_lb.Text = "Daily Report" + Environment.NewLine;
                report_show_lb.Text += report_dateTimePicker1.Value.Year + "/" + report_dateTimePicker1.Value.Month + "/" + report_dateTimePicker1.Value.Day + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Expense:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "      " + "Account" + Environment.NewLine;
                report_show_lb.Text += "-----------------------------------------------------------------------------------" + Environment.NewLine;
                foreach (Transaction ele in exp_data)
                {
                    report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Expense:      " + report.total_expense + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "===================================================================================" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Income:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "      " + "Account" + Environment.NewLine;
                report_show_lb.Text += "-----------------------------------------------------------------------------------" + Environment.NewLine;
                foreach (Transaction ele in inc_data)
                {
                    report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Income:       " + report.total_income + Environment.NewLine;
            }
            // Setting
            else if (main_tabControl.SelectedIndex == 3)
            {
                settingVar.list_place = settingDB.get_list_place();
                settingVar.list_recipient = settingDB.get_list_recipient();
                SettingFunction.update_NotificationListBox(setting_notification_listbox, settingVar.notification_infos);
                setting_notification_info_modify.Enabled = false;
                setting_notification_info_delete.Enabled = false;
                setting_update_List2ListBox("TotalList");
            }
            // List
            else if (main_tabControl.SelectedIndex == 4)
            {
                List<Detail> details = db.queryDetail();
                detail_label.Text = "";
                detail_label.Text += "Name" + "                " + "Type" + "      " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "       " + "Account" + "     " + "Date" + "        " + "Place" + "     " + "Comment" + Environment.NewLine;
                detail_label.Text += "--------------------" + "----------" + "--------------------" + "-------------------" + "-------------" + "------------" + "----------" + "----------" + "--------------------" + "----------" + Environment.NewLine;
                foreach (Detail ele in details)
                {
                    detail_label.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-10}", ele.type) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) +
                        String.Format("{0,-12}", ele.account) + String.Format("{0,-12}", ele.date.ToString("yyyy-MM-dd")) + String.Format("{0,-10}", ele.place) + String.Format("{0,-20}", ele.comment) + Environment.NewLine;
                }
                detail_label.Text += " " + Environment.NewLine;
            }
        }
        #region Account
        private void accounts_type_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            accounts_name.Items.Clear();
            accounts_name.Text = "";
            accounts_panel_detail.Controls.Clear();
            accounts_panel_creditcard.Controls.Clear();
            accounts_panel_debt.Controls.Clear();

            if (accounts_type.Text == "Debt/Loan")
            {
                accounts_panel_account.Controls.Remove(accounts_delete);

                string[] recipients_name = accountDB.get_list_recipient().ToArray();
                foreach (string ele in recipients_name)
                {
                    accounts_name.Items.Add(ele);
                }
                main_accounts.Controls.Remove(accounts_panel_creditcard);
                main_accounts.Controls.Remove(accounts_panel_detail);
                main_accounts.Controls.Add(accounts_panel_debt);
                accounts_panel_debt.Location = new Point(36, 159);
                accounts_panel_debt.Size = new Size(829, 376);
                refreshDebtLoanInfo();
            }
            else
            {
                accounts_panel_debt.Controls.Remove(accounts_btn_payDebt);
                accounts_panel_account.Controls.Add(accounts_delete);
                string[] account_name = accountDB.get_account_name(accounts_type.Text);
                foreach (string ele in account_name)
                {
                    accounts_name.Items.Add(ele);
                }
                if (accounts_type.Text == "Credit Card")
                {
                    main_accounts.Controls.Remove(accounts_panel_debt);
                    main_accounts.Controls.Add(accounts_panel_detail);
                    refreshCreditCardInfo();
                }
                else
                {
                    main_accounts.Controls.Remove(accounts_panel_creditcard);
                    main_accounts.Controls.Remove(accounts_panel_debt);
                    main_accounts.Controls.Add(accounts_panel_detail);
                    accounts_panel_detail.Location = new Point(36, 159);
                    accounts_panel_detail.Size = new Size(829, 376);
                }
            }
        }

        private void accounts_create_Click_1(object sender, EventArgs e)
        {
            if(accounts_txtType.Text == "")
            {
                MessageBox.Show("請輸入帳戶種類");
            }
            else if(accounts_txtName.Text == "")
            {
                MessageBox.Show("請輸入帳戶名稱");
            }
            else
            {
                try
                {
                    int test = int.Parse(accounts_txtBalance.Text);
                    accountDB.insert_account_name(accounts_txtType.Text, accounts_txtName.Text, accounts_txtBalance.Text);
                    string[] account_type = accountDB.get_account_type();
                    accounts_type.Items.Clear();
                    foreach (string ele in account_type)
                    {
                        accounts_type.Items.Add(ele);
                    }
                    string[] account_name = accountDB.get_account_name(accounts_type.Text);
                    accounts_name.Items.Clear();
                    foreach (string ele in account_name)
                    {
                        accounts_name.Items.Add(ele);
                    }
                    accounts_txtType.Text = "";
                    accounts_txtName.Text = "";
                    accounts_txtBalance.Text = "";

                    accounts_type.Text = "";
                    accounts_name.Text = "";
                    accounts_total_balance.Text = "0";
                    accounts_panel_detail.Controls.Clear();
                    accounts_panel_creditcard.Controls.Clear();
                    accounts_panel_debt.Controls.Clear();
                    main_accounts.Controls.Add(accounts_panel_detail);
                    main_accounts.Controls.Remove(accounts_panel_creditcard);
                    main_accounts.Controls.Remove(accounts_panel_debt);
                    AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
                    myAdd.AddRange(accountDB.get_account_type().Where(val => val != "Debt/Loan").ToArray());
                    accounts_txtType.AutoCompleteCustomSource = myAdd;
                    accounts_txtType.AutoCompleteMode = AutoCompleteMode.Suggest;
                    accounts_txtType.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
                catch
                {
                    MessageBox.Show("請輸入正確格式的帳戶餘額");
                }
            }
            
        }

        private void accounts_delete_Click(object sender, EventArgs e)
        {
            if(accounts_type.Text == "Debt/Loan")
            {
                MessageBox.Show("不能刪除此帳戶");
            }
            else
            {
                if (accounts_type.Text == "")
                {
                    MessageBox.Show("請選擇帳戶種類");
                }
                else if (accounts_name.Text == "")
                {
                    MessageBox.Show("請選擇帳戶名稱");
                }
                else
                {
                    accountDB.delete_account_name(accounts_type.Text, accounts_name.Text);
                    string[] account_type = accountDB.get_account_type();
                    accounts_type.Items.Clear();
                    foreach (string ele in account_type)
                    {
                        accounts_type.Items.Add(ele);
                    }
                    string[] account_name = accountDB.get_account_name(accounts_type.Text);
                    accounts_name.Items.Clear();
                    foreach (string ele in account_name)
                    {
                        accounts_name.Items.Add(ele);
                    }
                    accounts_type.Text = "";
                    accounts_name.Text = "";
                    accounts_total_balance.Text = "0";
                    accounts_panel_detail.Controls.Clear();
                    accounts_panel_creditcard.Controls.Clear();
                    accounts_panel_debt.Controls.Clear();
                    main_accounts.Controls.Add(accounts_panel_detail);
                    main_accounts.Controls.Remove(accounts_panel_creditcard);
                    main_accounts.Controls.Remove(accounts_panel_debt);
                    AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
                    myAdd.AddRange(accountDB.get_account_type().Where(val => val != "Debt/Loan").ToArray());
                    accounts_txtType.AutoCompleteCustomSource = myAdd;
                    accounts_txtType.AutoCompleteMode = AutoCompleteMode.Suggest;
                    accounts_txtType.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
        }
        void refreshDebtLoanInfo()
        {
            accounts_panel_debt.Controls.Clear();
            accounts_panel_debt.Controls.Add(accounts_btn_payDebt);
            accounts_btn_payDebt.Enabled = true;
            accounts_total_balance.Text = accountDB.get_debtLoan_balance(who: accounts_name.Text);
            DebtLoan[] debt_info = accountDB.get_debtLoans_detail(who: accounts_name.Text);
            accounts_panel_debt.Location = new Point(36, 159);
            accounts_panel_debt.Size = new Size(829, 376);
            Label accounts_label1 = new Label();
            accounts_label1.Location = new Point(10, 10);
            accounts_label1.Text = "品項";
            accounts_panel_debt.Controls.Add(accounts_label1);
            Label accounts_label2 = new Label();
            accounts_label2.Location = new Point(150, 10);
            accounts_label2.Text = "Debt/Loan";
            accounts_panel_debt.Controls.Add(accounts_label2);
            Label accounts_label3 = new Label();
            accounts_label3.Location = new Point(300, 10);
            accounts_label3.Text = "金額";
            accounts_panel_debt.Controls.Add(accounts_label3);
            Label accounts_label4 = new Label();
            accounts_label4.Location = new Point(450, 10);
            accounts_label4.Text = "日期";
            accounts_panel_debt.Controls.Add(accounts_label4);
            Label accounts_label5 = new Label();
            accounts_label5.Location = new Point(600, 10);
            accounts_label5.Text = "帳戶";
            accounts_panel_debt.Controls.Add(accounts_label5);
            Label accounts_label6 = new Label();
            accounts_label6.Location = new Point(750, 10);
            accounts_label6.Text = "已結清";
            accounts_panel_debt.Controls.Add(accounts_label6);

            account.y = 50;

            for (int i = 0; i < debt_info.Length; i++)
            {
                Label accounts_detail = new Label();
                Label accounts_type = new Label();
                Label accounts_amount = new Label();
                Label accounts_date = new Label();
                accounts_detail.Location = new Point(10, account.y);
                accounts_type.Location = new Point(150, account.y);
                accounts_amount.Location = new Point(300, account.y);
                accounts_date.Location = new Point(450, account.y);
                //DateTimePicker accounts_paid_date = new DateTimePicker();
                ComboBox accounts_account = new ComboBox();
                string[] allAccounts = accountDB.get_all_account();
                foreach (string account in allAccounts)
                {
                    if (account != "Debt/Loan")
                    {
                        accounts_account.Items.Add(account);
                    }
                }
                accounts_account.Location = new Point(600, account.y);

                CheckBox accounts_paid = new CheckBox();
                accounts_paid.Location = new Point(750, account.y);
                try { accounts_paid.Name = "p" + debt_info[i].id.ToString(); }
                catch { accounts_paid.Name = "p0"; }
                try { accounts_account.Name = "a" + debt_info[i].id.ToString(); }
                catch { accounts_account.Name = "a0"; }


                account.debt_checkbox.Add(accounts_paid);
                account.debt_account.Add(accounts_account);

                accounts_detail.Text = debt_info[i].detail;
                accounts_type.Text = debt_info[i].type;
                accounts_amount.Text = debt_info[i].amount.ToString();
                accounts_date.Text = debt_info[i].date.ToString("yyyy/MM/dd");
                accounts_account.Text = debt_info[i].account;

                accounts_panel_debt.Controls.Add(accounts_detail);
                accounts_panel_debt.Controls.Add(accounts_type);
                accounts_panel_debt.Controls.Add(accounts_amount);
                accounts_panel_debt.Controls.Add(accounts_date);
                accounts_panel_debt.Controls.Add(accounts_account);
                accounts_panel_debt.Controls.Add(accounts_paid);
                accounts_panel_debt.Show();
                account.y += 50;
            }
        }
        void refreshCreditCardInfo()
        {
            accounts_total_balance.Text = accountDB.queryAccountBalance(accountType: accounts_type.Text, accountName: accounts_name.Text);
            CreditCardInfo[] accounts_info = accountDB.get_unpaid_transaction(accounts_name.Text);
            if (accounts_type.Text == "Credit Card")
            {
                main_accounts.Controls.Add(accounts_panel_creditcard);
                accounts_panel_creditcard.Controls.Clear();
                accounts_panel_creditcard.Enabled = true;
                accounts_panel_detail.Location = new Point(36, 354);
                accounts_panel_detail.Size = new Size(829, 190);
                Label accounts_label1 = new Label();
                accounts_label1.Location = new Point(10, 10);
                accounts_label1.Text = "未付款";
                accounts_panel_creditcard.Controls.Add(accounts_label1);
                Label accounts_label2 = new Label();
                accounts_label2.Location = new Point(account.x1, 50);
                accounts_label2.Text = "品項";
                accounts_panel_creditcard.Controls.Add(accounts_label2);
                Label accounts_label3 = new Label();
                accounts_label3.Location = new Point(account.x2, 50);
                accounts_label3.Text = "金額";
                accounts_panel_creditcard.Controls.Add(accounts_label3);
                Label accounts_label4 = new Label();
                accounts_label4.Location = new Point(account.x3, 50);
                accounts_label4.Text = "付款日期";
                accounts_panel_creditcard.Controls.Add(accounts_label4);
                Label accounts_label5 = new Label();
                accounts_label5.Location = new Point(account.x4, 50);
                accounts_label5.Text = "已付款";
                accounts_panel_creditcard.Controls.Add(accounts_label5);
                accounts_panel_creditcard.Controls.Add(accounts_ok);

                account.y = 100;
                //List<CreditCardInfo> accounts_check = new List<CreditCardInfo>();
                for (int i = 0; i < accounts_info.Length; i++)
                {
                    Label accounts_item = new Label();
                    Label accounts_amount = new Label();
                    accounts_item.Location = new Point(account.x1, account.y);
                    accounts_amount.Location = new Point(account.x2, account.y);
                    DateTimePicker accounts_paid_date = new DateTimePicker();
                    accounts_paid_date.Location = new Point(account.x3, account.y);
                    accounts_panel_creditcard.Controls.Add(accounts_paid_date);
                    CheckBox accounts_paid = new CheckBox();
                    try { accounts_paid.Name = "b" + accounts_info[i].id.ToString(); }
                    catch { accounts_paid.Name = "b0"; }
                    try { accounts_paid_date.Name = "d" + accounts_info[i].id.ToString(); }
                    catch { accounts_paid_date.Name = "d0"; }
                    accounts_paid.Location = new Point(account.x4, account.y);
                    accounts_panel_creditcard.Controls.Add(accounts_paid);
                    account.accounts_checkbox.Add(accounts_paid);
                    account.accounts_bankDate.Add(accounts_paid_date);
                    accounts_item.Text = accounts_info[i].name;
                    accounts_amount.Text = accounts_info[i].amount;
                    accounts_panel_creditcard.Controls.Add(accounts_item);
                    accounts_panel_creditcard.Controls.Add(accounts_amount);
                    accounts_panel_creditcard.Show();
                    account.y += 50;

                }
            }

            accounts_panel_detail.Controls.Clear();
            Label accounts_label6 = new Label();
            accounts_label6.Location = new Point(10, 10);
            accounts_label6.Text = "交易紀錄";
            accounts_panel_detail.Controls.Add(accounts_label6);
            Label accounts_label7 = new Label();
            accounts_label7.Location = new Point(account.x1, 50);
            accounts_label7.Text = "品項";
            accounts_panel_detail.Controls.Add(accounts_label7);
            Label accounts_label8 = new Label();
            accounts_label8.Location = new Point(account.x2, 50);
            accounts_label8.Text = "金額";
            accounts_panel_detail.Controls.Add(accounts_label8);
            Label accounts_label9 = new Label();
            accounts_label9.Location = new Point(account.x3, 50);
            accounts_label9.Text = "交易日期";
            accounts_panel_detail.Controls.Add(accounts_label9);
            accounts_panel_detail.Show();

            AccountDetail[] accounts_detail = accountDB.get_account_detail(accounts_name.Text);
            account.y = 100;
            for (int i = 0; i < accounts_detail.Length; i++)
            {
                Label accounts_item = new Label();
                Label accounts_amount = new Label();
                Label accounts_date = new Label();
                accounts_item.Location = new Point(account.x1, account.y);
                accounts_amount.Location = new Point(account.x2, account.y);
                accounts_date.Location = new Point(account.x3, account.y);
                accounts_item.Text = accounts_detail[i].name;
                accounts_amount.Text = accounts_detail[i].amount;
                accounts_date.Text = accounts_detail[i].date.ToString();
                accounts_panel_detail.Controls.Add(accounts_item);
                accounts_panel_detail.Controls.Add(accounts_amount);
                accounts_panel_detail.Controls.Add(accounts_date);
                accounts_panel_detail.Show();
                account.y += 50;
            }
        }
        private void accounts_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            accounts_ok.Enabled = true;
            accounts_panel_detail.Enabled = true;
            if (accounts_type.Text != "Debt/Loan")
            {
                refreshCreditCardInfo();
                accounts_total_balance.Text = accountDB.queryAccountBalance(accountType: accounts_type.Text, accountName: accounts_name.Text);
            }
            else
            {
                accounts_total_balance.Text = accountDB.get_debtLoan_balance(accounts_name.Text);
                refreshDebtLoanInfo();
            }

        }

        private void accounts_ok_Click(object sender, EventArgs e)
        {
            foreach (CheckBox ele in account.accounts_checkbox)
            {
                if (ele.Checked)
                {
                    foreach (DateTimePicker element in account.accounts_bankDate)
                    {
                        if (element.Name.Substring(1) == ele.Name.Substring(1))
                        {
                            DateTime date = element.Value;
                            accountDB.updateBankChecked(int.Parse(ele.Name.Substring(1)), date);
                        }
                    }
                }
            }
            refreshCreditCardInfo();
        }

        private void accounts_btn_payDebt_Click(object sender, EventArgs e)
        {
            foreach (CheckBox ele in account.debt_checkbox)
            {
                if (ele.Checked)
                {
                    foreach (ComboBox element in account.debt_account)
                    {
                        if (element.Name.Substring(1) == ele.Name.Substring(1))
                        {
                            string payAccount = element.Text;
                            accountDB.pay_DebtLoan(int.Parse(ele.Name.Substring(1)), payAccount);
                        }
                    }
                }
            }
            refreshDebtLoanInfo();
        }
        #endregion


        #region Report
        private void report_day_rb_CheckedChanged(object sender, EventArgs e)
        {
            string[] day_exp_cate;
            int[] day_exp_amount;
            string[] day_inc_cate;
            int[] day_inc_amount;
            report_month_gb.Enabled = false;
            report_du_gb.Enabled = false;
            report_day_gb.Enabled = true;
            report_linechart.Visible = false;
            report_piechart1.Visible = true;
            report_piechart2.Visible = true;
            report_dateTimePicker1.Value = DateTime.Now;
            (day_exp_cate, day_exp_amount) = reportDB.get_day_exp_cate(DateTime.Now);
            report.pie_exp_series.Points.DataBindXY(day_exp_cate, day_exp_amount);
            (day_inc_cate, day_inc_amount) = reportDB.get_day_inc_cate(DateTime.Now);
            report.pie_inc_series.Points.DataBindXY(day_inc_cate, day_inc_amount);
            (report.total_expense, report.total_income) = reportDB.get_day_total_amount(DateTime.Now);
            report.bar_series[0].Points.Clear();
            report.bar_series[1].Points.Clear();
            report.bar_series[0].Points.Add(-report.total_expense);
            report.bar_series[1].Points.Add(report.total_income);
            report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
            List<Transaction> exp_data;
            List<Transaction> inc_data;
            exp_data = reportDB.get_day_exp_data(DateTime.Now);
            inc_data = reportDB.get_day_inc_data(DateTime.Now);
            report_show_lb.Text = "Daily Report" + Environment.NewLine;
            report_show_lb.Text += report_dateTimePicker1.Value.Year + "/" + report_dateTimePicker1.Value.Month + "/" + report_dateTimePicker1.Value.Day + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Expense:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "      " + "Account" + Environment.NewLine;
            report_show_lb.Text += "---------------------------------------------------------------------------------------" + Environment.NewLine;
            foreach (Transaction ele in exp_data)
            {
                report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;

            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Expense:      " + report.total_expense + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "========================================================================================" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Income:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "      " + "Account" + Environment.NewLine;
            report_show_lb.Text += "---------------------------------------------------------------------------------------" + Environment.NewLine;
            foreach (Transaction ele in inc_data)
            {
                report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;
            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Income:       " + report.total_income + Environment.NewLine;
        }

        private void report_month_rb_CheckedChanged(object sender, EventArgs e)
        {
            report_day_gb.Enabled = false;
            report_du_gb.Enabled = false;
            report_month_gb.Enabled = true;
            report_linechart.Visible = false;
            report_piechart1.Visible = true;
            report_piechart2.Visible = true;
            report_year_cb.Text = DateTime.Now.Year.ToString();
            report_month_cb.Text = DateTime.Now.Month.ToString();
            string year;
            string month;
            string[] exp_cate;
            int[] exp_amount;
            string[] inc_cate;
            int[] inc_amount;
            year = report_year_cb.Text;
            month = report_month_cb.Text;
            (exp_cate, exp_amount) = reportDB.get_month_exp_cate(year, month);
            report.pie_exp_series.Points.DataBindXY(exp_cate, exp_amount);
            (inc_cate, inc_amount) = reportDB.get_month_inc_cate(year, month);
            report.pie_inc_series.Points.DataBindXY(inc_cate, inc_amount);
            (report.total_expense, report.total_income) = reportDB.get_month_total_amount(year, month);
            report.bar_series[0].Points.Clear();
            report.bar_series[1].Points.Clear();
            report.bar_series[0].Points.Add(-report.total_expense);
            report.bar_series[1].Points.Add(report.total_income);
            report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
            report_show_lb.Text = "Monthly Report" + Environment.NewLine;
            report_show_lb.Text += year + "/" + month + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Expense:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            for (int i = 0; i < exp_cate.Length; i++)
            {
                report_show_lb.Text += String.Format("{0,-21}", exp_cate[i]) + String.Format("{0,-7}", exp_amount[i]) + Environment.NewLine;
            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Expense:       " + report.total_expense + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "=======================================================================================" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Income:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            for (int i = 0; i < inc_cate.Length; i++)
            {
                report_show_lb.Text += String.Format("{0,-21}", inc_cate[i]) + String.Format("{0,-7}", inc_amount[i]) + Environment.NewLine;
            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Income:        " + report.total_income + Environment.NewLine;

        }

        private void report_du_rb_CheckedChanged(object sender, EventArgs e)
        {
            report_day_gb.Enabled = false;
            report_month_gb.Enabled = false;
            report_du_gb.Enabled = true;
            report_linechart.Visible = true;
            report_piechart1.Visible = false;
            report_piechart2.Visible = false;
            report_year_from_cb.Text = "2021";
            report_month_from_cb.Text = "1";
            report_year_to_cb.Text = "2021";
            report_month_to_cb.Text = "6";
            string year_from;
            string year_to;
            string month_from;
            string month_to;
            int[] monthly_expense;
            int[] monthly_income;
            year_from = report_year_from_cb.Text;
            year_to = report_year_to_cb.Text;
            month_from = report_month_from_cb.Text;
            month_to = report_month_to_cb.Text;
            (monthly_expense, monthly_income) = reportDB.get_duration_data(year_from, month_from, year_to, month_to);
            report.start_date = Convert.ToDateTime(month_from + "/" + year_from);
            report.end_date = Convert.ToDateTime(month_to + "/" + year_to);
            report.du_datetime_list.Clear();
            report_linechart.Series.Clear();
            while (report.start_date != report.end_date)
            {
                report.du_datetime_list.Add(report.start_date);
                report.start_date = report.start_date.AddMonths(1);
            }
            report.du_datetime_list.Add(report.start_date);
            report.line_series1.Color = Color.Salmon;
            report.line_series2.Color = Color.MediumSlateBlue;
            report.line_series1.ChartType = SeriesChartType.Line;
            report.line_series2.ChartType = SeriesChartType.Line;
            report.line_series1.Points.Clear();
            report.line_series2.Points.Clear();
            for (int index = 0; index < monthly_expense.Length; index++)
            {
                report.line_series1.Points.AddXY(report.du_datetime_list[index].Month.ToString(), -monthly_expense[index]);
                report.line_series2.Points.AddXY(report.du_datetime_list[index].Month.ToString(), monthly_income[index]);
            }
            report_linechart.Series.Add(report.line_series1);
            report_linechart.Series.Add(report.line_series2);
            report_linechart.Series[0].BorderWidth = 2;
            report_linechart.Series[1].BorderWidth = 2;
            (report.total_expense, report.total_income) = reportDB.get_du_total_amount(year_from, month_from, year_to, month_to);
            report.bar_series[0].Points.Clear();
            report.bar_series[1].Points.Clear();
            report.bar_series[0].Points.Add(-report.total_expense);
            report.bar_series[1].Points.Add(report.total_income);
            report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
            report_show_lb.Text = "Duration Report" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            if (monthly_expense.Length % 2 == 0)
            {
                for (int i = 0; i < monthly_expense.Length; i++)
                {
                    report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                    report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                    report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                    i = i + 1;
                    report_show_lb.Text += Environment.NewLine;
                }
            }
            else
            {
                for (int i = 0; i < monthly_expense.Length - 1; i++)
                {
                    report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                    report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                    report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                    i = i + 1;
                    report_show_lb.Text += Environment.NewLine;
                }
                report_show_lb.Text += report.du_datetime_list[monthly_expense.Length - 1].Year.ToString() + "/" + report.du_datetime_list[monthly_expense.Length - 1].Month.ToString() + Environment.NewLine;
                report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[monthly_expense.Length - 1]) + Environment.NewLine;
                report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[monthly_expense.Length - 1]) + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
            }
            report_show_lb.Text += "------------------------------------------------------------" + Environment.NewLine;
            report_show_lb.Text += "Total Expense:    " + report.total_expense + Environment.NewLine;
            report_show_lb.Text += "Total Income:     " + report.total_income + Environment.NewLine;
        }

        private void report_dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime date;
            string[] day_exp_cate;
            int[] day_exp_amount;
            string[] day_inc_cate;
            int[] day_inc_amount;
            date = report_dateTimePicker1.Value;
            (day_exp_cate, day_exp_amount) = reportDB.get_day_exp_cate(date);
            report_piechart1.Series.Clear();
            report.pie_exp_series.ChartType = SeriesChartType.Pie;
            report.pie_exp_series.IsValueShownAsLabel = true;
            report.pie_exp_series.LegendText = "#AXISLABEL";
            report.pie_exp_series.Label = "#PERCENT{P0}";
            report.pie_exp_series.Points.DataBindXY(day_exp_cate, day_exp_amount);
            report_piechart1.Series.Add(report.pie_exp_series);
            (day_inc_cate, day_inc_amount) = reportDB.get_day_inc_cate(date);
            report_piechart2.Series.Clear();
            report.pie_inc_series.ChartType = SeriesChartType.Pie;
            report.pie_inc_series.IsValueShownAsLabel = true;
            report.pie_inc_series.LegendText = "#AXISLABEL";
            report.pie_inc_series.Label = "#PERCENT{P0}";
            report.pie_inc_series.Points.DataBindXY(day_inc_cate, day_inc_amount);
            report_piechart2.Series.Add(report.pie_inc_series);
            (report.total_expense, report.total_income) = reportDB.get_day_total_amount(date);
            report_barchart.Series.Clear();
            for (int index = 0; index < 2; index++)
            {
                report.bar_series[index] = new Series();
                report.bar_series[index].Color = report.bar_colors[index];
                report.bar_series[index].ChartType = SeriesChartType.Bar;
                report.bar_series[index].Name = report.bar_item[index];
                report.bar_series[index].IsValueShownAsLabel = true;
            }
            report.bar_series[0].Points.Clear();
            report.bar_series[1].Points.Clear();
            report.bar_series[0].Points.Add(-report.total_expense);
            report.bar_series[1].Points.Add(report.total_income);
            report_barchart.Series.Add(report.bar_series[0]);
            report_barchart.Series.Add(report.bar_series[1]);
            report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
            List<Transaction> exp_data;
            List<Transaction> inc_data;
            exp_data = reportDB.get_day_exp_data(date);
            inc_data = reportDB.get_day_inc_data(date);
            report_show_lb.Text = "Daily Report" + Environment.NewLine;
            report_show_lb.Text += report_dateTimePicker1.Value.Year + "/" + report_dateTimePicker1.Value.Month + "/" + report_dateTimePicker1.Value.Day + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Expense:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "       " + "Account" + Environment.NewLine;
            report_show_lb.Text += "-------------------------------------------------------------------------------------" + Environment.NewLine;
            foreach (Transaction ele in exp_data)
            {
                report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;
            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Expense:      " + report.total_expense + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "======================================================================================" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Income:" + Environment.NewLine;
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Name" + "                " + "Category" + "            " + "Subcategory" + "        " + "Amount" + "       " + "Account" + Environment.NewLine;
            report_show_lb.Text += "-------------------------------------------------------------------------------------" + Environment.NewLine;
            foreach (Transaction ele in inc_data)
            {
                report_show_lb.Text += String.Format("{0,-20}", ele.name) + String.Format("{0,-20}", ele.category) + String.Format("{0,-19}", ele.subcategory) + String.Format("{0,-13}", ele.amount) + String.Format("{0,-8}", ele.account) + Environment.NewLine;
            }
            report_show_lb.Text += Environment.NewLine;
            report_show_lb.Text += "Total Income:       " + report.total_income + Environment.NewLine;
        }

        private void report_year_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (report_year_cb.Text != "" && report_month_cb.Text != "")
            {
                string year;
                string month;
                string[] exp_cate;
                int[] exp_amount;
                string[] inc_cate;
                int[] inc_amount;
                year = report_year_cb.Text;
                month = report_month_cb.Text;
                (exp_cate, exp_amount) = reportDB.get_month_exp_cate(year, month);
                report.pie_exp_series.Points.DataBindXY(exp_cate, exp_amount);
                (inc_cate, inc_amount) = reportDB.get_month_inc_cate(year, month);
                report.pie_inc_series.Points.DataBindXY(inc_cate, inc_amount);
                (report.total_expense, report.total_income) = reportDB.get_month_total_amount(year, month);
                report.bar_series[0].Points.Clear();
                report.bar_series[1].Points.Clear();
                report.bar_series[0].Points.Add(-report.total_expense);
                report.bar_series[1].Points.Add(report.total_income);
                report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                report_show_lb.Text = "Monthly Report" + Environment.NewLine;
                report_show_lb.Text += year + "/" + month + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Expense:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                for (int i = 0; i < exp_cate.Length; i++)
                {
                    report_show_lb.Text += String.Format("{0,-21}", exp_cate[i]) + String.Format("{0,-7}", exp_amount[i]) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Expense:       " + report.total_expense + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "===================================================================================" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Income:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                for (int i = 0; i < inc_cate.Length; i++)
                {
                    report_show_lb.Text += String.Format("{0,-21}", inc_cate[i]) + String.Format("{0,-7}", inc_amount[i]) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Income:        " + report.total_income + Environment.NewLine;
            }
        }

        private void report_month_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (report_year_cb.Text != "" && report_month_cb.Text != "")
            {
                string year;
                string month;
                string[] exp_cate;
                int[] exp_amount;
                string[] inc_cate;
                int[] inc_amount;
                year = report_year_cb.Text;
                month = report_month_cb.Text;
                (exp_cate, exp_amount) = reportDB.get_month_exp_cate(year, month);
                report.pie_exp_series.Points.DataBindXY(exp_cate, exp_amount);
                (inc_cate, inc_amount) = reportDB.get_month_inc_cate(year, month);
                report.pie_inc_series.Points.DataBindXY(inc_cate, inc_amount);
                (report.total_expense, report.total_income) = reportDB.get_month_total_amount(year, month);
                report.bar_series[0].Points.Clear();
                report.bar_series[1].Points.Clear();
                report.bar_series[0].Points.Add(-report.total_expense);
                report.bar_series[1].Points.Add(report.total_income);
                report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                report_show_lb.Text = "Monthly Report" + Environment.NewLine;
                report_show_lb.Text += year + "/" + month + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Expense:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                for (int i = 0; i < exp_cate.Length; i++)
                {
                    report_show_lb.Text += String.Format("{0,-21}", exp_cate[i]) + String.Format("{0,-7}", exp_amount[i]) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Expense:       " + report.total_expense + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "===================================================================================" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Income:" + Environment.NewLine;
                report_show_lb.Text += Environment.NewLine;
                for (int i = 0; i < inc_cate.Length; i++)
                {
                    report_show_lb.Text += String.Format("{0,-21}", inc_cate[i]) + String.Format("{0,-7}", inc_amount[i]) + Environment.NewLine;
                }
                report_show_lb.Text += Environment.NewLine;
                report_show_lb.Text += "Total Income:        " + report.total_income + Environment.NewLine;
            }
        }

        private void report_year_from_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (report_year_from_cb.Text != "" && report_year_to_cb.Text != "" && report_month_from_cb.Text != "" && report_month_to_cb.Text != "")
                {
                    string year_from;
                    string year_to;
                    string month_from;
                    string month_to;
                    int[] monthly_expense;
                    int[] monthly_income;
                    year_from = report_year_from_cb.Text;
                    year_to = report_year_to_cb.Text;
                    month_from = report_month_from_cb.Text;
                    month_to = report_month_to_cb.Text;
                    (monthly_expense, monthly_income) = reportDB.get_duration_data(year_from, month_from, year_to, month_to);
                    report.start_date = Convert.ToDateTime(month_from + "/" + year_from);
                    report.end_date = Convert.ToDateTime(month_to + "/" + year_to);
                    report.du_datetime_list.Clear();
                    report_linechart.Series.Clear();
                    while (report.start_date != report.end_date)
                    {
                        report.du_datetime_list.Add(report.start_date);
                        report.start_date = report.start_date.AddMonths(1);
                    }
                    report.du_datetime_list.Add(report.start_date);
                    report.line_series1.Color = Color.Salmon;
                    report.line_series2.Color = Color.MediumSlateBlue;
                    report.line_series1.ChartType = SeriesChartType.Line;
                    report.line_series2.ChartType = SeriesChartType.Line;
                    report.line_series1.Points.Clear();
                    report.line_series2.Points.Clear();
                    for (int index = 0; index < monthly_expense.Length; index++)
                    {
                        report.line_series1.Points.AddXY(report.du_datetime_list[index].Month.ToString(), -monthly_expense[index]);
                        report.line_series2.Points.AddXY(report.du_datetime_list[index].Month.ToString(), monthly_income[index]);
                    }
                    report_linechart.Series.Add(report.line_series1);
                    report_linechart.Series.Add(report.line_series2);
                    report_linechart.Series[0].BorderWidth = 2;
                    report_linechart.Series[1].BorderWidth = 2;
                    (report.total_expense, report.total_income) = reportDB.get_du_total_amount(year_from, month_from, year_to, month_to);
                    report.bar_series[0].Points.Clear();
                    report.bar_series[1].Points.Clear();
                    report.bar_series[0].Points.Add(-report.total_expense);
                    report.bar_series[1].Points.Add(report.total_income);
                    report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                    report_show_lb.Text = "Duration Report" + Environment.NewLine;
                    report_show_lb.Text += Environment.NewLine;
                    if (monthly_expense.Length % 2 == 0)
                    {
                        for (int i = 0; i < monthly_expense.Length; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < monthly_expense.Length - 1; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                        report_show_lb.Text += report.du_datetime_list[monthly_expense.Length - 1].Year.ToString() + "/" + report.du_datetime_list[monthly_expense.Length - 1].Month.ToString() + Environment.NewLine;
                        report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += Environment.NewLine;
                    }
                    report_show_lb.Text += "------------------------------------------------------------" + Environment.NewLine;
                    report_show_lb.Text += "Total Expense:    " + report.total_expense + Environment.NewLine;
                    report_show_lb.Text += "Total Income:     " + report.total_income + Environment.NewLine;
                }
            }
            catch
            {
                MessageBox.Show("輸入格式錯誤!");
            }
        }


        private void report_month_from_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (report_year_from_cb.Text != "" && report_year_to_cb.Text != "" && report_month_from_cb.Text != "" && report_month_to_cb.Text != "")
                {
                    string year_from;
                    string year_to;
                    string month_from;
                    string month_to;
                    int[] monthly_expense;
                    int[] monthly_income;
                    year_from = report_year_from_cb.Text;
                    year_to = report_year_to_cb.Text;
                    month_from = report_month_from_cb.Text;
                    month_to = report_month_to_cb.Text;
                    (monthly_expense, monthly_income) = reportDB.get_duration_data(year_from, month_from, year_to, month_to);
                    report.start_date = Convert.ToDateTime(month_from + "/" + year_from);
                    report.end_date = Convert.ToDateTime(month_to + "/" + year_to);
                    report.du_datetime_list.Clear();
                    report_linechart.Series.Clear();
                    while (report.start_date != report.end_date)
                    {
                        report.du_datetime_list.Add(report.start_date);
                        report.start_date = report.start_date.AddMonths(1);
                    }
                    report.du_datetime_list.Add(report.start_date);
                    report.line_series1.Color = Color.Salmon;
                    report.line_series2.Color = Color.MediumSlateBlue;
                    report.line_series1.ChartType = SeriesChartType.Line;
                    report.line_series2.ChartType = SeriesChartType.Line;
                    report.line_series1.Points.Clear();
                    report.line_series2.Points.Clear();
                    for (int index = 0; index < monthly_expense.Length; index++)
                    {
                        report.line_series1.Points.AddXY(report.du_datetime_list[index].Month.ToString(), -monthly_expense[index]);
                        report.line_series2.Points.AddXY(report.du_datetime_list[index].Month.ToString(), monthly_income[index]);
                    }
                    report_linechart.Series.Add(report.line_series1);
                    report_linechart.Series.Add(report.line_series2);
                    report_linechart.Series[0].BorderWidth = 2;
                    report_linechart.Series[1].BorderWidth = 2;
                    (report.total_expense, report.total_income) = reportDB.get_du_total_amount(year_from, month_from, year_to, month_to);
                    report.bar_series[0].Points.Clear();
                    report.bar_series[1].Points.Clear();
                    report.bar_series[0].Points.Add(-report.total_expense);
                    report.bar_series[1].Points.Add(report.total_income);
                    report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                    report_show_lb.Text = "Duration Report" + Environment.NewLine;
                    report_show_lb.Text += Environment.NewLine;
                    if (monthly_expense.Length % 2 == 0)
                    {
                        for (int i = 0; i < monthly_expense.Length; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < monthly_expense.Length - 1; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                        report_show_lb.Text += report.du_datetime_list[monthly_expense.Length - 1].Year.ToString() + "/" + report.du_datetime_list[monthly_expense.Length - 1].Month.ToString() + Environment.NewLine;
                        report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += Environment.NewLine;
                    }
                    report_show_lb.Text += "------------------------------------------------------------" + Environment.NewLine;
                    report_show_lb.Text += "Total Expense:    " + report.total_expense + Environment.NewLine;
                    report_show_lb.Text += "Total Income:     " + report.total_income + Environment.NewLine;
                }
            }
            catch
            {
                MessageBox.Show("輸入格式錯誤!");
            }
        }

        private void report_year_to_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (report_year_from_cb.Text != "" && report_year_to_cb.Text != "" && report_month_from_cb.Text != "" && report_month_to_cb.Text != "")
                {
                    string year_from;
                    string year_to;
                    string month_from;
                    string month_to;
                    int[] monthly_expense;
                    int[] monthly_income;
                    year_from = report_year_from_cb.Text;
                    year_to = report_year_to_cb.Text;
                    month_from = report_month_from_cb.Text;
                    month_to = report_month_to_cb.Text;
                    (monthly_expense, monthly_income) = reportDB.get_duration_data(year_from, month_from, year_to, month_to);
                    report.start_date = Convert.ToDateTime(month_from + "/" + year_from);
                    report.end_date = Convert.ToDateTime(month_to + "/" + year_to);
                    report.du_datetime_list.Clear();
                    report_linechart.Series.Clear();
                    while (report.start_date != report.end_date)
                    {
                        report.du_datetime_list.Add(report.start_date);
                        report.start_date = report.start_date.AddMonths(1);
                    }
                    report.du_datetime_list.Add(report.start_date);
                    report.line_series1.Color = Color.Salmon;
                    report.line_series2.Color = Color.MediumSlateBlue;
                    report.line_series1.ChartType = SeriesChartType.Line;
                    report.line_series2.ChartType = SeriesChartType.Line;
                    report.line_series1.Points.Clear();
                    report.line_series2.Points.Clear();
                    for (int index = 0; index < monthly_expense.Length; index++)
                    {
                        report.line_series1.Points.AddXY(report.du_datetime_list[index].Month.ToString(), -monthly_expense[index]);
                        report.line_series2.Points.AddXY(report.du_datetime_list[index].Month.ToString(), monthly_income[index]);
                    }
                    report_linechart.Series.Add(report.line_series1);
                    report_linechart.Series.Add(report.line_series2);
                    report_linechart.Series[0].BorderWidth = 2;
                    report_linechart.Series[1].BorderWidth = 2;
                    (report.total_expense, report.total_income) = reportDB.get_du_total_amount(year_from, month_from, year_to, month_to);
                    report.bar_series[0].Points.Clear();
                    report.bar_series[1].Points.Clear();
                    report.bar_series[0].Points.Add(-report.total_expense);
                    report.bar_series[1].Points.Add(report.total_income);
                    report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                    report_show_lb.Text = "Duration Report" + Environment.NewLine;
                    report_show_lb.Text += Environment.NewLine;
                    if (monthly_expense.Length % 2 == 0)
                    {
                        for (int i = 0; i < monthly_expense.Length; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < monthly_expense.Length - 1; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                        report_show_lb.Text += report.du_datetime_list[monthly_expense.Length - 1].Year.ToString() + "/" + report.du_datetime_list[monthly_expense.Length - 1].Month.ToString() + Environment.NewLine;
                        report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += Environment.NewLine;
                    }
                    report_show_lb.Text += "------------------------------------------------------------" + Environment.NewLine;
                    report_show_lb.Text += "Total Expense:    " + report.total_expense + Environment.NewLine;
                    report_show_lb.Text += "Total Income:     " + report.total_income + Environment.NewLine;
                }
            }
            catch
            {
                MessageBox.Show("輸入格式錯誤!");
            }
        }

        private void report_month_to_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (report_year_from_cb.Text != "" && report_year_to_cb.Text != "" && report_month_from_cb.Text != "" && report_month_to_cb.Text != "")
                {
                    string year_from;
                    string year_to;
                    string month_from;
                    string month_to;
                    int[] monthly_expense;
                    int[] monthly_income;
                    year_from = report_year_from_cb.Text;
                    year_to = report_year_to_cb.Text;
                    month_from = report_month_from_cb.Text;
                    month_to = report_month_to_cb.Text;
                    (monthly_expense, monthly_income) = reportDB.get_duration_data(year_from, month_from, year_to, month_to);
                    report.start_date = Convert.ToDateTime(month_from + "/" + year_from);
                    report.end_date = Convert.ToDateTime(month_to + "/" + year_to);
                    report.du_datetime_list.Clear();
                    report_linechart.Series.Clear();
                    while (report.start_date != report.end_date)
                    {
                        report.du_datetime_list.Add(report.start_date);
                        report.start_date = report.start_date.AddMonths(1);
                    }
                    report.du_datetime_list.Add(report.start_date);
                    report.line_series1.Color = Color.Salmon;
                    report.line_series2.Color = Color.MediumSlateBlue;
                    report.line_series1.ChartType = SeriesChartType.Line;
                    report.line_series2.ChartType = SeriesChartType.Line;
                    report.line_series1.Points.Clear();
                    report.line_series2.Points.Clear();
                    for (int index = 0; index < monthly_expense.Length; index++)
                    {
                        report.line_series1.Points.AddXY(report.du_datetime_list[index].Month.ToString(), -monthly_expense[index]);
                        report.line_series2.Points.AddXY(report.du_datetime_list[index].Month.ToString(), monthly_income[index]);
                    }
                    report_linechart.Series.Add(report.line_series1);
                    report_linechart.Series.Add(report.line_series2);
                    report_linechart.Series[0].BorderWidth = 2;
                    report_linechart.Series[1].BorderWidth = 2;
                    (report.total_expense, report.total_income) = reportDB.get_du_total_amount(year_from, month_from, year_to, month_to);
                    report.bar_series[0].Points.Clear();
                    report.bar_series[1].Points.Clear();
                    report.bar_series[0].Points.Add(-report.total_expense);
                    report.bar_series[1].Points.Add(report.total_income);
                    report_barchart.ChartAreas["ChartArea1"].AxisY.Maximum = -report.total_expense + report.total_income;
                    report_show_lb.Text = "Duration Report" + Environment.NewLine;
                    report_show_lb.Text += Environment.NewLine;
                    if (monthly_expense.Length % 2 == 0)
                    {
                        for (int i = 0; i < monthly_expense.Length; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < monthly_expense.Length - 1; i++)
                        {
                            report_show_lb.Text += report.du_datetime_list[i].Year.ToString() + "/" + report.du_datetime_list[i].Month.ToString() + "                   " + report.du_datetime_list[i + 1].Year.ToString() + "/" + report.du_datetime_list[i + 1].Month.ToString() + Environment.NewLine;
                            report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[i]) + "Expense:  " + monthly_expense[i + 1] + Environment.NewLine;
                            report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[i]) + "Income:   " + monthly_income[i + 1] + Environment.NewLine;
                            i = i + 1;
                            report_show_lb.Text += Environment.NewLine;
                        }
                        report_show_lb.Text += report.du_datetime_list[monthly_expense.Length - 1].Year.ToString() + "/" + report.du_datetime_list[monthly_expense.Length - 1].Month.ToString() + Environment.NewLine;
                        report_show_lb.Text += "Expense:  " + string.Format("{0,-15}", monthly_expense[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += "Income:   " + string.Format("{0,-15}", monthly_income[monthly_expense.Length - 1]) + Environment.NewLine;
                        report_show_lb.Text += Environment.NewLine;
                    }
                    report_show_lb.Text += "------------------------------------------------------------" + Environment.NewLine;
                    report_show_lb.Text += "Total Expense:    " + report.total_expense + Environment.NewLine;
                    report_show_lb.Text += "Total Income:     " + report.total_income + Environment.NewLine;
                }
            }
            catch
            {
                MessageBox.Show("輸入格式錯誤!");
            }
        }

        #endregion

        #region Setting
        private void setting_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  category
            if (setting_tabControl.SelectedIndex == 3)
            {
                //  set title
                setting_title.Left = 100;
                setting_title.Text = "Category";
                //  set background
                main_settings.BackColor = Color.NavajoWhite;
                setting_category.BackColor = Color.NavajoWhite;

                //  set list box
                setting_update_List2ListBox("TotalList");
            }

            //  list
            if (setting_tabControl.SelectedIndex == 2)
            {
                //  set title
                setting_title.Left = 165;
                setting_title.Text = "List";
                //  set background
                main_settings.BackColor = Color.Wheat;
                setting_list.BackColor = Color.Wheat;
            }
            //  budget
            if (setting_tabControl.SelectedIndex == 1)
            {
                //  set title
                setting_title.Left = 160;
                setting_title.Text = "Budget";
                //  set background
                main_settings.BackColor = Color.Moccasin;
                setting_budget.BackColor = Color.Moccasin;

                //  get_DB
                setting_budget_cycletime_cb.Text = settingDB.get_budget_cycleDay();
                setting_budget_total_txt.Text = settingDB.get_budgetAmount();

                //  show remaining budget
                settingVar.startDate = SettingFunction.get_date(setting_budget_cycletime_cb.Text, true);
                settingVar.endDate = SettingFunction.get_date(setting_budget_cycletime_cb.Text, false).AddDays(-1);

                setting_budget_result_lbl1.Text = "截至 " + settingVar.endDate.Month.ToString() + " 月 " + settingVar.endDate.Day.ToString() + " 日前";
                setting_budget_result_money.Text = (int.Parse(settingDB.get_budgetAmount()) + settingDB.get_sum_period(settingVar.startDate, settingVar.endDate)).ToString();
                setting_budget_result_money.ForeColor = SettingFunction.budgetColor(int.Parse(setting_budget_result_money.Text), int.Parse(settingDB.get_budgetAmount()));
            }

            //  notification
            if (setting_tabControl.SelectedIndex == 0)
            {
                //  set title
                setting_title.Left = 45;
                setting_title.Text = "Notification";
                //  set background
                main_settings.BackColor = Color.AntiqueWhite;
                setting_notification.BackColor = Color.AntiqueWhite;
            }
        }
        #region category
        private void setting_category_inc_add_Click(object sender, EventArgs e)
        {
            SettingInfoForm infoForm = new SettingInfoForm("Income", this);
            infoForm.Show();
        }
        private void setting_category_exp_add_Click(object sender, EventArgs e)
        {
            SettingInfoForm infoForm = new SettingInfoForm("Expense", this);
            infoForm.Show();
        }
        private void setting_category_inc_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = setting_category_inc_listbox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                settingVar.category_incClickItem = setting_category_inc_listbox.IndexFromPoint(e.Location);
                SettingInfoForm infoForm = new SettingInfoForm("Income", this, setting_category_inc_listbox.SelectedItem.ToString());
                infoForm.Show();
            }
            else
            {
                setting_category_inc_listbox.SelectedIndex = -1;
            }
        }
        private void setting_category_exp_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = setting_category_exp_listbox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                settingVar.category_expClickItem = setting_category_exp_listbox.IndexFromPoint(e.Location);
                SettingInfoForm infoForm = new SettingInfoForm("Expense", this, setting_category_exp_listbox.SelectedItem.ToString());
                infoForm.Show();
            }
            else
            {
                setting_category_exp_listbox.SelectedIndex = -1;
            }
        }
        #endregion
        #region budget
        private void setting_budget_total_txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int validation = int.Parse(setting_budget_total_txt.Text);
            }
            catch
            {
                if (setting_budget_total_txt.Text != "")
                    MessageBox.Show("請輸入數字");

                return;
            }
            settingDB.set_bugetAmount(setting_budget_total_txt.Text);
            setting_budget_result_money.Text = (int.Parse(settingDB.get_budgetAmount()) + settingDB.get_sum_period(settingVar.startDate, settingVar.endDate)).ToString();
            setting_budget_result_money.ForeColor = SettingFunction.budgetColor(int.Parse(setting_budget_result_money.Text), int.Parse(settingDB.get_budgetAmount()));
        }
        private void setting_budget_cycletime_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int validation = int.Parse(setting_budget_cycletime_cb.Text);
                if (!(1 <= validation && validation <= 31))
                {
                    MessageBox.Show("日期非法");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("日期非法");
                return;
            }
            settingDB.set_budget_cycleDay(setting_budget_cycletime_cb.Text);

            //  show remaining budget
            settingVar.startDate = SettingFunction.get_date(setting_budget_cycletime_cb.Text, true);
            settingVar.endDate = SettingFunction.get_date(setting_budget_cycletime_cb.Text, false).AddDays(-1);

            setting_budget_result_lbl1.Text = "截至 " + settingVar.endDate.Month.ToString() + " 月 " + settingVar.endDate.Day.ToString() + " 日前";
            setting_budget_result_money.Text = (int.Parse(settingDB.get_budgetAmount()) + settingDB.get_sum_period(settingVar.startDate, settingVar.endDate)).ToString();
            setting_budget_result_money.ForeColor = SettingFunction.budgetColor(int.Parse(setting_budget_result_money.Text), int.Parse(settingDB.get_budgetAmount()));
        }
        #endregion
        #region list
        private void setting_list_place_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = setting_list_place_listbox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                settingVar.list_placeClickItem = setting_list_place_listbox.IndexFromPoint(e.Location);
                SettingInfoForm infoForm = new SettingInfoForm("Place", this, setting_list_place_listbox.SelectedItem.ToString());
                infoForm.Show();
            }
            else
            {
                setting_list_place_listbox.SelectedIndex = -1;
            }
        }
        private void setting_list_recipient_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = setting_list_recipient_listbox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                settingVar.list_recipientClickItem = setting_list_recipient_listbox.IndexFromPoint(e.Location);
                SettingInfoForm infoForm = new SettingInfoForm("Recipient", this, setting_list_recipient_listbox.SelectedItem.ToString());
                infoForm.Show();
            }
            else
            {
                setting_list_recipient_listbox.SelectedIndex = -1;
            }
        }
        #endregion
        #region notification
        private void setting_notification_listbox_MouseClick(object sender, MouseEventArgs e)
        {
            int index = setting_notification_listbox.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                settingVar.notificationClickItem = setting_notification_listbox.IndexFromPoint(e.Location);
                setting_notification_info_modify.Enabled = true;
                setting_notification_info_delete.Enabled = true;

                settingVar.notificationOriText = setting_notification_listbox.SelectedItem.ToString();

                setting_notification_title_txt.Text = settingVar.notification_infos[settingVar.notificationClickItem].notiTitle;
                setting_notification_content_txt.Text = settingVar.notification_infos[settingVar.notificationClickItem].notiContent;
                setting_notification_date_cb.Text = settingVar.notification_infos[settingVar.notificationClickItem].notiDate.Day.ToString();
            }
            else
            {
                setting_notification_info_modify.Enabled = false;
                setting_notification_info_delete.Enabled = false;
                settingVar.notificationClickItem = -1;
                setting_notification_listbox.SelectedIndex = -1;
            }
        }
        private void setting_notification_info_add_Click(object sender, EventArgs e)
        {

            if (setting_notification_title_txt.Text == "")
            {
                MessageBox.Show("請輸入標題");
            }
            else if (setting_notification_date_cb.Text == "")
            {
                MessageBox.Show("請輸入日期");
            }
            else
            {
                try
                {
                    int validation = int.Parse(setting_notification_date_cb.Text);
                    if (!(1 <= validation && validation <= 25))
                    {
                        MessageBox.Show("日期非法");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("日期非法");
                    return;
                }

                DateTime notiDate;
                if (int.Parse(setting_notification_date_cb.Text) <= DateTime.Now.Day)
                    notiDate = DateTime.Now;
                else
                    notiDate = DateTime.Now.AddMonths(1);

                SettingNotification settingNotification = new SettingNotification();
                settingNotification.notiTitle = setting_notification_title_txt.Text;
                settingNotification.notiContent = setting_notification_content_txt.Text;
                settingNotification.notiDate = new DateTime(notiDate.Year, notiDate.Month, int.Parse(setting_notification_date_cb.Text));
                settingVar.notification_infos.Add(settingNotification);
                settingDB.set_notification(settingVar.notification_infos);
                SettingFunction.update_NotificationListBox(setting_notification_listbox, settingVar.notification_infos);
            }

        }
        private void setting_notification_info_modify_Click(object sender, EventArgs e)
        {
            if (setting_notification_title_txt.Text == "")
            {
                MessageBox.Show("請輸入標題");
            }
            else if (setting_notification_date_cb.Text == "")
            {
                MessageBox.Show("請輸入日期");
            }
            else
            {
                try
                {
                    int validation = int.Parse(setting_notification_date_cb.Text);
                    if (!(1 <= validation && validation <= 25))
                    {
                        MessageBox.Show("日期非法");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("日期非法");
                    return;
                }

                DateTime notiDate;
                if (int.Parse(setting_notification_date_cb.Text) <= DateTime.Now.Day)
                    notiDate = DateTime.Now;
                else
                    notiDate = DateTime.Now.AddMonths(1);

                settingVar.notification_infos[settingVar.notificationClickItem].notiTitle = setting_notification_title_txt.Text;
                settingVar.notification_infos[settingVar.notificationClickItem].notiContent = setting_notification_content_txt.Text;
                settingVar.notification_infos[settingVar.notificationClickItem].notiDate = new DateTime(notiDate.Year, notiDate.Month, int.Parse(setting_notification_date_cb.Text));
                settingDB.set_notification(settingVar.notification_infos);
                SettingFunction.update_NotificationListBox(setting_notification_listbox, settingVar.notification_infos);
                setting_notification_info_modify.Enabled = false;
                setting_notification_info_delete.Enabled = false;
            }
        }
        private void setting_notification_info_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要刪除 " + settingVar.notificationOriText + " 嗎?", "通知列表", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                settingVar.notification_infos.RemoveAt(settingVar.notificationClickItem);
                settingDB.set_notification(settingVar.notification_infos);
                SettingFunction.update_NotificationListBox(setting_notification_listbox, settingVar.notification_infos);
                setting_notification_info_modify.Enabled = false;
                setting_notification_info_delete.Enabled = false;
            }
        }
        private void setting_notification_timer_Tick(object sender, EventArgs e)
        {
            SettingFunction.notification(settingVar.notification_infos, settingDB);
        }
        #endregion
        #region function
        public void setting_list_add(string type, string str)
        {
            switch (type)
            {
                default:
                    return;
                case "Income":
                    settingVar.category_inc.Add(str);
                    settingDB.set_category("Income", settingVar.category_inc);
                    break;
                case "Expense":
                    settingVar.category_exp.Add(str);
                    settingDB.set_category("Expense", settingVar.category_exp);
                    break;
            }
        }
        public void setting_list_modify(string type, string origintext, string modifytext)
        {
            switch (type)
            {
                default:
                    return;
                case "Income":
                    settingVar.category_inc[settingVar.category_incClickItem] = modifytext;
                    settingDB.set_category("Income", settingVar.category_inc);
                    break;
                case "Expense":
                    settingVar.category_exp[settingVar.category_expClickItem] = modifytext;
                    settingDB.set_category("Expense", settingVar.category_exp);
                    break;
                case "Place":
                    settingVar.list_place[settingVar.list_placeClickItem] = modifytext;
                    settingDB.set_list("Place", settingVar.list_place);
                    break;
                case "Recipient":
                    settingVar.list_recipient[settingVar.list_recipientClickItem] = modifytext;
                    settingDB.set_list("Recipient", settingVar.list_recipient);
                    break;
            }
        }
        public void setting_list_remove(string type, string origintext)
        {
            switch (type)
            {
                default:
                    return;
                case "Income":
                    settingVar.category_inc.RemoveAt(settingVar.category_incClickItem);
                    settingDB.set_category("Income", settingVar.category_inc);
                    break;
                case "Expense":
                    settingVar.category_exp.RemoveAt(settingVar.category_expClickItem);
                    settingDB.set_category("Expense", settingVar.category_exp);
                    break;
                case "Place":
                    settingVar.list_place.RemoveAt(settingVar.list_placeClickItem);
                    settingDB.set_list("Place", settingVar.list_place);
                    break;
                case "Recipient":
                    settingVar.list_recipient.RemoveAt(settingVar.list_recipientClickItem);
                    settingDB.set_list("Recipient", settingVar.list_recipient);
                    break;
            }
        }
        public void setting_update_List2ListBox(string type)
        {
            switch (type)
            {
                default:
                    return;
                case "Income":
                    SettingFunction.update_List2ListBox(setting_category_inc_listbox, settingVar.category_inc);
                    break;
                case "Expense":
                    SettingFunction.update_List2ListBox(setting_category_exp_listbox, settingVar.category_exp);
                    break;
                case "Place":
                    SettingFunction.update_List2ListBox(setting_list_place_listbox, settingVar.list_place);
                    break;
                case "Recipient":
                    SettingFunction.update_List2ListBox(setting_list_recipient_listbox, settingVar.list_recipient);
                    break;
                case "TotalList":
                    SettingFunction.update_List2ListBox(setting_category_inc_listbox, settingVar.category_inc);
                    SettingFunction.update_List2ListBox(setting_category_exp_listbox, settingVar.category_exp);
                    SettingFunction.update_List2ListBox(setting_list_place_listbox, settingVar.list_place);
                    SettingFunction.update_List2ListBox(setting_list_recipient_listbox, settingVar.list_recipient);
                    break;
            }
        }
        #endregion
        #endregion




        private void trans_btn_save_Click(object sender, EventArgs e)
        {
            bool error = true;
            bool inputError = false;
            if (tran_rdb_transfer.Checked)
            {
                int amount = 0;
                try
                {
                    amount = int.Parse(transactionGlobal.transfer_txtbox_amount.Text);
                }
                catch
                {
                    MessageBox.Show("金額輸入錯誤");
                    inputError = true;
                }

                if (amount <= 0)
                {
                    MessageBox.Show("請輸入正數");
                    inputError = true;
                }
                else if (transactionGlobal.transfer_cbo_from.Text == "")
                {
                    MessageBox.Show("請選擇轉出帳戶");
                    inputError = true;
                }
                else if (transactionGlobal.transfer_cbo_to.Text == "")
                {
                    MessageBox.Show("請選擇轉入帳戶");
                    inputError = true;
                }

                if (!inputError)
                {
                    db.transferMoney(from: transactionGlobal.transfer_cbo_from.Text,
                        to: transactionGlobal.transfer_cbo_to.Text,
                        amount: amount);
                    transactionGlobal.clearTransferInput();
                    error = false;
                }
            }
            else if (tran_rdb_debtandloan.Checked)
            {
                DebtLoan inputDebtLoan = new DebtLoan();

                // Check Detail
                if(transactionGlobal.trans_txtbox_name.Text == "")
                {
                    MessageBox.Show("請輸入款項名稱");
                    inputError = true;
                }
                else
                {
                    inputDebtLoan.detail = transactionGlobal.trans_txtbox_name.Text;
                }
                // Check Amount
                int amount = 0;
                try
                {
                    amount = int.Parse(transactionGlobal.trans_txtbox_amount.Text);
                    if (amount <= 0)
                    {
                        MessageBox.Show("請輸入正數");
                        inputError = true;
                    }
                }
                catch
                {
                    MessageBox.Show("金額輸入錯誤");
                    inputError = true;
                }

                // Check Type, Account
                if (transactionGlobal.trans_cbo_category.Text == "")
                {
                    MessageBox.Show("請選擇款項種類");
                    inputError = true;
                }
                else
                {
                    inputDebtLoan.type = transactionGlobal.trans_cbo_category.Text;
                    if (inputDebtLoan.type == "Loan")
                    {
                        if (transactionGlobal.trans_cbo_account.Text == "")
                        {
                            MessageBox.Show("請選擇帳戶");
                            inputError = true;
                        }
                        else
                        {
                            inputDebtLoan.account = transactionGlobal.trans_cbo_account.Text;
                        }
                    }
                    else
                    {
                        inputDebtLoan.account = transactionGlobal.trans_cbo_account.Text;
                    }
                }
                
                // Check Recipient
                if(transactionGlobal.trans_txtbox_store.Text == "")
                {
                    MessageBox.Show("請輸入對象");
                    inputError = true;
                }
                else
                {
                    inputDebtLoan.recipient = transactionGlobal.trans_txtbox_store.Text;
                    List<string> listRecipient = db.get_list_recipient();
                    if (!listRecipient.Contains(inputDebtLoan.recipient))
                    {
                        db.insertRecipient(inputDebtLoan.recipient);
                    }
                }

                if (!inputError)
                {
                    inputDebtLoan.amount = amount;
                    inputDebtLoan.date = transactionGlobal.trans_date.Value;
                    db.insertDebtLoan(inputValue: inputDebtLoan);
                    transactionGlobal.clearTransInput();
                    error = false;
                }
            }

            //income and expense
            else
            {
                Transaction transInput = new Transaction();

                // Check Name
                if (transactionGlobal.trans_txtbox_name.Text == "")
                {
                    MessageBox.Show("請輸入款項名稱");
                    inputError = true;
                }
                else
                {
                    transInput.name = transactionGlobal.trans_txtbox_name.Text;
                }

                // Check Amount
                int amount = 0;
                try
                {
                    amount = int.Parse(transactionGlobal.trans_txtbox_amount.Text);
                    if (amount <= 0)
                    {
                        MessageBox.Show("請輸入正數");
                        inputError = true;
                    }
                }
                catch
                {
                    MessageBox.Show("金額輸入錯誤");
                    inputError = true;
                }

                // Check Category
                if (transactionGlobal.trans_cbo_category.Text == "")
                {
                    MessageBox.Show("請選擇款項種類");
                    inputError = true;
                }
                else
                {
                    transInput.category = transactionGlobal.trans_cbo_category.Text;
                }

                // Check Account
                if (transactionGlobal.trans_cbo_account.Text == "")
                {
                    MessageBox.Show("請選擇帳戶");
                    inputError = true;
                }
                else
                {
                    transInput.account = transactionGlobal.trans_cbo_account.Text;
                }

                if (transactionGlobal.trans_txtbox_store.Text == "")
                {
                    transInput.place = transactionGlobal.trans_txtbox_store.Text;
                }
                else
                {
                    List<string> listPlace = db.get_list_place();
                    transInput.place = transactionGlobal.trans_txtbox_store.Text;
                    if (!listPlace.Contains(transInput.place))
                    {
                        db.insertPlace(transInput.place);
                    }
                }

                if (!inputError)
                {
                    transInput.amount = amount;
                    transInput.date = transactionGlobal.trans_date.Value;

                    transInput.comment = transactionGlobal.trans_txtbox_note.Text;
                    if (trans_rdb_expense.Checked)
                    {
                        db.insertTransaction(transInput, "Expense");
                        transactionGlobal.clearTransInput();
                        error = false;
                    }
                    else if (trans_rdb_income.Checked)
                    {
                        db.insertTransaction(transInput, "Income");
                        transactionGlobal.clearTransInput();
                        error = false;
                    }
                }
            }

            if (!error)
            {
                if (tran_rdb_transfer.Checked)
                {
                    trans_transfer_display();
                }
                else if (tran_rdb_debtandloan.Checked)
                {
                    trans_debtLoan_display();
                }
                else if (trans_rdb_expense.Checked)
                {
                    trans_expense_display();
                }
                else if (trans_rdb_income.Checked)
                {
                    trans_income_display();
                }

                MessageBox.Show("已成功新增一筆紀錄！");
            }
        }


        void trans_expense_display()
        {
            trans_panel_budget.Visible = true;
            trans_panel1.Controls.Clear();
            trans_panel1.Enabled = true;
            Label trans_name = new Label();
            trans_name.Location = new Point(10, 10);
            trans_name.Text = "款項名稱";
            TextBox trans_txtbox_name = new TextBox();
            trans_txtbox_name.Location = new Point(125, 10);

            Label trans_amount = new Label();
            trans_amount.Location = new Point(400, 10);
            trans_amount.Text = "款項金額";
            TextBox trans_txtbox_amount = new TextBox();
            trans_txtbox_amount.Location = new Point(515, 10);

            Label trans_category = new Label();
            trans_category.Location = new Point(10, 70);
            trans_category.Text = "款項種類";
            ComboBox trans_cbo_category = new ComboBox();
            trans_cbo_category.Location = new Point(125, 70);
            trans_cbo_category.Size = new Size(200, 28);
            List<string> trans_list_category = new List<string>();
            trans_list_category = db.get_category("Expense");
            foreach (string ele in trans_list_category)
            {
                trans_cbo_category.Items.Add(ele);
            }

            Label trans_account = new Label();
            trans_account.Location = new Point(400, 70);
            trans_account.Text = "帳戶";
            ComboBox trans_cbo_account = new ComboBox();
            trans_cbo_account.Location = new Point(515, 70);
            trans_cbo_account.Size = new Size(200, 28);
            string[] trans_list_account = db.get_all_accounts();
            foreach (string ele in trans_list_account)
            {
                if (ele != "Debt/Loan")
                {
                    trans_cbo_account.Items.Add(ele);
                }
            }

            Label trans_date = new Label();
            trans_date.Location = new Point(10, 130);
            trans_date.Text = "記錄日期";
            DateTimePicker date = new DateTimePicker();
            date.Location = new Point(125, 130);

            Label trans_store = new Label();
            trans_store.Location = new Point(400, 130);
            trans_store.Text = "商家";
            TextBox trans_txtbox_store = new TextBox();
            trans_txtbox_store.Location = new Point(515, 130);

            AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
            myAdd.AddRange(db.get_list_place().ToArray());
            trans_txtbox_store.AutoCompleteCustomSource = myAdd;
            trans_txtbox_store.AutoCompleteMode = AutoCompleteMode.Suggest;
            trans_txtbox_store.AutoCompleteSource = AutoCompleteSource.CustomSource;


            Label trans_note = new Label();
            trans_note.Location = new Point(10, 190);
            trans_note.Text = "備註";
            TextBox trans_txtbox_note = new TextBox();
            trans_txtbox_note.Location = new Point(125, 190);

            trans_panel1.Controls.Add(trans_name);
            trans_panel1.Controls.Add(trans_txtbox_name);
            trans_panel1.Controls.Add(trans_amount);
            trans_panel1.Controls.Add(trans_txtbox_amount);
            trans_panel1.Controls.Add(trans_category);
            trans_panel1.Controls.Add(trans_cbo_category);
            trans_panel1.Controls.Add(trans_account);
            trans_panel1.Controls.Add(trans_cbo_account);
            trans_panel1.Controls.Add(trans_date);
            trans_panel1.Controls.Add(date);
            trans_panel1.Controls.Add(trans_store);
            trans_panel1.Controls.Add(trans_txtbox_store);
            trans_panel1.Controls.Add(trans_note);
            trans_panel1.Controls.Add(trans_txtbox_note);
            trans_panel1.Show();

            transactionGlobal.trans_txtbox_name = trans_txtbox_name;
            transactionGlobal.trans_txtbox_amount = trans_txtbox_amount;
            transactionGlobal.trans_cbo_category = trans_cbo_category;
            transactionGlobal.trans_cbo_account = trans_cbo_account;
            transactionGlobal.trans_date = date;
            transactionGlobal.trans_txtbox_store = trans_txtbox_store;
            transactionGlobal.trans_txtbox_note = trans_txtbox_note;

            //trans_panel_budget.Show();
            trans_remainder.Text = (int.Parse(settingDB.get_budgetAmount()) + settingDB.get_sum_period(settingVar.startDate, settingVar.endDate)).ToString();

        }


        void trans_income_display()
        {
            trans_panel_budget.Visible = false;
            //Panel trans_panel1 = new Panel();
            trans_panel1.Controls.Clear();
            trans_panel1.Enabled = true;
            Label trans_name = new Label();
            trans_name.Location = new Point(10, 10);
            trans_name.Text = "款項名稱";
            TextBox trans_txtbox_name = new TextBox();
            trans_txtbox_name.Location = new Point(125, 10);

            Label trans_amount = new Label();
            trans_amount.Location = new Point(400, 10);
            trans_amount.Text = "款項金額";
            TextBox trans_txtbox_amount = new TextBox();
            trans_txtbox_amount.Location = new Point(515, 10);

            Label trans_category = new Label();
            trans_category.Location = new Point(10, 70);
            trans_category.Text = "款項種類";
            ComboBox trans_cbo_category = new ComboBox();
            trans_cbo_category.Location = new Point(125, 70);
            trans_cbo_category.Size = new Size(200, 28);
            List<string> trans_list_category = new List<string>();
            trans_list_category = db.get_category("Income");
            foreach (string ele in trans_list_category)
            {
                trans_cbo_category.Items.Add(ele);
            }

            Label trans_account = new Label();
            trans_account.Location = new Point(400, 70);
            trans_account.Text = "帳戶";
            ComboBox trans_cbo_account = new ComboBox();
            trans_cbo_account.Location = new Point(515, 70);
            trans_cbo_account.Size = new Size(200, 28);
            string[] trans_list_account = db.get_all_accounts();
            foreach (string ele in trans_list_account)
            {
                if (ele != "Debt/Loan")
                {
                    trans_cbo_account.Items.Add(ele);
                }
            }

            Label trans_date = new Label();
            trans_date.Location = new Point(10, 130);
            trans_date.Text = "記錄日期";
            DateTimePicker date = new DateTimePicker();
            date.Location = new Point(125, 130);

            Label trans_store = new Label();
            trans_store.Location = new Point(400, 130);
            trans_store.Text = "商家";
            TextBox trans_txtbox_store = new TextBox();
            trans_txtbox_store.Location = new Point(515, 130);

            AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
            myAdd.AddRange(db.get_list_place().ToArray());
            trans_txtbox_store.AutoCompleteCustomSource = myAdd;
            trans_txtbox_store.AutoCompleteMode = AutoCompleteMode.Suggest;
            trans_txtbox_store.AutoCompleteSource = AutoCompleteSource.CustomSource;

            Label trans_note = new Label();
            trans_note.Location = new Point(10, 190);
            trans_note.Text = "備註";
            TextBox trans_txtbox_note = new TextBox();
            trans_txtbox_note.Location = new Point(125, 190);

            #region Add to panel
            trans_panel1.Controls.Add(trans_name);
            trans_panel1.Controls.Add(trans_txtbox_name);
            trans_panel1.Controls.Add(trans_amount);
            trans_panel1.Controls.Add(trans_txtbox_amount);
            trans_panel1.Controls.Add(trans_category);
            trans_panel1.Controls.Add(trans_cbo_category);
            trans_panel1.Controls.Add(trans_account);
            trans_panel1.Controls.Add(trans_cbo_account);
            trans_panel1.Controls.Add(trans_date);
            trans_panel1.Controls.Add(date);
            trans_panel1.Controls.Add(trans_store);
            trans_panel1.Controls.Add(trans_txtbox_store);
            trans_panel1.Controls.Add(trans_note);
            trans_panel1.Controls.Add(trans_txtbox_note);
            trans_panel1.Show();
            #endregion

            transactionGlobal.trans_txtbox_name = trans_txtbox_name;
            transactionGlobal.trans_txtbox_amount = trans_txtbox_amount;
            transactionGlobal.trans_cbo_category = trans_cbo_category;
            transactionGlobal.trans_cbo_account = trans_cbo_account;
            transactionGlobal.trans_date = date;
            transactionGlobal.trans_txtbox_store = trans_txtbox_store;
            transactionGlobal.trans_txtbox_note = trans_txtbox_note;
        }

        void trans_debtLoan_display()
        {
            trans_panel_budget.Visible = true;
            //Panel trans_panel1 = new Panel();
            trans_panel1.Controls.Clear();
            trans_panel1.Enabled = true;
            Label trans_name = new Label();
            trans_name.Location = new Point(10, 10);
            trans_name.Text = "款項名稱";
            TextBox trans_txtbox_name = new TextBox();
            trans_txtbox_name.Location = new Point(125, 10);

            Label trans_amount = new Label();
            trans_amount.Location = new Point(400, 10);
            trans_amount.Text = "款項金額";
            TextBox trans_txtbox_amount = new TextBox();
            trans_txtbox_amount.Location = new Point(515, 10);

            Label trans_category = new Label();
            trans_category.Location = new Point(10, 70);
            trans_category.Text = "款項種類";
            ComboBox trans_cbo_category = new ComboBox();
            trans_cbo_category.Location = new Point(125, 70);
            trans_cbo_category.Size = new Size(200, 28);
            string[] trans_debtandloan = new string[] { "Debt", "Loan" };
            trans_cbo_category.Items.Add(trans_debtandloan[0]);
            trans_cbo_category.Items.Add(trans_debtandloan[1]);
            trans_cbo_category.SelectedIndex = 0;
            Label trans_account = new Label();
            trans_account.Location = new Point(400, 70);
            trans_account.Text = "帳戶";
            ComboBox trans_cbo_account = new ComboBox();
            trans_cbo_account.Location = new Point(515, 70);
            trans_cbo_account.Size = new Size(200, 28);
            string[] trans_list_account = db.get_all_accounts();
            foreach (string ele in trans_list_account)
            {
                if (ele != "Debt/Loan")
                {
                    trans_cbo_account.Items.Add(ele);
                }
            }

            Label trans_date = new Label();
            trans_date.Location = new Point(10, 130);
            trans_date.Text = "記錄日期";
            DateTimePicker date = new DateTimePicker();
            date.Location = new Point(125, 130);

            Label trans_store = new Label();
            trans_store.Location = new Point(400, 130);
            trans_store.Text = "對象";
            TextBox trans_txtbox_store = new TextBox();
            trans_txtbox_store.Location = new Point(515, 130);

            AutoCompleteStringCollection myAdd = new AutoCompleteStringCollection();
            myAdd.AddRange(db.get_list_recipient().ToArray());
            trans_txtbox_store.AutoCompleteCustomSource = myAdd;
            trans_txtbox_store.AutoCompleteMode = AutoCompleteMode.Suggest;
            trans_txtbox_store.AutoCompleteSource = AutoCompleteSource.CustomSource;

            trans_panel1.Controls.Add(trans_name);
            trans_panel1.Controls.Add(trans_txtbox_name);
            trans_panel1.Controls.Add(trans_amount);
            trans_panel1.Controls.Add(trans_txtbox_amount);
            trans_panel1.Controls.Add(trans_category);
            trans_panel1.Controls.Add(trans_cbo_category);
            trans_panel1.Controls.Add(trans_account);
            trans_panel1.Controls.Add(trans_cbo_account);
            trans_panel1.Controls.Add(trans_date);
            trans_panel1.Controls.Add(date);
            trans_panel1.Controls.Add(trans_store);
            trans_panel1.Controls.Add(trans_txtbox_store);

            //trans_panel1.Show();

            transactionGlobal.trans_txtbox_name = trans_txtbox_name;
            transactionGlobal.trans_txtbox_amount = trans_txtbox_amount;
            transactionGlobal.trans_cbo_category = trans_cbo_category;
            transactionGlobal.trans_cbo_account = trans_cbo_account;
            transactionGlobal.trans_date = date;
            transactionGlobal.trans_txtbox_store = trans_txtbox_store;
        }

        void trans_transfer_display()
        {
            trans_panel_budget.Visible = false;
            //Panel trans_panel1 = new Panel();
            trans_panel1.Controls.Clear();

            Label trans_amount = new Label();
            trans_amount.Location = new Point(10, 10);
            trans_amount.Text = "款項金額";
            TextBox trans_txtbox_amount = new TextBox();
            trans_txtbox_amount.Location = new Point(125, 10);

            Label trans_transfer_from = new Label();
            trans_transfer_from.Location = new Point(10, 70);
            trans_transfer_from.Text = "轉出帳戶";
            ComboBox trans_cbo_transfer_from = new ComboBox();
            trans_cbo_transfer_from.Location = new Point(125, 70);
            trans_cbo_transfer_from.Size = new Size(200, 28);

            Label trans_transfer_to = new Label();
            trans_transfer_to.Location = new Point(400, 70);
            trans_transfer_to.Text = "轉入帳戶";
            ComboBox trans_cbo_transfer_to = new ComboBox();
            trans_cbo_transfer_to.Location = new Point(515, 70);
            trans_cbo_transfer_to.Size = new Size(200, 28);
            string[] trans_list_account = db.get_all_accounts();
            foreach (string ele in trans_list_account)
            {
                if (ele != "Debt/Loan")
                {
                    trans_cbo_transfer_from.Items.Add(ele);
                    trans_cbo_transfer_to.Items.Add(ele);
                }
            }


            trans_panel1.Controls.Add(trans_amount);
            trans_panel1.Controls.Add(trans_txtbox_amount);
            trans_panel1.Controls.Add(trans_transfer_from);
            trans_panel1.Controls.Add(trans_cbo_transfer_from);
            trans_panel1.Controls.Add(trans_transfer_to);
            trans_panel1.Controls.Add(trans_cbo_transfer_to);
            trans_panel1.Show();

            transactionGlobal.transfer_txtbox_amount = trans_txtbox_amount;
            transactionGlobal.transfer_cbo_from = trans_cbo_transfer_from;
            transactionGlobal.transfer_cbo_to = trans_cbo_transfer_to;

        }

        private void trans_rdb_income_CheckedChanged(object sender, EventArgs e)
        {
            if (trans_rdb_income.Checked)
            {
                trans_income_display();
            }
        }

        private void tran_rdb_debtandloan_CheckedChanged(object sender, EventArgs e)
        {
            if (tran_rdb_debtandloan.Checked)
            {
                trans_debtLoan_display();
            }
        }

        private void tran_rdb_transfer_CheckedChanged(object sender, EventArgs e)
        {
            if (tran_rdb_transfer.Checked)
            {
                trans_transfer_display();
            }
        }

        private void trans_rdb_expense_CheckedChanged(object sender, EventArgs e)
        {
            if (trans_rdb_expense.Checked)
            {
                trans_expense_display();
            }
        }


    }


}

