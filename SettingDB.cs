using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MoneyTrackerAPP
{
    class SettingDB : Database
    {
        private string dbName;
        public SettingDB(string dbName) : base(dbName)
        {
            this.dbName = dbName;
        }

        public string get_budgetAmount()
        {
            string db_budgetAmount = null;
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
                            db_budgetAmount = reader.GetInt32(0).ToString();
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return db_budgetAmount;
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
    }
}
