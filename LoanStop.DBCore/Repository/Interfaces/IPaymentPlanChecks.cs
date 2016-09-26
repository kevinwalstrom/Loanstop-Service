using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{

    public interface IPaymentPlanCheck
    {
        void Add(Entity.PaymentPlanCheck paymentPlanCheck);
        Entity.PaymentPlanCheck Update(Entity.PaymentPlanCheck paymentPlanCheck);
        Entity.PaymentPlanCheck GetById(long paymentPlanCheckId);
        ICollection<Entity.PaymentPlanCheck> GetByDate(DateTime queryDate);
        ICollection<Entity.PaymentPlanCheck> GetByTransaction(long transactionId);
    }

}
