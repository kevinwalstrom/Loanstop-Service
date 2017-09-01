using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Reports
{
    public class Outsourced
    {
        private List<StoreConnectionType> Connections;
        private Colorado colorado;
        private Wyoming wyoming;

        public Outsourced(List<StoreConnectionType> connections)
        {
            this.Connections = connections;
        }

        public object Execute(DateTime theDate)
        {
            var rtrn = new List<object>();

            DateTime undepositedDate = DateTime.Today;
            var endDate = theDate.AddMonths(1);
            string monthName = theDate.ToString("MMMM");
            int year = theDate.Year;

            // 1) - Loans Receivable

            foreach (var connection in Connections)
            {
                decimal storeCollect = 0;
                decimal outsourced = 0;

                var report = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

                storeCollect = report.StoreCollect(theDate, endDate);
                outsourced = report.Outsourced(theDate, endDate);

                rtrn.Add(new { month= monthName, store = connection.StoreName, storeCollect = storeCollect, outsourced = outsourced, year = year });
            }


            return rtrn;

        }

    }
}