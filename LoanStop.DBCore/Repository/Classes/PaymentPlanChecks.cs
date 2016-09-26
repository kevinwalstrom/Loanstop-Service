using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{

    public class PaymentPlanCheck : IPaymentPlanCheck
    {
        public string ConnectionString {get; set;}

        public PaymentPlanCheck(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.PaymentPlanCheck paymentPlanCheck)
        {
            Entity.PaymentPlanCheck newItem = new Entity.PaymentPlanCheck();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.AmountPaid = paymentPlanCheck.AmountPaid;
                newItem.AmountRecieved = paymentPlanCheck.AmountRecieved;
                newItem.CheckNumber = paymentPlanCheck.CheckNumber;
                newItem.DateCleared = paymentPlanCheck.DateCleared;
                newItem.DateDue = paymentPlanCheck.DateDue;
                newItem.DateReturned = paymentPlanCheck.DateReturned;
                newItem.HoldDate = paymentPlanCheck.HoldDate;
                newItem.Name = paymentPlanCheck.Name;
                newItem.OrignalAmount = paymentPlanCheck.OrignalAmount;
                newItem.SsNumber = paymentPlanCheck.SsNumber;
                newItem.TransactionId = paymentPlanCheck.TransactionId;
                newItem.Status = paymentPlanCheck.Status;
                newItem.TransDate = paymentPlanCheck.TransDate;

                db.PaymentPlanChecks.Add(newItem);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        public Entity.PaymentPlanCheck Update(Entity.PaymentPlanCheck paymentPlanCheck)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                Entity.PaymentPlanCheck updateItem = db.PaymentPlanChecks.Find(paymentPlanCheck.Id);

                updateItem.AmountPaid = paymentPlanCheck.AmountPaid;
                updateItem.AmountRecieved = paymentPlanCheck.AmountRecieved;
                updateItem.CheckNumber = paymentPlanCheck.CheckNumber;
                updateItem.DateCleared = paymentPlanCheck.DateCleared;
                updateItem.DateDue = paymentPlanCheck.DateDue;
                updateItem.DatePaid = paymentPlanCheck.DatePaid;
                updateItem.Status = paymentPlanCheck.Status;
                updateItem.DateReturned = paymentPlanCheck.DateReturned;
                updateItem.HoldDate = paymentPlanCheck.HoldDate;
                updateItem.Name = paymentPlanCheck.Name;
                updateItem.OrignalAmount = paymentPlanCheck.OrignalAmount;
                updateItem.SsNumber = paymentPlanCheck.SsNumber;
                updateItem.TransactionId = paymentPlanCheck.TransactionId;

                db.SaveChanges();
            }

            return paymentPlanCheck;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public Entity.PaymentPlanCheck GetById(long paymentPlanCheckId)
        {
            Entity.PaymentPlanCheck rtrnTransaction;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                rtrnTransaction = db.PaymentPlanChecks.Find(paymentPlanCheckId);
            }

            return rtrnTransaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        public ICollection<Entity.PaymentPlanCheck> GetByDate(DateTime queryDate)
        {
            List<Entity.PaymentPlanCheck> rtrnTransaction = new List<Entity.PaymentPlanCheck>();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                var query = db.PaymentPlanChecks.Where(s => s.TransDate == queryDate);

                foreach (var t in query)
                {
                    var newItem = new Entity.PaymentPlanCheck()
                    {

                    };

                    rtrnTransaction.Add(newItem);
                }
            }

            return rtrnTransaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssNumber"></param>
        /// <returns></returns>
        public ICollection<Entity.PaymentPlanCheck> GetByTransaction(long transactionId)
        {
            List<Entity.PaymentPlanCheck> rtrnPaymentPlanChecks = new List<Entity.PaymentPlanCheck>();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.PaymentPlanChecks.Where(s => s.TransactionId == transactionId).OrderBy(o => o.DateDue);

                foreach (var t in query)
                {
                    rtrnPaymentPlanChecks.Add(t);
                }
            }

            return rtrnPaymentPlanChecks;
        }

    }


}
