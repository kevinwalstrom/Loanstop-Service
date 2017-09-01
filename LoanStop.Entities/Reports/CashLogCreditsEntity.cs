using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Reports
{
    public class CashLogCreditsDebitsEntity
    {
        public string transaction_type { get; set; }
        public decimal amount { get; set; }

    }

    public class AgingEntity
    {
        public string TransactionState { get; set; }
        public string AgingRange { get; set; }
        public int NumberOfTransactions { get; set; }
    }

}
