using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.Export;
using LoanStop.Entities.CommonTypes;
using System.Diagnostics;

namespace LoanStop.Services.WebApi.Reports
{
    public class ExportAll
    {
    

        public List<QuickBooksStoreExport> ExportMultiple(List<StoreConnectionType> states, DateTime startDate, DateTime endDate)
        { 
            List<QuickBooksStoreExport> rtrn = new List<QuickBooksStoreExport>();
            
            foreach (var store in states)
            {
               string connString = store.ConnectionString();
               
               Debug.WriteLine(connString);
               
               var exp = new Export(store.StoreName, store.State, connString);

                List<ExportEntity> results;
                if(store.State.ToLower() == "colorado")
                { 
                   results = exp.ExecuteQuriesCO(startDate, endDate);
                }
                else 
                { 
                   results = exp.ExecuteQuriesWY(startDate, endDate);
                }


               var item = new QuickBooksStoreExport(){State = store.State, Store = store.DatabaseName, Results = results};
               
               rtrn.Add(item);
            }

            return rtrn;
        }

        //public 


    }
}