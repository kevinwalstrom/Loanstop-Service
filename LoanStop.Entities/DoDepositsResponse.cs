using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    public class DoDepositsResponse
    {
        [DataMember]
        public List<DepositEntity> Closed { get; set; }

        [DataMember]
        public List<string[]> Receipt { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string ACHAmount { get; set; }
    
    }

}
