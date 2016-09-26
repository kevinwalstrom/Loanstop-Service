using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    public class CashCheckResponse
    {
        public CashCheckResponse(bool prama1)
        {
            this.Error = prama1;
        }

        public bool Error { get; set; }
        public List<object> Ppcs { get; set; }
    
    }
}
