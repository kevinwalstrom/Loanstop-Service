using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Reports
{
    public class Collections
    {
        private StoreConnectionType Connection;
        private Colorado colorado;
        private Wyoming wyoming;

        public Collections(StoreConnectionType connection)
        {
            this.Connection = connection;
        }

        public object Execute(DateTime theDate)
        {
            var rtrn = new List<object>();

            DateTime undepositedDate = DateTime.Today;
            var endDate = theDate.AddMonths(1);
            string monthName = theDate.ToString("MMMM");
            int year = theDate.Year;

            // 1) - Loans Receivable

            decimal storeCollect = 0;
            decimal outsourced = 0;

            var report = new LoanStop.DBCore.Repository.Reports(this.Connection.ConnectionString());

            storeCollect = report.StoreCollect(theDate, endDate);
            outsourced = report.Outsourced(theDate, endDate);

            rtrn.Add(new { month = monthName, store = this.Connection.StoreName, storeCollect = storeCollect, outsourced = outsourced, year = year });


            return rtrn;

        }

    }
}