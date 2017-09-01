using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Reports
{
    public class AverageLoanAmount
    {
        private List<StoreConnectionType> Connections;
        private Colorado colorado;
        private Wyoming wyoming;

        public AverageLoanAmount(List<StoreConnectionType> connections)
        {
            this.Connections = connections;
        }

        public object Colorado(DateTime theDate)
        {
            var tempAverageAmount = new List<decimal>();

            var endDate = theDate.AddYears(1);

            foreach (var connection in Connections)
            {

                var report = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

                tempAverageAmount.Add(report.AverageLoanAmountColorado(theDate, endDate));

            }

            return tempAverageAmount.Average();

        }


        public object Wyoming(DateTime theDate)
        {
            var tempAverageAmount = new List<decimal>();

            var endDate = theDate.AddYears(1);

            foreach (var connection in Connections)
            {
                var report = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

                tempAverageAmount.Add(report.AverageLoanAmountColorado(theDate, endDate));
            }


            return tempAverageAmount.Average();

        }



    }
}