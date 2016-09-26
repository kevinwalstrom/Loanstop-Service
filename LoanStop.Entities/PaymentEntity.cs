using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities
{
    public class PaymentEntity
    {

        public long Id { get; set;}
        public long TransactionId { get; set;}
        public string Name { get; set;}
        public string SsNumber { get; set;}
        public string Description { get; set;}
        public DateTime? DateDue { get; set;}
        public DateTime? DatePaid { get; set;}
        public decimal? AmountDue { get; set;}
        public decimal AmountPaid { get; set;}
        public decimal? OtherFees { get; set;}
        public decimal Balance { get; set;}
        public int? PaymentNumber { get; set;}
        public string PaymentType { get; set;}
    }
}
