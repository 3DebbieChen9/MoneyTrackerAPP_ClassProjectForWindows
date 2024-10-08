﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrackerAPP
{
    class Detail
    {
        public string name;
        public string type;
        public string category;
        public string subcategory;
        public string account;
        public int amount;
        public DateTime date;
        public string place;
        public string comment;
        public Detail() { }
        public Detail(string name, string type, string category, string account, int amount, DateTime date,
            string subcategory = null, string place = null, string comment = null)
        {
            this.name = name;
            this.type = type;
            this.category = category;
            this.subcategory = subcategory;
            this.account = account;
            this.amount = amount;
            this.date = date;
            this.place = place;
            this.comment = comment;
        }
    }

    class DebtLoan
    {
        public int id;
        public string recipient;
        public string detail;
        public string type;
        public string account;
        public int amount;
        public DateTime date;

        public DebtLoan()
        {
            this.id = 0;
            this.account = null;
        }
        public DebtLoan(string detail, string type, int amount, DateTime date, string recipient, string account = null, int id = 0)
        {
            this.recipient = recipient;
            this.detail = detail;
            this.type = type;
            this.account = account;
            this.amount = amount;
            this.date = date;
            this.id = id;
        }
    }

    class Transaction
    {
        public string name;
        public string category;
        public string account;
        public int amount;
        public DateTime date;

        public string place;
        public string comment;

        public string subcategory;
        public bool bankChecked;
        public DateTime? bankDate;

        public Transaction()
        {
            this.place = null;
            this.comment = null;

            this.subcategory = null;
            this.bankChecked = false;
            this.bankDate = null;
        }
        public Transaction(string name, string category, string account, int amount, DateTime date,
            string subcategory = null, string place = null, string comment = null)
        {
            this.name = name;
            this.category = category;
            this.subcategory = subcategory;
            this.account = account;
            this.amount = amount;
            this.date = date;
            this.place = place;
            this.comment = comment;
            this.bankChecked = false;
            this.bankDate = null;
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
