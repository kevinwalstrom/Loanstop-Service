using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using Entity = LoanStopModel;


namespace LoanStop.DBCore.Repository
{

    public class Transaction : ITransaction
    {
        
        public string ConnectionString {get; set;}

        public Transaction(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public Entity.Transaction Add(Entity.Transaction transaction)
        {
            Entity.Transaction newItem = new Entity.Transaction();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.AmountDispursed = transaction.AmountDispursed;
                newItem.AmountRecieved = transaction.AmountRecieved;
                newItem.CheckNumber = transaction.CheckNumber;
                newItem.CheckType = transaction.CheckType;
                newItem.Consecutive = transaction.Consecutive;
                newItem.DateCleared = transaction.DateCleared;
                newItem.DateDue = transaction.DateDue;
                newItem.DateReturned = transaction.DateReturned;
                newItem.DispursedAs = transaction.DispursedAs;
                newItem.HoldDate = transaction.HoldDate;
                newItem.Issuer = transaction.Issuer;
                newItem.Location= transaction.Location;
                newItem.Name = transaction.Name;
                newItem.SsNumber = transaction.SsNumber;
                newItem.Status = transaction.Status;
                newItem.TransDate = transaction.TransDate;
                newItem.TimeStamp = transaction.TransDate.Value;

                db.Transactions.Add(newItem);

                db.SaveChanges();

                transaction.Id = newItem.Id;
            }

            return transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        public void Update(Entity.Transaction transaction)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                Entity.Transaction updateItem = db.Transactions.Find(transaction.Id);

                updateItem.AmountDispursed = transaction.AmountDispursed;
                updateItem.AmountRecieved = transaction.AmountRecieved;
                updateItem.CheckNumber = transaction.CheckNumber;
                updateItem.CheckType = transaction.CheckType;
                updateItem.Consecutive = transaction.Consecutive;
                updateItem.DateCleared = transaction.DateCleared;
                updateItem.DateDue = transaction.DateDue;
                updateItem.DateReturned = transaction.DateReturned;
                updateItem.DispursedAs = transaction.DispursedAs;
                updateItem.HoldDate = transaction.HoldDate;
                updateItem.Issuer = transaction.Issuer;
                updateItem.Location = transaction.Location;
                updateItem.Name = transaction.Name;
                updateItem.SsNumber = transaction.SsNumber;
                updateItem.Status = transaction.Status;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public Entity.Transaction GetById(long transactionId)
        {
            Entity.Transaction rtrnTransaction;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                rtrnTransaction = db.Transactions.Find(transactionId);
            }

            var ppcRepository = new PaymentPlanCheck(ConnectionString);

            rtrnTransaction.FeeApplied = false;
            rtrnTransaction.PaymentPlanChecks = ppcRepository.GetByTransaction(rtrnTransaction.Id) as List<Entity.PaymentPlanCheck>;

            foreach (var ppc in rtrnTransaction.PaymentPlanChecks)
            {
                if ((ppc.Status == "Bounced" || ppc.Status == "Paid NSF" || ppc.Status == "Partial NSF") && rtrnTransaction.FeeApplied == false)
                {
                    ppc.OtherFee = 25.0m;
                    rtrnTransaction.FeeApplied = true;
                }
            }

            return rtrnTransaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryDate"></param>
        /// <returns></returns>
        public ICollection<Entity.Transaction> GetByDate(DateTime queryDate)
        {
            List<Entity.Transaction> rtrnTransaction = new List<Entity.Transaction>() ;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                var query = db.Transactions.Where(s => s.TransDate == queryDate);

                foreach (var t in query)
                {
                    rtrnTransaction.Add(t);
                }
            }

            return rtrnTransaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssNumber"></param>
        /// <returns></returns>
        public ICollection<Entity.Transaction> GetBySSNumber(string ssNumber)
        {
            List<Entity.Transaction> rtrnTransaction = new List<Entity.Transaction>();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                var query = db.Transactions.Where(s => s.SsNumber == ssNumber);

                foreach (var t in query)
                {
                    var newItem = new Entity.Transaction();

                    newItem.Id  = t.Id;
                    newItem.TransDate = t.TransDate;
                    newItem.AmountDispursed = t.AmountDispursed;
                    newItem.AmountRecieved = t.AmountRecieved;
                    newItem.CheckNumber = t.CheckNumber;
                    newItem.CheckType = t.CheckType;
                    newItem.Consecutive = t.Consecutive;
                    newItem.DateCleared = t.DateCleared;
                    newItem.DateDue = t.DateDue;
                    newItem.DateReturned = t.DateReturned;
                    newItem.DispursedAs = t.DispursedAs;
                    newItem.HoldDate = t.HoldDate;
                    newItem.Issuer = t.Issuer;
                    newItem.Location = t.Location;
                    newItem.Name = t.Name;
                    newItem.SsNumber = t.SsNumber;
                    newItem.Status = t.Status;
                    newItem.TransDate = t.TransDate;
                    newItem.TimeStamp = t.TimeStamp;

                    rtrnTransaction.Add(newItem);
                }
            }

            return rtrnTransaction.OrderByDescending(s => s.Id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssNumber"></param>
        /// <returns></returns>
        public decimal CustomerAmountDisbursed(string ssNumber)
        {
            decimal returnValue = 0;
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                string command = string.Format("SELECT amount_dispursed FROM transactions WHERE ss_number = '{0}' AND status IN ('Open', 'Deposit', 'Pick Up', 'Payment Plan' )", ssNumber);

                var query = db.Database.SqlQuery<TransactionTable>(command);

                returnValue = query.Sum(s => s.amount_dispursed);
            }

            return returnValue;
        }

    }

    class TransactionTable
    {
        public virtual decimal amount_dispursed { get; set;}
    }

}
