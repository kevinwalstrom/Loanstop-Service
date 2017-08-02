using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public class Payment : IPayment
    {
        public string ConnectionString {get; set;}

        public Payment(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.Payment payment)
        {
            Entity.Payment newItem = new Entity.Payment();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.DateDue = payment.DateDue;
                newItem.TransactionId = payment.TransactionId;
                newItem.SsNumber = payment.SsNumber; 
                newItem.Name = payment.Name;
                newItem.Balance = payment.Balance;
                newItem.Description = payment.Description;
                newItem.DatePaid = payment.DatePaid;
                newItem.AmountPaid = payment.AmountPaid;
                newItem.AmountDue = payment.AmountDue;
                newItem.PaymentNumber = payment.PaymentNumber;
                newItem.PaymentType = payment.PaymentType;
                newItem.OtherFees = payment.OtherFees;

                db.Payments.Add(newItem);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasCustomerMadePayments(string ssNumber, DateTime date)
        {
            bool returnValue = false;
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                string command = string.Format("SELECT * FROM payments WHERE ss_number = '{0}'", ssNumber);

                var query = db.Database.SqlQuery<PaymentTable>(command);

                if (query.Count() > 0)
                    returnValue = true;
            }

            return returnValue;
        }


        public List<LoanStopModel.Payment> GetAllForTransaction(long transactionId)
        {
            List<LoanStopModel.Payment> returnValue = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Payments.Where(s => s.TransactionId == transactionId);

                if (query.Count() > 0)
                {
                    returnValue = query.ToList();
                }
            }

            return returnValue;
        }

        public List<LoanStopModel.Payment> GetPaymentNumberEqualsNullForTransaction(long transactionId)
        {
            List<LoanStopModel.Payment> returnValue = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Payments.Where(s => s.TransactionId == transactionId && s.PaymentNumber == null);

                if (query.Count() > 0)
                {
                    returnValue = query.ToList();
                }
            }

            return returnValue;
        }

    }

    class PaymentTable
    {
        public virtual int id { get; set;}
        public virtual int transaction_id { get; set;}
        public virtual string name { get; set;}
        public virtual string ss_number { get; set;}
        public virtual string description { get; set;}
        public virtual DateTime? date_due { get; set;}
        public virtual DateTime? date_paid { get; set;}
        public virtual string amount_due { get; set;}
        public virtual string amount_paid { get; set;}
        public virtual string other_fees { get; set;}
        public virtual string balance { get; set;}
        public virtual string posted_2_peach { get; set;}
        public virtual string remarks { get; set;}
        public virtual string location { get; set;}
        public virtual int? payment_number { get; set;}
        public virtual string walked { get; set;}
        public virtual string payment_type { get; set;}

    }


}
