using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;
using LoanStop.Entities.Accounting;

namespace LoanStop.DBCore.Repository
{

    public class CashLog : ICashLog
    {
        public string ConnectionString {get; set;}

        public CashLog(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.CashLog cashLog)
        {
            Entity.CashLog newItem = new Entity.CashLog();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.Category = cashLog.Category;
                newItem.TransactionType = cashLog.TransactionType;
                newItem.Description = cashLog.Description;
                newItem.Amount = cashLog.Amount;
                newItem.Type = cashLog.Type;
                newItem.PayableTo = cashLog.PayableTo;
                newItem.SsNumber = cashLog.SsNumber;
                newItem.TransactionNumber = cashLog.TransactionNumber;
                newItem.Date = cashLog.Date;
                newItem.Timestamp = DateTime.Now;

                db.CashLogs.Add(newItem);

                db.SaveChanges();
            }
        }

        public decimal Balance(DateTime date)
        {

            decimal rtrn = 0;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                rtrn = db.CashLogs.Where(s => s.Date < date).Sum(s => s.Amount);
            }

            return rtrn;
        }

        public List<AccountingTableEntity> TableData(DateTime startDate, DateTime endDate, bool IsLoansOnly)
        {

            List<AccountingTableEntity> rtrn = null;

            var begingSelectedCashBalance = Balance(startDate);

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                if (IsLoansOnly)
                {
                    rtrn = db.CashLogs.Where(s => (s.Date >= startDate && s.Date <= endDate) && s.Type == "loan")
                                      .Select(newElement => new AccountingTableEntity()
                                      {
                                          Id = newElement.Id,
                                          TransactionType = newElement.TransactionType, 
                                          TransDate= newElement.Date, 
                                          Debit = Math.Abs(newElement.Amount), 
                                          Credit = null,
                                          PayableTo = newElement.PayableTo,
                                          Description = newElement.Description,
                                          Employee = newElement.Employee, 
                                          Balance = null
                                      }).OrderBy(o => o.Id).ToList();
                }
                else
                {
                    var credit = db.CashLogs.Where(s => (s.Date  >= startDate && s.Date <= endDate) && s.Type.ToLower() == "credit")
                          .Select(newElement => new AccountingTableEntity()
                          {
                              Id = newElement.Id,
                              TransactionType = newElement.TransactionType,
                              TransDate = newElement.Date,
                              Debit = null,
                              Credit = Math.Abs(newElement.Amount),
                              PayableTo = newElement.PayableTo,
                              Description = newElement.Description,
                              Employee = newElement.Employee,
                              Balance = null
                          }).ToList();


                    var debit = db.CashLogs.Where(s => (s.Date >= startDate && s.Date <= endDate) && s.Type.ToLower() == "debit")
                          .Select(newElement => new AccountingTableEntity()
                          {
                              Id = newElement.Id,
                              TransactionType = newElement.TransactionType,
                              TransDate = newElement.Date,
                              Debit = Math.Abs(newElement.Amount),
                              Credit = null,
                              PayableTo = newElement.PayableTo,
                              Description = newElement.Description,
                              Employee = newElement.Employee,
                              Balance = null
                          }).ToList();

                    rtrn = credit.Union(debit).ToList().OrderBy(s => s.Id).ToList();

                    var currentBalance = begingSelectedCashBalance;

                    foreach(var entity in rtrn)
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
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public List<LoanStopModel.CashLog> GetAllForTransaction(long transactionId)
        {
            List<LoanStopModel.CashLog> returnValue = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.CashLogs.Where(s => s.TransactionNumber == transactionId);

                if (query.Count() > 0)
                {
                    returnValue = query.ToList();
                }
            }

            return returnValue;
        }

    }
}

