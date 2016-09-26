using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    public class TransactionEntity
    {
        [DataMember]
        public long Id { get; set; }
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
        public decimal AmountDispursed { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public List<PaymentPlanCheckEntity> PaymentPlanChecks { get; set; }
        [DataMember]
        public DateTime TransDate { get; set; }
    }
}
