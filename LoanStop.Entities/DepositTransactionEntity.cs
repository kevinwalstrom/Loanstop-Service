using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace LoanStop.Entities
{
    [DataContract]
    public class DepositTransactionEntity
    {
        [DataMember]
        public List<DepositEntity> Deposits;
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public decimal CheckAmount { get; set; }
        [DataMember]
        public decimal ACHAmount { get; set; }
        [DataMember]
        public decimal Cash { get; set; }
    }
}
