using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace LoanStop.Services.WebApi.Models
{
    
    [DataContract]
    public class RequestType
    {
        [DataMember]
        public string Store {get; set;}
        [DataMember]
        public string Entity { get; set; }
    }
}