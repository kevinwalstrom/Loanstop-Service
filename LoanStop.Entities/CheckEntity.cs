using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities
{
    public class CheckEntity
    {
        public string TransactionType {get; set;}
        public int? CheckNumber {get; set;}
        public string PayableTo {get; set;}
        public decimal Amount {get; set;}
        public string Type {get; set;}
        public DateTime DateEntered  {get; set;} 
        public string Category  {get; set;} 
        public DateTime DateTime  {get; set;} 
        public string Description  {get; set;} 
        public string SsNumber  {get; set;} 
        public long? TransactionNumber  {get; set;} 
    }
}
