using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    public class NewInstallmentLoanModel
    {
        public string SsNumber { get; set;}
        public decimal AmountDisbursed { get; set;}
        public decimal AmountReceived { get; set;}
        public string PaymentPreference { get; set;}
        public decimal PaymentAmount { get; set;}
        public DateTime FirstPaymentDay { get; set;}
        public DateTime TransDate { get; set;}
        public string StoreCode { get; set;}
        public List<InstallmentRecord> Records { get; set;}
    }

    public class InstallmentRecord
    {
        public int? Id { get; set;}
        public int PaymentNumber { get; set;}
        public DateTime? HoldDate { get; set;}
    }

    public class AdjustHoldDatesModel
    {
        public List<InstallmentRecord> Records { get; set;}
    }


}
