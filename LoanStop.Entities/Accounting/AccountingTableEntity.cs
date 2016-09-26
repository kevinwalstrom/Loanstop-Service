using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Accounting
{
    public class AccountingTableEntity
    {
        public long Id;
        public string TransactionType;
        public DateTime TransDate;
        public long? CheckNumber;
        public decimal? Debit;
        public decimal? Credit;
        public string PayableTo;
        public string Description;
        public string Employee;
        public decimal? Balance;

    }
}
