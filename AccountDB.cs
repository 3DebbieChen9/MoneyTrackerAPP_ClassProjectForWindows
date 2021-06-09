using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MoneyTrackerAPP
{
    class Account
    {
        public string account;
        public string type;
        public int balance;

        public Account() { }
        public Account(string account, string type = null, int balance = 0)
        {
            this.account = account;
            this.type = type;
            this.balance = balance;
        }
    }

    class AccountDetail
    {
        public string name;
        public string amount;
        public DateTime date;

        public AccountDetail() { }
        public AccountDetail(string name, string amount, DateTime date)
        {
            this.name = name;
            this.amount = amount;
            this.date = date;
        }
    }

    class CreditCardInfo
    {
        public int id;
        public string name;
        public string amount;
        public DateTime date;
        public bool bankChecked;
        public DateTime? bankDate;

        public CreditCardInfo() { }

        public CreditCardInfo(int id, string name, string amount, DateTime date,
            bool bankChecked = false, DateTime? bankDate = null)
        {
            this.id = id;
            this.name = name;
            this.amount = amount;
            this.date = date;
            this.bankChecked = bankChecked;
            this.bankDate = bankDate;
        }
    }

    class AccountDB : Database
    {
        private string dbName;
        public AccountDB(string dbName) : base(dbName)
        {
            this.dbName = dbName;
        }

        public string[] get_account_type()
        {
            List<string> accountTypes = new List<string>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT DISTINCT(Type) FROM Accounts
                    ";

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accountTypes.Add(reader.GetString(0));
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return accountTypes.ToArray();
        }

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

        public void insert_account_name(string accountType, string accountName, string balance)
        {
            Account inputAccount = new Account(account: accountName, type: accountType, balance: int.Parse(balance));
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        INSERT INTO Accounts (Account, Type, Balance)
                        VALUES ($account, $type, $balance)
                    ";

                    // Not NULL
                    command.Parameters.AddWithValue("$account", inputAccount.account);
                    command.Parameters.AddWithValue("$type", inputAccount.type);
                    command.Parameters.AddWithValue("$balance", inputAccount.balance);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void delete_account_name(string accountType, string accountName)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        DELETE FROM Accounts 
                        WHERE Account = $accountName AND Type = $accountType
                    ";

                    // Not NULL
                    command.Parameters.AddWithValue("$accountType", accountType);
                    command.Parameters.AddWithValue("$accountName", accountName);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public AccountDetail[] get_account_detail(string accountName)
        {
            List<AccountDetail> accountDetails = new List<AccountDetail>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Name, Amount, Date FROM Transactions
                        WHERE Account = $accountName
                    ";
                    command.Parameters.AddWithValue("$accountName", accountName);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AccountDetail tmp = new AccountDetail();
                            tmp.name = reader.GetString(0);
                            tmp.amount = reader.GetInt32(1).ToString();
                            tmp.date = reader.GetDateTime(2);
                            accountDetails.Add(tmp);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return accountDetails.ToArray();
        }

        public CreditCardInfo[] get_unpaid_transaction(string accountName)
        {
            List<CreditCardInfo> accountDetails = new List<CreditCardInfo>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT id, Name, Amount, Date FROM Transactions
                        WHERE BankChecked != TRUE AND Account = $accountName
                    ";
                    command.Parameters.AddWithValue("$accountName", accountName);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CreditCardInfo tmp = new CreditCardInfo();
                            tmp.id = reader.GetInt32(0);
                            tmp.name = reader.GetString(1);
                            tmp.amount = reader.GetInt32(2).ToString();
                            tmp.date = reader.GetDateTime(3);
                            tmp.bankChecked = false;
                            accountDetails.Add(tmp);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return accountDetails.ToArray();
        }

        public void updateBankChecked(int id, DateTime bankDate)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE Transactions
                        SET BankChecked = TRUE, BankDate = DateTime($bankDate)
                        WHERE id = $id
                    ";

                    // Not NULL
                    command.Parameters.AddWithValue("$bankDate", bankDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("$id", id);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void pay_CreditCard(List<int> ids, List<DateTime> bankDates)
        {
            for(int i = 0; i < ids.Count; i++)
            {
                updateBankChecked(ids[i], bankDates[i]);
            }
        }
    }
}
