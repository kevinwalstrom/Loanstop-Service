using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanStop.DailyBalance
{
    public class SummaryBase
    {
        protected string _sql;
        public string Store { get; set; }
        public DateTime TheDate { get; set; }
        //public string Total { get; set; }
  
    }
}
