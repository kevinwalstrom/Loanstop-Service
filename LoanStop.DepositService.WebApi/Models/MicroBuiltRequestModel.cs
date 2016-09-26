using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace LoanStop.Services.WebApi.Models
{
    [DataContract]
    public class MicroBuiltRequestModel
    {
        [DataMember]
        public string RequestType {get; set;}
        [DataMember]
        public string Store { get; set; }
        [DataMember]
        public string SSNumber { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public decimal Balance { get; set; }       
    }
}