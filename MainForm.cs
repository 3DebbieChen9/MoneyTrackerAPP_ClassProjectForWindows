using Microsoft.Data.Sqlite;
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

namespace MoneyTrackerAPP
{
    public partial class MainForm : Form
    {
        static string dbName = "../../moneyRecord.db";
        static Database db = new Database(dbName);
        static AccountDB accountDB = new AccountDB(dbName);
        static ReportDB reportDB = new ReportDB(dbName);
        static SettingDB settingDB = new SettingDB(dbName);
        AccountGlobal account = new AccountGlobal();
        Report report = new Report();

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
                    e.Graphics.FillRectangle(new SolidBrush(Color.PaleTurquoise), e.Bounds);
                    break;
                case 1:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Turquoise), e.Bounds);
                    break;
                case 2:
                    e.Graphics.FillRectangle(new SolidBrush(Color.PaleTurquoise), e.Bounds);
                    break;
                case 3:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Aquamarine), e.Bounds);
                    break;
                case 4:
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightSkyBlue), e.Bounds);
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
            Transaction trans = new Transaction(name: " ", category: " ", account: " ", amount: 10, date: DateTime.Now, place: "", comment: "");
            db.insertTransaction(trans, "Expense");
            //db.insertTransaction(trans, "Income");
            //string remainBudget = db.get_budgetAmount();

        }
        private void main_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Account
            if(main_tabControl.SelectedIndex == 1)
            {
                accounts_type.Text = "";
                accounts_name.Text = "";
                accounts_total_balance.Text = "0";
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
                report.pie_inc_series.Label = "#PERCENT{P}";
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
                        String.Format("{0,-12}", ele.account) + String.Format("{0,-10}", ele.date.ToString("yyyy-MM-dd")) + String.Format("{0,-10}", ele.place) + String.Format("{0,-20}", ele.comment) + Environment.NewLine;
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
            string[] account_name = accountDB.get_account_name(accounts_type.Text);
            foreach (string ele in account_name)
            {
                accounts_name.Items.Add(ele);
            }
            if (accounts_type.Text == "Credit Card")
            {
                refreshCreditCardInfo();
            }
            if (accounts_type.Text != "Credit Card")
            {
                accounts_panel_creditcard.Enabled = false;
                accounts_panel_creditcard.Controls.Clear();
                main_accounts.Controls.Remove(accounts_panel_creditcard);
                accounts_panel_detail.Location = new Point(36, 159);
                accounts_panel_detail.Size = new Size(829, 376);
            }
        }

        private void accounts_create_Click_1(object sender, EventArgs e)
        {
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
        }

        private void accounts_delete_Click(object sender, EventArgs e)
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
                List<CreditCardInfo> accounts_check = new List<CreditCardInfo>();
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
                    try { accounts_paid.Name = accounts_info[i].id.ToString(); }
                    catch { accounts_paid.Name = "0"; }

                    accounts_paid.Location = new Point(account.x4, account.y);
                    accounts_panel_creditcard.Controls.Add(accounts_paid);
                    account.accounts_checkbox.Add(accounts_paid);
                    accounts_item.Text = accounts_info[i].name;
                    accounts_amount.Text = accounts_info[i].amount;
                    accounts_panel_creditcard.Controls.Add(accounts_item);
                    accounts_panel_creditcard.Controls.Add(accounts_amount);
                    accounts_panel_creditcard.Show();
                    account.y += 50;
                    account.accounts_id.Add(accounts_info[i].id);
                    account.accounts_bankDates.Add(accounts_info[i].date);
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
            refreshCreditCardInfo();
            accounts_total_balance.Text = accountDB.queryAccountBalance(accountType: accounts_type.Text, accountName: accounts_name.Text);
        }

        private void accounts_ok_Click(object sender, EventArgs e)
        {
            foreach (CheckBox ele in account.accounts_checkbox)
            {
                if (ele.Checked)
                {
                    accountDB.updateBankChecked(int.Parse(ele.Name), DateTime.Now);
                }
            }
            refreshCreditCardInfo();
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

        private void trans_cbo_category_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trans_txtbox_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void trans_txtbox_amount_TextChanged(object sender, EventArgs e)
        {

        }

        private void trans_btn_save_Click(object sender, EventArgs e)
        {

        }
    }
}
