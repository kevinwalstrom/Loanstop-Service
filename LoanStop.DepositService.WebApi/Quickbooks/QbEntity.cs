using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStop.Services.WebApi.Quickbooks
{
    public class QbEntity
    {
        
        public string Trns {get; set;}
        public string TrnsType{get; set;}
        public DateTime Date{get; set;}
        public string Accnt{get; set;}
        public string Name{get; set;}
        public decimal Amount{get; set;}
        public string Docnum{get; set;}
        public string Memo{get; set;}
        public string Class{get; set;}
        public string Clear{get; set;}
        public string ToPrint{get; set;}
        public string Addr1{get; set;}
        public string Addr2{get; set;}
        public string Addr3{get; set;}

        public QbEntity()
        {
            this.Docnum = string.Empty;
            this.Memo = string.Empty;
            this.Clear = "N";
            this.ToPrint = "N";
            this.Addr1 = string.Empty;
            this.Addr2 = string.Empty;
            this.Addr3 = string.Empty;
        }

    }
}