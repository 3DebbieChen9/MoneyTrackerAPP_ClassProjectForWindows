using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrackerAPP
{
    class Database
    {
        private string dbName;
        public Database(string _dbName)
        {
            this.dbName = _dbName;
        }

        #region INSERT
        public void insertTransaction(Transaction inputValue, bool subTrans = false)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    if (!subTrans)
                    {
                        command.CommandText =
                        @"
                            INSERT INTO Transactions (GroupID, Name, Type, Category, Subcategory, Account, Amount, Date, Place, Recipient, Comment, Tag, BankChecked, BankDate)
                            VALUES (
                                    (SELECT MAX(GroupID) FROM Transactions)+1, 
                                    $name, $type, $category, $subcategory, $account, $amount, 
                                    DATE($date), $place, $recipient, $comment, $tag, $bankChecked, DateTime($bankDate)
                                   )
                        ";
                    }
                    else
                    {
                        command.CommandText =
                        @"
                            INSERT INTO Transactions (GroupID, Name, Type, Category, Subcategory, Account, Amount, Date, Place, Recipient, Comment, Tag, BankChecked, BankDate)
                            VALUES (
                                    (SELECT MAX(GroupID) FROM Transactions), 
                                    $name, $type, $category, $subcategory, $account, $amount, 
                                    DATE($date), $place, $recipient, $comment, $tag, $bankChecked, DateTime($bankDate)
                                   )
                        ";
                    }

                    // Not NULL
                    command.Parameters.AddWithValue("$name", inputValue.name);
                    command.Parameters.AddWithValue("$type", inputValue.type);
                    command.Parameters.AddWithValue("$category", inputValue.category);
                    command.Parameters.AddWithValue("$account", inputValue.account);
                    command.Parameters.AddWithValue("$amount", inputValue.amount);
                    command.Parameters.AddWithValue("$date", inputValue.date.ToString("yyyy-MM-dd"));

                    // NULL able
                    if (inputValue.subcategory != null) { command.Parameters.AddWithValue("$subcategory", inputValue.subcategory); } else { command.Parameters.AddWithValue("$subcategory", DBNull.Value); }
                    if (inputValue.place != null) { command.Parameters.AddWithValue("$place", inputValue.place); } else { command.Parameters.AddWithValue("$place", DBNull.Value); }
                    if (inputValue.recipient != null) { command.Parameters.AddWithValue("$recipient", inputValue.recipient); } else { command.Parameters.AddWithValue("$recipient", DBNull.Value); }
                    if (inputValue.comment != null) { command.Parameters.AddWithValue("$comment", inputValue.comment); } else { command.Parameters.AddWithValue("$comment", DBNull.Value); }
                    if (inputValue.tag != null) { command.Parameters.AddWithValue("$tag", inputValue.tag); } else { command.Parameters.AddWithValue("$tag", DBNull.Value); }
                    command.Parameters.AddWithValue("$bankChecked", inputValue.bankChecked);
                    if (inputValue.bankDate != null) { command.Parameters.AddWithValue("$bankDate", inputValue.bankDate?.ToString("yyyy-MM-dd HH:mm:ss")); } else { command.Parameters.AddWithValue("$bankDate", DBNull.Value); }

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }



        #endregion

        #region UPDATE
        #endregion

        #region QUERY 
        public List<string> queryDistictCategorByTypeWithinDate(string type, DateTime startTime, DateTime endTime)
        {
            List<string> categories = new List<string>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Category) FROM Transactions
                            WHERE (Type = $type) AND (Date BETWEEN Date($startTime) AND Date($endTime))
                    ";
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$startTime", startTime.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$endTime", endTime.ToString("yyyy-MM-dd"));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader.GetString(0);
                            categories.Add(category);
                            //Console.WriteLine(category);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return categories;
        }

        public List<string> queryDistinctCategoryByType(string type)
        {
            List<string> categories = new List<string>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Category) FROM Transactions
                            WHERE Type = $type
                    ";
                    command.Parameters.AddWithValue("$type", type);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string category = reader.GetString(0);
                            categories.Add(category);
                            //Console.WriteLine(category);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return categories;
        }

        public int querySumAmountByCategoryWithinDate(string type, string category, DateTime startTime, DateTime endTime)
        {
            int sum = 0;
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT SUM(Amount) FROM Transactions
                            WHERE (Type = $type AND Category = $category)
                                AND (Date BETWEEN Date($startTime) AND Date($endTime))
                    ";
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$category", category);
                    command.Parameters.AddWithValue("$startTime", startTime.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$endTime", endTime.ToString("yyyy-MM-dd"));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try { sum = reader.GetInt32(0); }
                            catch { sum = 0; }
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

            return sum;
        }

        public int querySumAmountByTypeWithinDate(string type, DateTime startTime, DateTime endTime)
        {
            int sum = 0;
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT SUM(Amount) FROM Transactions
                            WHERE (Type = $type) AND (Date BETWEEN Date($startTime) AND Date($endTime))
                    ";
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$startTime", startTime.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$endTime", endTime.ToString("yyyy-MM-dd"));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try { sum = reader.GetInt32(0); }
                            catch { sum = 0; }
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

            return sum;
        }

        public List<Transaction> queryDetailByTypeWithinDate(string type, DateTime startTime, DateTime endTime)
        {
            List<Transaction> detail = new List<Transaction>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Name, Type, Category, Subcategory, Account, Amount, Date, Place, Recipient, Comment, Tag, BankChecked, BankDate
                            FROM Transactions
                            WHERE (Type = $type) AND (Date BETWEEN Date($startTime) AND Date($endTime))
                    ";

                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$startTime", startTime.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("$endTime", endTime.ToString("yyyy-MM-dd"));

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction tmpTrans = new Transaction();
                            tmpTrans.name = reader.GetString(0);
                            tmpTrans.type = reader.GetString(1);
                            tmpTrans.category = reader.GetString(2);
                            try { tmpTrans.subcategory = reader.GetString(3); } catch { tmpTrans.subcategory = null; }
                            tmpTrans.account = reader.GetString(4);
                            tmpTrans.amount = reader.GetInt32(5);
                            tmpTrans.date = reader.GetDateTime(6);
                            try { tmpTrans.place = reader.GetString(7); } catch { tmpTrans.place = null; }
                            try { tmpTrans.recipient = reader.GetString(8); } catch { tmpTrans.recipient = null; }
                            try { tmpTrans.comment = reader.GetString(9); } catch { tmpTrans.comment = null; }
                            try { tmpTrans.tag = reader.GetString(10); } catch { tmpTrans.tag = null; }
                            tmpTrans.bankChecked = reader.GetBoolean(11);
                            try { tmpTrans.bankDate = reader.GetDateTime(12); } catch { tmpTrans.bankDate = null; }
                            detail.Add(tmpTrans);
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

            return detail;
        }

        #endregion

    }
}
