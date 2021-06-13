using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.Uwp.Notifications;


namespace MoneyTrackerAPP
{
    //class SettingNotification
    //{
    //    public string notiTitle = null;
    //    public string notiContent = null;
    //    public DateTime notiDate = new DateTime();
    //}

    class SettingVar
    {
        public DateTime startDate = new DateTime();
        public DateTime endDate = new DateTime();


        public int category_incClickItem = -1;
        public int category_expClickItem = -1;
        public int list_placeClickItem = -1;
        public int list_recipientClickItem = -1;
        public int notificationClickItem = -1;
        public List<string> category_inc = new List<string>();
        public List<string> category_exp = new List<string>();
        public List<string> list_place = new List<string>();
        public List<string> list_recipient = new List<string>();
        public List<SettingNotification> notification_infos = new List<SettingNotification>();

        public string notificationOriText = null;
    }

    class SettingFunction
    {
        public static DateTime get_date(string cycleDay, bool startOrEnd)
        {
            string nowDate = null;
            DateTime validationDate = new DateTime();

            if ((DateTime.Now.Day < int.Parse(cycleDay)) && (int.Parse(cycleDay) <= DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day).Day))
            {
                if (startOrEnd)     //  startDate
                    nowDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
                else                //  endDate
                    nowDate = DateTime.Now.ToString("yyyyMM");
            }
            else
            {
                if (startOrEnd)     //  startDate
                    nowDate = DateTime.Now.ToString("yyyyMM");
                else                //  endDate
                    nowDate = DateTime.Now.AddMonths(1).ToString("yyyyMM");
            }
            while (!DateTime.TryParse(nowDate.Substring(0, 4) + "/" + nowDate.Substring(4, 2) + "/" + cycleDay, out validationDate))
            {
                cycleDay = (int.Parse(cycleDay) - 1).ToString();
            }
            return validationDate;
        }

        public static void update_List2ListBox(ListBox listBox, List<string> list)
        {
            listBox.Items.Clear();
            foreach (string str in list)
                listBox.Items.Add(str);
        }

        public static Color budgetColor(int residualBudget, int AmountBudjet)
        {

            if (residualBudget <= 0)
            {
                return Color.Red;
            }
            else if (residualBudget <= 0.3 * AmountBudjet)
            {
                return Color.Orange;
            }
            else
            {
                return Color.Green;
            }
        }

        public static void update_NotificationListBox(ListBox listBox, List<SettingNotification> notification_infos)
        {
            listBox.Items.Clear();
            foreach (SettingNotification notification_info in notification_infos)
                listBox.Items.Add(notification_info.notiTitle);
        }

        public static void notification(List<SettingNotification> notification_infos, SettingDB settingDB)
        {
            foreach (SettingNotification notification_info in notification_infos)
            {
                if (DateTime.Now.Date.CompareTo(notification_info.notiDate.Date) == 0)
                {
                    new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText(notification_info.notiTitle)
                    .AddText(notification_info.notiContent)
                    .Show();

                    notification_info.notiDate = notification_info.notiDate.AddMonths(1);
                    settingDB.set_notification(notification_infos);
                }
            }
        }
    }


}
