using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LoanStop.Entities.Accounting;
using Entity = LoanStopModel;
using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Business.Accounting
{
    public class Checkbook : BaseActivity
    {
            
        public Checkbook(string store, string connectionString) : base(store, connectionString) { }

        public void Transaction(string store, CheckbookModel model)
        {

            var checkbookRepository = new Repository.Checkbook(this.connectionString);

            logAction(this.store, "Checkbook-Post", model);
        
            var newItem = new Entity.Checkbook();

            newItem.DateEntered = DateTime.Now.Date;
            newItem.Category = model.Category;
            newItem.TransactionType = model.TransactionType;
            newItem.Description = model.Description;
            newItem.Amount = model.Amount;
            newItem.Type = model.Type;
            newItem.PayableTo = model.PayableTo;
            newItem.SsNumber = model.SsNumber;
            newItem.TransactionNumber = model.TransactionNumber;

            checkbookRepository.Add(newItem);
        }
    

        public void Void(string store, CheckbookModel model)
        {
            logAction(this.store, "Checkbook-Void", model);

            //var item = cashRepository.Get();

            //item.Type = "Void";
            //item.Timestamp = DateTime.Now;

            //checkbookRepository.Update(newItem);

        }

    
    }
}