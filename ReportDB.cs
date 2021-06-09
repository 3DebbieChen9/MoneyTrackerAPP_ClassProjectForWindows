using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTrackerAPP
{
    class ReportDB : Database
    {
        private string dbName;
        public ReportDB(string dbName) : base(dbName)
        {
            this.dbName = dbName;
        }

        // Monthly
        public (string[] expenseCategories, int[] expenseAmount) get_month_exp_cate(string year, string month)
        {
            DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
            DateTime endDate = Convert.ToDateTime(month + "/" + DateTime.DaysInMonth(startDate.Year, startDate.Month).ToString() + "/" + year);

            List<string> expenseCategories = new List<string>();
            List<int> expenseAmount = new List<int>();

            expenseCategories = queryDistictCategorByTypeWithinDate(type: "Expense", startTime: startDate, endTime: endDate);
            foreach (string category in expenseCategories)
            {
                expenseAmount.Add(querySumAmountByCategoryWithinDate(type: "Expense", category: category, startTime: startDate, endTime: endDate));
            }

            return (expenseCategories: expenseCategories.ToArray(), expenseAmount: expenseAmount.ToArray());
        }

        public (string[] expenseCategories,  int[] expenseAmount) get_month_inc_cate(string year, string month)
        {
            DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
            DateTime endDate = Convert.ToDateTime(month + "/" + DateTime.DaysInMonth(startDate.Year, startDate.Month).ToString() + "/" + year);

            List<string> expenseCategories = new List<string>();
            List<int> expenseAmount = new List<int>();

            expenseCategories = queryDistictCategorByTypeWithinDate(type: "Income", startTime: startDate, endTime: endDate);
            foreach (string category in expenseCategories)
            {
                expenseAmount.Add(querySumAmountByCategoryWithinDate(type: "Income", category: category, startTime: startDate, endTime: endDate));
            }

            return (expenseCategories: expenseCategories.ToArray(), expenseAmount: expenseAmount.ToArray());
        }

        public (int totalExpense, int totalIncome) get_month_total_amount(string year, string month)
        {
            DateTime startDate = Convert.ToDateTime(month + "/01/" + year);
            DateTime endDate = Convert.ToDateTime(month + "/" + DateTime.DaysInMonth(startDate.Year, startDate.Month).ToString() + "/" + year);

            int totalExpense = querySumAmountByTypeWithinDate(type: "Expense", startTime: startDate, endTime: endDate);
            int totalIncome = querySumAmountByTypeWithinDate(type: "Income", startTime: startDate, endTime: endDate);

            return (totalExpense: totalExpense, totalIncome: totalIncome);
        }

        // Daily
        public (string[] expenseCategories, int[] expenseAmount) get_day_exp_cate(DateTime date)
        {
            List<string> expenseCategories = new List<string>();
            List<int> expenseAmount = new List<int>();

            DateTime lastDay = Convert.ToDateTime(date.Month.ToString() + "/" + DateTime.DaysInMonth(date.Year, date.Month).ToString() + "/" + date.Year.ToString());
            expenseCategories = queryDistictCategorByTypeWithinDate(type: "Expense", startTime: date, endTime: lastDay);
            foreach (string category in expenseCategories)
            {
                expenseAmount.Add(querySumAmountByCategoryWithinDate(type: "Expense", category: category, startTime: date, endTime: date));
            }

            return (expenseCategories: expenseCategories.ToArray(), expenseAmount: expenseAmount.ToArray());
        }

        public (string[] expenseCategories, int[] expenseAmount) get_day_inc_cate(DateTime date)
        {
            List<string> expenseCategories = new List<string>();
            List<int> expenseAmount = new List<int>();

            DateTime lastDay = Convert.ToDateTime(date.Month.ToString() + "/" + DateTime.DaysInMonth(date.Year, date.Month).ToString() + "/" + date.Year.ToString());
            expenseCategories = queryDistictCategorByTypeWithinDate(type: "Income", startTime: date, endTime: lastDay);
            foreach (string category in expenseCategories)
            {
                expenseAmount.Add(querySumAmountByCategoryWithinDate(type: "Income", category: category, startTime: date, endTime: date));
            }

            return (expenseCategories: expenseCategories.ToArray(), expenseAmount: expenseAmount.ToArray());
        }

        public (int totalExpense, int totalIncome) get_day_total_amount(DateTime date)
        {
            int totalExpense = querySumAmountByTypeWithinDate(type: "Expense", startTime: date, endTime: date);
            int totalIncome = querySumAmountByTypeWithinDate(type: "Income", startTime: date, endTime: date);

            return (totalExpense: totalExpense, totalIncome: totalIncome);
        }

        public List<Transaction> get_day_exp_data(DateTime date)
        {
            return queryDetailByTypeWithinDate("Expense", date, date);
        }

        public List<Transaction> get_day_inc_data(DateTime date)
        {
            return queryDetailByTypeWithinDate("Income", date, date);
        }

        // Duration
        public (int totalExpense, int totalIncome) get_du_total_amount(string startYear, string startMonth, string endYear, string endMonth)
        {
            DateTime startDate = Convert.ToDateTime(startMonth + "/01/" + startYear);
            DateTime endDate = Convert.ToDateTime(endMonth + "/"+DateTime.DaysInMonth(int.Parse(endYear),int.Parse(endMonth)).ToString() + "/"+endYear);
            int totalExpense = querySumAmountByTypeWithinDate(type: "Expense", startTime: startDate, endTime: endDate);
            int totalIncome = querySumAmountByTypeWithinDate(type: "Income", startTime: startDate, endTime: endDate); ;

            return (totalExpense: totalExpense, totalIncome: totalIncome);
        }

        public (int[] monthlyExpenses, int[] monthlyIncomes) get_duration_data(string startYear, string startMonth, string endYear, string endMonth)
        {
            List<int> monthlyExpenses = new List<int>();
            List<int> monthlyIncomes = new List<int>();

            DateTime startDate = Convert.ToDateTime(startMonth + "/01/" + startYear);
            DateTime endDate = Convert.ToDateTime(endMonth + "/01/" + endYear);

            DateTime beginDate = startDate;
            while (beginDate <= endDate)
            {
                DateTime lastDay = new DateTime(beginDate.Year, beginDate.Month, DateTime.DaysInMonth(beginDate.Year, beginDate.Month));
                monthlyExpenses.Add(querySumAmountByTypeWithinDate(type: "Expense", startTime: beginDate, endTime: lastDay));
                monthlyIncomes.Add(querySumAmountByTypeWithinDate(type: "Income", startTime: beginDate, endTime: lastDay));
                // pull out month and year
                beginDate = beginDate.AddMonths(1);
            }
            return (monthlyExpenses: monthlyExpenses.ToArray(), monthlyIncomes: monthlyIncomes.ToArray());
        }
    }
}
