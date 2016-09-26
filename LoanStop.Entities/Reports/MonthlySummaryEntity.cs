using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Reports
{
    public class MonthlySummaryEntity
    {
        public string Name { get; set;}
        public int NewAccounts { get; set;}
        public int NumberOfLoans { get; set;}
        public decimal AmountDisbursed { get; set;}
        public decimal AmountReceived { get; set;}
        public decimal AmountBounced { get; set;}
        public decimal AmountPaid { get; set;}
        public decimal NetFees { get; set;}
        public decimal CheckCashReceived { get; set;}
        public decimal CheckCashDisbursed { get; set;}
        public decimal CheckCashBounced { get; set;}
        public decimal CheckCashPaid { get; set;}
        public decimal CheckCashNetFees { get; set;}
        public decimal DebitCardReceived { get; set;}
        public decimal DebitCardDisbursed { get; set;}
        public decimal DebitCardNet { get; set;}
        public decimal TotalNet { get; set;}
        public decimal NSFRate { get; set;}
        public decimal CheckCashNSFRate { get; set;}
    
    }
}
