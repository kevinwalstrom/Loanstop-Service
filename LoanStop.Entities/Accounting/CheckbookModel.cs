using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Accounting
{
    public class CheckbookModel
    {
        public string Category {get; set;}
        public string TransactionType {get; set;}
        public string Description  {get; set;}
        public decimal Amount {get; set;}
        public string Type {get; set;}
        public string PayableTo {get; set;}
        public string SsNumber {get; set;}
        public long? TransactionNumber {get; set;}
    }
}
