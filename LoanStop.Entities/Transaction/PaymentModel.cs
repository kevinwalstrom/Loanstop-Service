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

    public class PaymentTableModel
    {
        //public ClientEntity Client {get; set;}
        //public TransactionEntity Transaction {get; set;}
        //public PaymentPlanCheckEntity Ppc {get; set;}

        public string SsNumber { get; set; }
        public long TransactionId { get; set; }
        public long PpcId { get; set; }

        public string Firstname { get; set; } 
        public string Lastname { get; set; }


        public decimal OtherFees { get; set; } //amount to payoff loan
        public decimal AmountPaid { get; set; } //contracted amount to be paid
        public decimal AmountDue { get; set; } // actual amoutn paid
        public decimal Balance { get; set; } // fee amount for the month

        public bool IsCreditCardPayment { get; set; }
        public bool IsACH { get; set; }

        public int PaymentNumber { get; set; }
        public string Status { get; set; }
        public DateTime DatePaid { get; set; }
    }

    public class MoneyGramModel
    {

        public string MoneyGramType { get; set; }
        public int MoneyOrderCount { get; set; }

        public string SsNumber { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }


        public decimal CollectAmount { get; set; }
        public decimal LoadAmount { get; set; }
        public decimal Fee { get; set; }
        public decimal FeeDue { get; set; }



        public decimal OtherFees { get; set; } //amount to payoff loan
        public decimal AmountPaid { get; set; } //contracted amount to be paid
        public decimal AmountDue { get; set; } // actual amoutn paid
        public decimal Balance { get; set; } // fee amount for the month

        public bool IsCreditCardPayment { get; set; }
        public bool IsACH { get; set; }

        public int PaymentNumber { get; set; }
        public string Status { get; set; }
        public DateTime DatePaid { get; set; }
    }

}
