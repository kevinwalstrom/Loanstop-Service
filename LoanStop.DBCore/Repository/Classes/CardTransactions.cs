using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;
using LoanStop.Entities.Accounting;

namespace LoanStop.DBCore.Repository
{

    public class CardTransactions 
    {
        public string ConnectionString { get; set; }

        public CardTransactions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.CardTransaction cardTrans)
        {
            Entity.CardTransaction newItem = new Entity.CardTransaction();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.LoadAmount = cardTrans.LoadAmount;
                newItem.Status = cardTrans.Status;
                newItem.Total = cardTrans.Total;
                newItem.TotalDue = cardTrans.TotalDue;
                newItem.Trnx = cardTrans.Trnx;
                newItem.SsNumber = cardTrans.SsNumber;
                newItem.TrnxId = cardTrans.TrnxId;
                newItem.Datetime = new DateTime();
                newItem.Fee = cardTrans.Fee;
                newItem.FeeDue = cardTrans.FeeDue;

                db.CardTransactions.Add(newItem);

                db.SaveChanges();
            }
        }

    }
}


