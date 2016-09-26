using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanStopModel
{
    public partial class PaymentPlanCheck
    {
        private decimal otherFee = 0;

        [NotMapped]
        public decimal OtherFee
        {
            set
            {
               otherFee = value;
            }
            get
            {
               return otherFee;
            }
        }
    }
}
