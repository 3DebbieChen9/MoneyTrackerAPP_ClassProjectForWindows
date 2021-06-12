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
    class SettingVar
    {
        public DateTime startDate = new DateTime();
        public DateTime endDate = new DateTime();
        public List<string> category_inc = new List<string>();
        public List<string> category_exp = new List<string>();
        public List<string> list_place = new List<string>();
        public List<string> list_recipient = new List<string>();
    }

    class SettingFunction
    {   
        public static DateTime get_date(string cycleDay, bool startOrEnd)
        {
            string nowDate = null;
            DateTime validationDate = new DateTime();

            if (startOrEnd)     //  startDate
                nowDate = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
            else                //  endDate
                nowDate = DateTime.Now.AddMonths(1).ToString("yyyyMM");

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
            
            if(residualBudget <= 0)
            {
                return Color.Red;
            }
            else if(residualBudget <= 0.3 * AmountBudjet)
            {
                return Color.Orange;
            }
            else
            {
                return Color.Green;
            }
        }

        public static void notification()
        {
            new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("Andrew sent you a picture")
                    .AddText("Check this out, The Enchantments in Washington!")
                    .Show();
        }
    }

    
}
