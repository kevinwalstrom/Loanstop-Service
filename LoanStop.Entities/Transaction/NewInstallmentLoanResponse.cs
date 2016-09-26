using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    public class NewInstallmentLoanResponse
    {
 
        public NewInstallmentLoanResponse(bool prama1)
        {
            this.Error = prama1;
        }

        public bool Error { get; set; }
        public List<object> Ppcs { get; set; }
        public decimal APR { get; set; }
        public decimal FinanceCharge { get; set; }
        public decimal PrepaidFinanceCharge { get; set; }
    }
}
