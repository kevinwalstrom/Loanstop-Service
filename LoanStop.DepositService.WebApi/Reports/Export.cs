using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Services.WebApi;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.Services.WebApi.Reports
{
    public class Export : BaseActivity
    {
        IStateQuries rep;
    
        public Export(string store, string state, string connectionString) : base(store, connectionString)
        {
            if (state.ToLower() == "colorado")
            { 
                rep = new Repository.Colorado(connectionString);
            }
            else
            {
                rep = new Repository.Wyoming(connectionString);
            }
        
        }

        public List<ExportEntity> ExportStore(DateTime startDate, DateTime endDate)
        { 
            var rtrn = rep.CallExport(startDate, endDate);

            return rtrn;
        }

        public List<QuickBooksStoreExport> ExportLineItem(string store, string lineItem, DateTime startDate, DateTime endDate)
        { 

            List<QuickBooksStoreExport> rtrn = new List<QuickBooksStoreExport>();
            var exportStore = new QuickBooksStoreExport();
            exportStore.Store = store;
            exportStore.Results = rep.ExportLineItem(lineItem, startDate, endDate);

            rtrn.Add(exportStore);

            return rtrn;
        }

        public List<ExportEntity> ExecuteQuriesCO(DateTime startDate, DateTime endDate)
        {

            List<ExportEntity> rtrn = new List<ExportEntity>();

            //loans
            var cashLoans = rep.ExportLineItem("loaned",startDate, endDate);
            rtrn.AddRange(cashLoans);
            
            //ach payments - by day, cash payments
            var payments = rep.ExportLineItem("paymentstoloans",startDate, endDate);
            rtrn.AddRange(payments);

            //bounced
            var bouncers = rep.ExportLineItem("bounced",startDate, endDate);
            rtrn.AddRange(bouncers);
            
            //money gram
            var moneygrams = rep.ExportLineItem("moneygram",startDate, endDate);
            rtrn.AddRange(moneygrams);
            
            //check gold
            var checkgolds = rep.ExportLineItem("checkgold",startDate, endDate);
            rtrn.AddRange(checkgolds);

            //defaulted
            var defaulted = rep.ExportLineItem("defaulted",startDate, endDate);
            rtrn.AddRange(defaulted);

            //paymentstodefault
            var paymentstodefault = rep.ExportLineItem("paymentstodefault",startDate, endDate);
            rtrn.AddRange(paymentstodefault);

            //transfers
            var transfers = rep.ExportLineItem("transfers",startDate, endDate);
            rtrn.AddRange(transfers);
            
            //getcash
            var getcashs = rep.ExportLineItem("getcash",startDate, endDate);
            rtrn.AddRange(getcashs);

            //expenses
            var expenses = rep.ExportLineItem("expenses",startDate, endDate);
            rtrn.AddRange(expenses);
            
            return rtrn;
       }

        public List<ExportEntity> ExecuteQuriesWY(DateTime startDate, DateTime endDate)
        {

            List<ExportEntity> rtrn = new List<ExportEntity>();

            //loans
            var cashLoans = rep.ExportLineItem("loaned",startDate, endDate);
            rtrn.AddRange(cashLoans);
            
            //ach payments - by day, cash payments
            var payments = rep.ExportLineItem("checkDeposits",startDate, endDate);
            rtrn.AddRange(payments);

            var ach = rep.ExportLineItem("achDeposits",startDate, endDate);
            rtrn.AddRange(ach);

            var cashD = rep.ExportLineItem("cashDeposits",startDate, endDate);
            rtrn.AddRange(cashD);

            var cashpp = rep.ExportLineItem("cashpaidpp",startDate, endDate);
            rtrn.AddRange(cashpp);

            //var fees = rep.ExportLineItem("wyomingfeeincome",startDate, endDate);
            //rtrn.AddRange(fees);

            //bounced
            var bouncers = rep.ExportLineItem("bouncedWy",startDate, endDate);
            rtrn.AddRange(bouncers);
            
            //money gram
            var moneygrams = rep.ExportLineItem("moneygram",startDate, endDate);
            rtrn.AddRange(moneygrams);
            
            //check gold
            var checkgolds = rep.ExportLineItem("checkgold",startDate, endDate);
            rtrn.AddRange(checkgolds);

            //defaulted
            var defaulted = rep.ExportLineItem("defaulted",startDate, endDate);
            rtrn.AddRange(defaulted);

            //paymentstodefault
            var paymentstodefault = rep.ExportLineItem("paymentstodefault",startDate, endDate);
            rtrn.AddRange(paymentstodefault);

            //transfers
            var transfers = rep.ExportLineItem("transfers",startDate, endDate);
            rtrn.AddRange(transfers);
            
            //getcash
            var getcashs = rep.ExportLineItem("getcash",startDate, endDate);
            rtrn.AddRange(getcashs);

            //expenses
            var expenses = rep.ExportLineItem("expenses",startDate, endDate);
            rtrn.AddRange(expenses);
            
            return rtrn;
       }


    }
}