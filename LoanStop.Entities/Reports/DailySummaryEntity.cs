using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Reports
{
    public class DailySummaryEntity
    {
        public string StoreName {get; set;}
        public decimal Undeposit {get; set;}
        public decimal UndepositCo {get; set;}
        public decimal Payments {get; set;}
        public decimal Receivables {get; set;}
        public decimal Bounced {get; set;}
        public decimal Paid {get; set;}
        public decimal NetFees {get; set;}
        public decimal Cash {get; set;}
        public decimal Checking {get; set;}
        public decimal Transfers {get; set;}
        public decimal Expenses {get; set;}
    }
}
