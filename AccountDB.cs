using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace MoneyTrackerAPP
{
    class AccountGlobal
    {
        public int x1;
        public int x2;
        public int x3;
        public int x4;
        public int y;
        public List<CheckBox> accounts_checkbox;
        public List<int> accounts_id;
        public List<DateTime> accounts_bankDates;

        public AccountGlobal()
        {
            this.x1 = 100;
            this.x2 = 200;
            this.x3 = 300;
            this.x4 = 550;
            this.y = 100;
            this.accounts_checkbox = new List<CheckBox>();
            this.accounts_id = new List<int>();
            this.accounts_bankDates = new List<DateTime>();
        }
    }

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
            string accountName = "";
            int amount = 0;
            try
            {
                // Query Account, Amount By id
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        SELECT Account, Amount FROM Transactions WHERE id = $id;
                    ";

                    // Not NULL
                    command.Parameters.AddWithValue("$id", id);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accountName = reader.GetString(0);
                            try { amount = reader.GetInt32(1); } catch { amount = 0; }
                        }
                    }
                    connection.Close();
                }
                // UPDATE bankchecked, bankdate By id
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
                // UPDATE balance
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        UPDATE Accounts
                        SET Balance = (SELECT Balance FROM Accounts WHERE Account = $accountName) - $amount
                        WHERE Account = $accountName
                    ";

                    // Not NULL
                    command.Parameters.AddWithValue("$accountName", accountName);
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

        public void pay_CreditCard(List<int> ids, List<DateTime> bankDates)
        {
            for(int i = 0; i < ids.Count; i++)
            {
                updateBankChecked(ids[i], bankDates[i]);
            }
        }

        public string queryAccountBalance(string accountType, string accountName)
        {
            string balance = "0";
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT Balance FROM Accounts
                        WHERE Account = $accountName AND Type = $accountType
                    ";
                    command.Parameters.AddWithValue("$accountName", accountName);
                    command.Parameters.AddWithValue("$accountType", accountType);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try { balance = reader.GetInt32(0).ToString(); } catch { balance = "0"; }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return balance;
        }

        public string get_debtLoan_balance(string who)
        {
            string balance = "0";
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT SUM(Amount) FROM DebtLoan
                        WHERE Recipient = $who
                    ";
                    command.Parameters.AddWithValue("$who", who);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try { balance = reader.GetInt32(0).ToString(); } catch { balance = "0"; }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return balance;
        }

        public DebtLoan[] get_debtLoans_detail(string who)
        {
            List<DebtLoan> lists = new List<DebtLoan>();
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT id, Detail, Type, Amount, Account, Date FROM DebtLoan
                        WHERE Recipient = $who
                    ";
                    command.Parameters.AddWithValue("$who", who);

                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DebtLoan tmp = new DebtLoan();
                            tmp.id = reader.GetInt32(0);
                            tmp.detail = reader.GetString(1);
                            tmp.type = reader.GetString(2);
                            tmp.amount = reader.GetInt32(3);
                            tmp.account = reader.GetString(4);
                            tmp.date = reader.GetDateTime(5);
                            lists.Add(tmp);
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return lists.ToArray();
        }

        public void pay_DebtLoan(int id, string account, int amount)
        {
            try
            {
                using (var connection = new SqliteConnection("Data Source=" + this.dbName))
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText =
                    @"
                        DELETE FROM DebtLoan WHERE id = $id
                    ";
                    // Not NULL
                    command.Parameters.AddWithValue("$id", id);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
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
                    command.Parameters.AddWithValue("$account", account);
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
    }
}
