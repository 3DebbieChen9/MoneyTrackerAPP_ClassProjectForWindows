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

namespace MoneyTrackerAPP
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            main_applogo.Image = new Bitmap("icons/applogo_30x30.png");
            string dbName = "../../accounting_DB_v1.db";
            Database database = new Database(dbName);
            ReportDB reportDB = new ReportDB(dbName);
            DateTime date = Convert.ToDateTime("6" + "/04/" + "2021");
            //List<Transaction> result = reportDB.queryDetailByTypeWithinDate("Expense", date, date);
            //foreach (Transaction t in result)
            //{
            //    label1.Text = t.name;
            //    Console.WriteLine(t.name);
            //}

            Transaction newTrans = new Transaction(name: "不知道", type: "Expense", category: "Food & Dining", account: "Cash",
                amount: -100, date: date, place: "NTHU");
            Transaction newTrans2 = new Transaction(name: "不知道2", type: "Expense", category: "Food & Dining", account: "Cash",
                amount: -100, date: date, place: "NTHU");
            database.insertTransaction(newTrans);
            database.insertTransaction(newTrans2, true);

            Transaction transfer = new Transaction(name: "轉帳", type: "Transfer", category: "From", account: "Cash",
                amount: -100, date: date, recipient: "Easy Card");
            Transaction transfer2 = new Transaction(name: "轉帳", type: "Transfer", category: "To", account: "Easy Card",
                amount: 100, date: date, recipient: "Cash");
            database.insertTransaction(transfer);
            database.insertTransaction(transfer2, true);
        }

        private void main_closeApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
