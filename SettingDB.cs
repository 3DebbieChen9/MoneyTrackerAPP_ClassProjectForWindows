using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MoneyTrackerAPP
{
    class SettingNotification
    {
        public string notiTitle;
        public string notiContent;
        public DateTime notiDate;
        
        public SettingNotification()
        {
            this.notiTitle = null;
            this.notiContent = null;
            this.notiDate = new DateTime();
        }
    }

    class SettingDB : Database
    {
        private string dbName;
        public SettingDB(string dbName) : base(dbName)
        {
            this.dbName = dbName;
        }

        public string get_budget_cycleDay()
        {
            string db_cycleDay = null;
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Amount FROM Info WHERE Detail = 'Budget Cycle'
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            db_cycleDay = reader.GetInt32(0).ToString();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return db_cycleDay;
        }
        public void set_bugetAmount(string budgetAmount)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        UPDATE Info
                        SET Amount = $budgetAmount
                        WHERE Detail = 'Budget'
                    ";
                    command.Parameters.AddWithValue("$budgetAmount", int.Parse(budgetAmount));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void set_budget_cycleDay(string cycleDay)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        UPDATE Info
                        SET Amount = $budgetCycle
                        WHERE Detail = 'Budget Cycle'
                    ";

                    command.Parameters.AddWithValue("$budgetCycle", int.Parse(cycleDay));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public int get_sum_period(DateTime start, DateTime end)
        {
            int expenseSum = 0;
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT SUM(Amount) FROM Transactions
                            WHERE (Type = 'Expense') AND (Date BETWEEN Date($startTime) AND Date($endTime))
                    ";

                    command.Parameters.AddWithValue("$startTime", start.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$endTime", end.ToString("yyyy-MM-dd"));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try { expenseSum = reader.GetInt32(0); }
                            catch { expenseSum = 0; }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return expenseSum;
        }


        void deleteAllCategory(string type)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        DELETE FROM Categories WHERE Type = $type
                    ";

                    command.Parameters.AddWithValue("$type", type);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        void insertCategory(string type, string category, string subcategory = null)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        INSERT INTO Categories (Type, Category, Subcategory)
                        VALUES($type, $category, $subcategory)
                    ";

                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$category", category);
                    if (subcategory != null) { command.Parameters.AddWithValue("$subcategory", subcategory); } else { command.Parameters.AddWithValue("$subcategory", DBNull.Value); }

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void set_category(string type, List<string> newCategories)
        {
            deleteAllCategory(type);
            foreach (string category in newCategories)
            {
                insertCategory(type, category);
            }
        }


        public void deleteAllPlaces()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        DELETE FROM Places
                    ";
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void deleteAllRecipients()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        DELETE FROM Recipients
                    ";
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void set_list(string target, List<string> newList)
        {
            switch (target)
            {
                case "Place":
                    deleteAllPlaces();
                    foreach (string place in newList)
                    {
                        insertPlace(place);
                    }
                    break;
                case "Recipient":
                    deleteAllRecipients();
                    foreach (string recipient in newList)
                    {
                        insertRecipient(recipient);
                    }
                    break;
                default:
                    break;
            }
        }

        void deleteAllNoti()
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        DELETE FROM Notifications
                    ";
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        void insertNotification(SettingNotification noti)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        INSERT INTO Notifications (notiTitle, notiContent, notiDate)
                        VALUES ($title, $content, DateTime($date))
                    ";
                    if (noti.notiTitle != null) { command.Parameters.AddWithValue("$title", noti.notiTitle); }
                    else { command.Parameters.AddWithValue("$title", DBNull.Value); }
                    if (noti.notiContent != null) { command.Parameters.AddWithValue("$content", noti.notiContent); }
                    else { command.Parameters.AddWithValue("$content", DBNull.Value); }
                    command.Parameters.AddWithValue("$date", noti.notiDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void set_notification(List<SettingNotification> notifications)
        {
            deleteAllNoti();
            foreach(SettingNotification notification in notifications)
            {
                insertNotification(notification);
            }
        }
        public List<SettingNotification> get_notification()
        {
            List<SettingNotification> notifications = new List<SettingNotification>();

            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT notiTitle, notiContent, notiDate FROM Notifications
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SettingNotification tmp = new SettingNotification();
                            tmp.notiTitle = reader.GetString(0);
                            tmp.notiContent = reader.GetString(1);
                            tmp.notiDate = reader.GetDateTime(2);
                            notifications.Add(tmp);
                        }
                    }
                    //Console.WriteLine("Sum = {0}", sum);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return notifications;
        }
    }
}
