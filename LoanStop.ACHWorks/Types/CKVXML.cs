using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.ACHWorks.Types
{
    [Serializable()]
    public class CKVXML
    {

        [System.Xml.Serialization.XmlElement("amount")]
        public string Amount { get; set; }

        [System.Xml.Serialization.XmlElement("accttype")]
        public string AcctType { get; set; }

        [System.Xml.Serialization.XmlElement("routingno")]
        public string RoutingNo { get; set; }

        [System.Xml.Serialization.XmlElement("acctno")]
        public string AcctNo { get; set; }

        [System.Xml.Serialization.XmlElement("code")]
        public string Code { get; set; }

        [System.Xml.Serialization.XmlElement("result")]
        public string Result { get; set; }

        [System.Xml.Serialization.XmlElement("meaning")]
        public string Meaning { get; set; }

        [System.Xml.Serialization.XmlElement("description")]
        public string Description { get; set; }

    }
}
