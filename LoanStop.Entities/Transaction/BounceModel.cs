using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    [DataContract]
    public class BounceModel
    {
        [DataMember]
        public string SsNumber { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public long PaymentPlanCheckId { get; set; }
        [DataMember]
        public decimal PaymentAmount { get; set; }
        [DataMember]
        public decimal MonthlyFee { get; set; }
        [DataMember]
        public DateTime DateReturned { get; set; }
        [DataMember]
        public int PaymentNumber { get; set; }
    
    }
}
