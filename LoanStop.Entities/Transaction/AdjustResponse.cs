using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LoanStop.Entities.Transaction
{
    public class AdjustResponse
    {
        public object Transaction {get;  set;}
        public List<PaymentPlanCheckEntity> ppcs {get;  set;}
        public List<PaymentEntity> payments {get;  set;}
        public List<CheckEntity> checks {get;  set;}
        public List<CashEntity> cash {get;  set;}
    }
}
