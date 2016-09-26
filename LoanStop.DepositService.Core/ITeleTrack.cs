using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.DepositService.Core
{
    interface ITeleTrack
    {
         long IssueLoan();
         long PartialPayment();
         long PaidInFull();
    }
}
