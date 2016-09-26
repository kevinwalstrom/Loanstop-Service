using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    public class GetDepositsResponse
    {
        [DataMember]
        public List<DepositEntity> ACHDeposits { get; set; }
        [DataMember]
        public List<DepositEntity> ClosedAccounts { get; set; }
    }
}
