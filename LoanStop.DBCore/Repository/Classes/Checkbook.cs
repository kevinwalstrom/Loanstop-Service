using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;
using LoanStop.Entities.Accounting;

namespace LoanStop.DBCore.Repository
{
    public class Checkbook : ICheckbook
    {
        public string ConnectionString {get; set;}

        public Checkbook(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.Checkbook cashLog)
        {
            Entity.Checkbook newItem = new Entity.Checkbook();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.DateEntered = DateTime.Now.Date;
                newItem.DateTime = DateTime.Now;
                newItem.Category = cashLog.Category;
                newItem.TransactionType = cashLog.TransactionType;
                newItem.Description = cashLog.Description;
                newItem.Amount = cashLog.Amount;
                newItem.Type = cashLog.Type;
                newItem.PayableTo = cashLog.PayableTo;
                newItem.SsNumber = cashLog.SsNumber;
                newItem.TransactionNumber = cashLog.TransactionNumber;

                db.Checkbooks.Add(newItem);

                db.SaveChanges();
            }
        }

        public decimal Balance(DateTime date)
        {

            decimal rtrn = 0;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                rtrn = db.Checkbooks.Where(s => s.DateEntered < date).Sum(s => s.Amount);
            }

            return rtrn;
        }

        public List<AccountingTableEntity> TableData(DateTime startDate, DateTime endDate, bool IsLoansOnly)
        {

            List<AccountingTableEntity> rtrn = null;

            var begingSelectedCheckbookBalance = Balance(startDate);

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                if (IsLoansOnly)
                {
                    rtrn = db.Checkbooks.Where(s => (s.DateEntered >= startDate && s.DateEntered <= endDate) && s.Type == "loan")
                                      .Select(newElement => new AccountingTableEntity()
                                      {
                                          Id = newElement.Id,
                                          TransactionType = newElement.TransactionType,
                                          TransDate = newElement.DateEntered,
                                          CheckNumber = newElement.CheckNumber > 10 ? (long?)newElement.CheckNumber : null,
                                          Debit = Math.Abs(newElement.Amount),
                                          Credit = null,
                                          PayableTo = newElement.PayableTo,
                                          Description = newElement.Description,
                                          Employee = newElement.Employee,
                                          Balance = null
                                      }).ToList();
                }
                else
                {
                    var credit = db.Checkbooks.Where(s => (s.DateEntered >= startDate && s.DateEntered <= endDate) && s.Type.ToLower() == "credit")
                          .Select(newElement => new AccountingTableEntity()
                          {
                              Id = newElement.Id,
                              TransactionType = newElement.TransactionType,
                              TransDate = newElement.DateEntered,
                              CheckNumber = newElement.CheckNumber > 10 ? (long?)newElement.CheckNumber : null,
                              Debit = null,
                              Credit = Math.Abs(newElement.Amount),
                              PayableTo = newElement.PayableTo,
                              Description = newElement.Description,
                              Employee = newElement.Employee,
                              Balance = null
                          }).ToList();


                    var debit = db.Checkbooks.Where(s => (s.DateEntered >= startDate && s.DateEntered <= endDate) && s.Type.ToLower() == "debit")
                          .Select(newElement => new AccountingTableEntity()
                          {
                              Id = newElement.Id,
                              TransactionType = newElement.TransactionType,
                              TransDate = newElement.DateEntered,
                              CheckNumber = newElement.CheckNumber > 10 ? (long?)newElement.CheckNumber : null,
                              Debit = Math.Abs(newElement.Amount),
                              Credit = null,
                              PayableTo = newElement.PayableTo,
                              Description = newElement.Description,
                              Employee = newElement.Employee,
                              Balance = null
                          }).ToList();

                    rtrn = credit.Union(debit).ToList().OrderBy(s => s.Id).ToList();

                    var currentBalance = begingSelectedCheckbookBalance;

                    foreach (var entity in rtrn)
                    {
                        if (entity.Debit.HasValue)
                            currentBalance = currentBalance - entity.Debit.Value;

                        if (entity.Credit.HasValue)
                            currentBalance = currentBalance + entity.Credit.Value;

                        entity.Balance = currentBalance;
                    }

                    rtrn = rtrn.OrderBy(s => s.Id).ToList();

                }
            }

            return rtrn;
        }

        public List<LoanStopModel.Checkbook> GetAllForTransaction(long transactionId)
        {
            List<LoanStopModel.Checkbook> returnValue = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Checkbooks.Where(s => s.TransactionNumber == transactionId);

                if (query.Count() > 0)
                {
                    returnValue = query.ToList();
                }
            }

            return returnValue;
        }


    }

}
