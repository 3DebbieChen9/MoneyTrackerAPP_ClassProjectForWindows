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
       
        public void insertTransaction(Transaction inputValue, string type)
        {
            try
            {
                // Insert INTO Transactions
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        INSERT INTO Transactions (Name, Type, Category, Subcategory, Account, Amount, Date, Place, Comment)
                        VALUES ( 
                                $name, $type, $category, $subcategory, $account, $amount, 
                                DATE($date), $place, $comment
                                )
                    ";
                    // Not NULL
                    command.Parameters.AddWithValue("$name", inputValue.name);
                    command.Parameters.AddWithValue("$type", type);
                    command.Parameters.AddWithValue("$category", inputValue.category);
                    command.Parameters.AddWithValue("$account", inputValue.account);
                    if(type == "Expense") { inputValue.amount = -inputValue.amount; }
                    command.Parameters.AddWithValue("$amount", inputValue.amount);
                    command.Parameters.AddWithValue("$date", inputValue.date.ToString("yyyy-MM-dd"));

                    // NULL able
                    if (inputValue.subcategory != null) { command.Parameters.AddWithValue("$subcategory", inputValue.subcategory); } else { command.Parameters.AddWithValue("$subcategory", DBNull.Value); }
                    if (inputValue.place != null) { command.Parameters.AddWithValue("$place", inputValue.place); } else { command.Parameters.AddWithValue("$place", DBNull.Value); }
                    if (inputValue.comment != null) { command.Parameters.AddWithValue("$comment", inputValue.comment); } else { command.Parameters.AddWithValue("$comment", DBNull.Value); }
                    
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                // UPDATE Accounts Balance
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE Accounts 
                        SET Balance = (SELECT Balance FROM Accounts WHERE Account = $account) + $amount
                        WHERE Account = $account
                    ";
                    // Not NULL
                    command.Parameters.AddWithValue("$account", inputValue.account);
                    if (type == "Expense") { inputValue.amount = -inputValue.amount; }
                    command.Parameters.AddWithValue("$amount", inputValue.amount);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                if(type == "Expense")
                {
                    using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText =
                        @"
                            UPDATE Info 
                            SET Amount = (SELECT Amount FROM Info WHERE Detail = 'Budget') - $amount
                            WHERE Detail = 'Budget'
                         ";
                        // Not NULL
                        command.Parameters.AddWithValue("$amount", inputValue.amount);

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void insertDebtLoan(DebtLoan inputValue)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        INSERT INTO DebtLoan (Recipient, Detail, Type, Account, Amount, Date)
                        VALUES ($recipient, $detail, $type, $account, $amount, Date($date))
                    ";
                    // Not NULL
                    command.Parameters.AddWithValue("$recipient", inputValue.recipient);
                    command.Parameters.AddWithValue("$type", inputValue.type);
                    command.Parameters.AddWithValue("$detail", inputValue.detail);
                    if (inputValue.account != null) { command.Parameters.AddWithValue("$account", inputValue.account); } else { command.Parameters.AddWithValue("$account", DBNull.Value); }
                    if (inputValue.type == "Debt") { command.Parameters.AddWithValue("$amount", -inputValue.amount); }
                    else { command.Parameters.AddWithValue("$amount", inputValue.amount); }
                    command.Parameters.AddWithValue("$date", inputValue.date.ToString("yyyy-MM-dd"));

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                if(inputValue.type == "Loan")
                {
                    using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText =
                        @"
                            UPDATE Accounts 
                            SET Balance = (SELECT Balance FROM Accounts WHERE Account = $account) - $amount
                            WHERE Account = $account
                        ";
                        command.Parameters.AddWithValue("$account", inputValue.account);
                        command.Parameters.AddWithValue("$amount", inputValue.amount);

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                if(inputValue.type == "Debt")
                {
                    using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                    {
                        connection.Open();

                        var command = connection.CreateCommand();

                        command.CommandText =
                        @"
                            UPDATE Info 
                            SET Amount = (SELECT Amount FROM Info WHERE Detail = 'Budget') - $amount
                            WHERE Detail = 'Budget'
                         ";
                        // Not NULL
                        command.Parameters.AddWithValue("$amount", inputValue.amount);

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void insertPlace(string place)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        INSERT INTO Places (Place)
                        VALUES($place)
                    ";

                    command.Parameters.AddWithValue("$place", place);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void insertRecipient(string recipient)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        INSERT INTO Recipients (Recipient)
                        VALUES($recipient)
                    ";

                    command.Parameters.AddWithValue("$recipient", recipient);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void transferMoney(string from, string to, int amount)
        {
            try
            {
                // From
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE Accounts 
                        SET Balance = (SELECT Balance FROM Accounts WHERE Account = $account) - $amount
                        WHERE Account = $account
                    ";
                    command.Parameters.AddWithValue("$account", from);
                    command.Parameters.AddWithValue("$amount", amount);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
                // To
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE Accounts 
                        SET Balance = (SELECT Balance FROM Accounts WHERE Account = $account) + $amount
                        WHERE Account = $account
                    ";
                    command.Parameters.AddWithValue("$account", to);
                    command.Parameters.AddWithValue("$amount", amount);

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


        #region QUERY 

        // 預算
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

        // 對象
        public List<string> get_list_recipient()
        {
            List<string> recipients = new List<string>();

            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Recipient) FROM Recipients
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recipients.Add(reader.GetString(0));
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

            return recipients;
        }

        // 地點/商家
        public List<string> get_list_place()
        {
            List<string> places = new List<string>();

            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Place) FROM Places
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            places.Add(reader.GetString(0));
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

            return places;
        }

        // 種類
        public List<string> get_category(string type)
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
                        SELECT DISTINCT(Category) FROM Categories WHERE Type = $type
                    ";

                    command.Parameters.AddWithValue("$type", type);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(reader.GetString(0));
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

            return categories;
        }

        // 帳戶
        public string[] get_account_name(string accountType)
        {
            List<string> accountNames = new List<string>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Account) FROM Accounts
                        WHERE Type = $type
                    ";
                    command.Parameters.AddWithValue("$type", accountType);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accountNames.Add(reader.GetString(0));
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return accountNames.ToArray();
        }

        
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
                        SELECT Name, Category, Subcategory, Account, Amount, Date, Place, Comment, BankChecked, BankDate
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
                            tmpTrans.category = reader.GetString(1);
                            try { tmpTrans.subcategory = reader.GetString(2); } catch { tmpTrans.subcategory = null; }
                            tmpTrans.account = reader.GetString(3);
                            tmpTrans.amount = reader.GetInt32(4);
                            tmpTrans.date = reader.GetDateTime(5);
                            try { tmpTrans.place = reader.GetString(6); } catch { tmpTrans.place = null; }
                            try { tmpTrans.comment = reader.GetString(7); } catch { tmpTrans.comment = null; }
                            tmpTrans.bankChecked = reader.GetBoolean(8);
                            try { tmpTrans.bankDate = reader.GetDateTime(9); } catch { tmpTrans.bankDate = null; }
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

        public List<Detail> queryDetail()
        {
            List<Detail> detail = new List<Detail>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Name, Type, Category, Subcategory, Account, Amount, Date, Place, Comment, Tag
                            FROM Transactions ORDER BY Date DESC
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Detail tmpTrans = new Detail();
                            tmpTrans.name = reader.GetString(0);
                            tmpTrans.type = reader.GetString(1);
                            tmpTrans.category = reader.GetString(2);
                            try { tmpTrans.subcategory = reader.GetString(3); } catch { tmpTrans.subcategory = null; }
                            tmpTrans.account = reader.GetString(4);
                            tmpTrans.amount = reader.GetInt32(5);
                            tmpTrans.date = reader.GetDateTime(6);
                            try { tmpTrans.place = reader.GetString(7); } catch { tmpTrans.place = null; }
                            try { tmpTrans.comment = reader.GetString(8); } catch { tmpTrans.comment = null; }
                            try { tmpTrans.tag = reader.GetString(9); } catch { tmpTrans.tag = null; }

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
