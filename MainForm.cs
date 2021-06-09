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
using System.Runtime.InteropServices;

namespace MoneyTrackerAPP
{
    public partial class MainForm : Form
    {
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




            //Transaction newTrans = new Transaction(name: "不知道", type: "Expense", category: "Food & Dining", account: "Cash",
            //    amount: -100, date: date, place: "NTHU");
            //Transaction newTrans2 = new Transaction(name: "不知道2", type: "Expense", category: "Food & Dining", account: "Cash",
            //    amount: -100, date: date, place: "NTHU");
            //database.insertTransaction(newTrans);
            //database.insertTransaction(newTrans2, true);

            //Transaction transfer = new Transaction(name: "轉帳", type: "Transfer", category: "From", account: "Cash",
            //    amount: -100, date: date, recipient: "Easy Card");
            //Transaction transfer2 = new Transaction(name: "轉帳", type: "Transfer", category: "To", account: "Easy Card",
            //    amount: 100, date: date, recipient: "Cash");
            //database.insertTransaction(transfer);
            //database.insertTransaction(transfer2, true);
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
                    e.Graphics.FillRectangle(new SolidBrush(Color.MediumTurquoise), e.Bounds);
                    break;
                case 3:
                    e.Graphics.FillRectangle(new SolidBrush(Color.Aquamarine), e.Bounds);
                    break;
                default:
                    break;
            }

            Rectangle paddedBounds = new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y, 100, 100);
            paddedBounds.Inflate(-15, -15);
            e.Graphics.DrawImage(main_iconImages.Images[e.Index], paddedBounds);
        }

        private void main_tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }


        #endregion

        
    }
}
