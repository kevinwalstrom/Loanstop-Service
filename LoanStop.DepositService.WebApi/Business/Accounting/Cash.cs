using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LoanStop.Entities.Accounting;
using Entity = LoanStopModel;
using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Business.Accounting
{
    public class Cash : BaseActivity
    {
    
        public Cash(string store, string connectionString) : base(store, connectionString)
        {
        }
        
        public void Transaction(string store, CashModel model)
        {
            var cashRepository = new Repository.CashLog(this.connectionString);

            logAction(this.store, "Cash-Post", model);
        
            var newItem = new Entity.CashLog();

            newItem.Category = model.Category;
            newItem.TransactionType = model.TransactionType;
            newItem.Description = model.Description;
            newItem.Amount = model.Amount;
            newItem.Type = model.Type;
            newItem.PayableTo = model.PayableTo;
            newItem.SsNumber = model.SsNumber;
            newItem.TransactionNumber = model.TransactionNumber;
            newItem.Date = model.Date;
            newItem.Timestamp = DateTime.Now;

            cashRepository.Add(newItem);
        
        }
    
        public void Void(string store, CashModel model)
        {

            logAction(this.store, "Cash-Void", model);
            
            //var item = cashRepository.Get();

            //item.Type = "Void";
            //item.Timestamp = DateTime.Now;

            //cashRepository.Update(newItem);

        }

    
    }
}