using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    public class PaymentModel
    {
        //public ClientEntity Client {get; set;}
        //public TransactionEntity Transaction {get; set;}
        //public PaymentPlanCheckEntity Ppc {get; set;}

        public string SsNumber {get; set;} 
        public long TransactionId {get; set;} 
        public long PpcId {get; set;} 

        public decimal PayoffAmount {get; set;} //amount to payoff loan
        public decimal PaymentAmount {get; set;} //contracted amount to be paid
        public decimal AmountPaid {get; set;} // actual amoutn paid
        public decimal MonthlyFee {get; set;} // fee amount for the month
        public decimal NSFFee {get; set;} // NSF fee - either 25 or 0

        public decimal PartialPaymentAmount {get; set;} // NSF fee - either 25 or 0
        public decimal OrignialAmount {get; set;} // NSF fee - either 25 or 0

        public int PaymentNumber {get; set;} 
        public string Status {get; set;}
        public DateTime PaymentDate {get; set;}
    }
}
