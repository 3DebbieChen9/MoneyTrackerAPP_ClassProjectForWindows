using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrackerAPP
{
    class Transaction
    {
        public string name;
        public string type;
        public string category;
        public string subcategory;
        public string account;
        public int amount;
        public DateTime date;
        public string place;
        public string recipient;
        public string comment;
        public string tag;
        public bool? bankChecked;
        public DateTime? bankDate;

        public Transaction() { }
        public Transaction(string name, string type, string category, string account, int amount, DateTime date,
            string subcategory = null, string place = null, string recipient = null, string comment = null, string tag = null,
            bool? bankChecked = null, DateTime? bankDate = null)
        {
            this.name = name;
            this.type = type;
            this.category = category;
            this.subcategory = subcategory;
            this.account = account;
            this.amount = amount;
            this.date = date;
            this.place = place;
            this.recipient = recipient;
            this.comment = comment;
            this.tag = tag;
            this.bankChecked = bankChecked;
            this.bankDate = bankDate;
        }
    }

    class Account
    {
        public string account;
        public string type;
        public int? balance;

        public Account() { }
        public Account(string account, string type = null, int? balance = null)
        {
            this.account = account;
            this.type = type;
            this.balance = balance;
        }
    }

    class Category
    {
        public string type;
        public string category;
        public string subcategory;

        public Category() { }
        public Category(string type, string category, string subcategory = null)
        {
            this.type = type;
            this.category = category;
            this.subcategory = subcategory;
        }
    }
}
