using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    public class PaymentPlanCheckEntity
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long TransactionId {get; set;}
        [DataMember]
        public string SsNumber { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Issuer { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public decimal AmountRecieved { get; set; }
        [DataMember]
        public decimal AmountPaid { get; set; }
        [DataMember]
        public string OrignalAmount { get; set; }
        [DataMember]
        public DateTime TransDate { get; set; }
        [DataMember]
        public DateTime? DateCleared { get; set; }
        [DataMember]
        public DateTime? DateReturned { get; set; }
        [DataMember]
        public DateTime? DatePaid { get; set; }
        [DataMember]
        public string Status { get; set; }
    }
}
