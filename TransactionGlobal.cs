using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyTrackerAPP
{
    class TransactionGlobal
    {
        public TextBox trans_txtbox_name;
        public TextBox trans_txtbox_amount;
        public ComboBox trans_cbo_category;
        public ComboBox trans_cbo_account;
        public DateTimePicker trans_date;
        public TextBox trans_txtbox_store;
        public TextBox trans_txtbox_note;

        public TextBox transfer_txtbox_amount;
        public ComboBox transfer_cbo_from;
        public ComboBox transfer_cbo_to;

        public void clearTransInput()
        {
            trans_txtbox_name = null;
            trans_txtbox_amount = null;
            trans_cbo_category = null;
            trans_cbo_account = null;
            trans_date = null;
            trans_txtbox_store = null;
            trans_txtbox_note = null;
        }

        public void clearTransferInput()
        {
            transfer_txtbox_amount = null;
            transfer_cbo_from = null;
            transfer_cbo_to = null;
        }


        //public Transaction transactionToInput;
        //public DebtLoan debtLoanInput;

        //public TransactionGlobal()
        //{
        //    this.transactionToInput = new Transaction();
        //    this.debtLoanInput = new DebtLoan();
        //}

        //public void clear_transInput()
        //{
        //    this.transactionToInput.name = null;
        //    this.transactionToInput.category = null;
        //    this.transactionToInput.subcategory = null;
        //    this.transactionToInput.account = null;
        //    this.transactionToInput.amount = 0;
        //    this.transactionToInput.date = DateTime.MinValue;
        //    this.transactionToInput.place = null;
        //    this.transactionToInput.comment = null;
        //    this.transactionToInput.bankChecked = false;
        //    this.transactionToInput.bankDate = null;
        //}

        //public void clear_debtLoanInput()
        //{
        //    this.debtLoanInput.recipient = null;
        //    this.debtLoanInput.detail = null;
        //    this.debtLoanInput.type = null;
        //    this.debtLoanInput.account = null;
        //    this.debtLoanInput.amount = 0;
        //    this.debtLoanInput.date = DateTime.MinValue;
        //    this.debtLoanInput.id = 0;
        //}
    }
}
