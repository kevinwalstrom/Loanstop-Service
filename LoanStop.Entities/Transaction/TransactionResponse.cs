using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{

    public class TransactionResponse
    {
        public TransactionEntity Transaction { get; set;} 
        public List<PaymentPlanCheckEntity> Ppcs { get; set;} 
        public List<PaymentEntity> Payments { get; set;} 
    }

}
