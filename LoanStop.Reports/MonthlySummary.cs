using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.Reports
{
    public class MonthlySummary 
    {
        public MonthlySummary() 
        {}

        public List<MonthlySummaryEntity> Execute(List<StoreConnectionType> stores, DateTime startDate, DateTime endDate)
        { 
            var rtrnList = new List<MonthlySummaryEntity>();     
        
            foreach (var store in stores)
            { 
                var monthly = new MonthlyStoreSummary(store.StoreName, store.State,
                    store.ConnectionString());
            
                var currentStore = monthly.Store(startDate, endDate);
                
                currentStore.Name = store.StoreName;

                rtrnList.Add(currentStore);
            }

            return rtrnList;
        }
    }
}
