using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MoneyTrackerAPP
{
    class Budget
    {
        public string budgetAmount;
        public string cycleDay;
        
        public Budget() { }
        public Budget(string budgetAmount, string cycleDay)
        {
            this.budgetAmount = budgetAmount;
            this.cycleDay = cycleDay;
        }
    }
    class SettingDB : Database
    {
        private string dbName;
        public SettingDB(string dbName) : base(dbName)
        {
            this.dbName = dbName;
        }
        
        public Budget get_budget_info()
        {
            Budget budgetInfo = new Budget();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Amount FROM Info WHERE Detail = 'Budget'
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            budgetInfo.budgetAmount = reader.GetInt32(0).ToString();
                        }
                    }
                    connection.Close();
                }

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
                            budgetInfo.cycleDay = reader.GetInt32(0).ToString();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return budgetInfo;
        }

        public void set_budget_info(string budgetAmount, string cycleDay)
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
                    //Console.WriteLine("Sum = {0}", sum);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return expenseSum;
        }


    }
}
