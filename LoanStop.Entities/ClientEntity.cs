using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    public class ClientEntity
    {
        [DataMember]
        public string SsNumber { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public string Lastname { get; set; }
        [DataMember]
        public string Firstname { get; set; }
        [DataMember]
        public string DriverLicense { get; set; }
        [DataMember]
        public string Employer { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string HomePhone { get; set; }
        [DataMember]
        public string WorkPhone { get; set; }
        [DataMember]
        public string PaydaySchedule { get; set; }
        [DataMember]
        public string PaydayNotes { get; set; }
        [DataMember]
        public string HasMc { get; set; }
        [DataMember]
        public string Intl { get; set; }

        [DataMember]
        public string NetFromPaystubs { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string BankAccount { get; set; }
        [DataMember]
        public string RoutingNumber { get; set; }
        
    }
}
