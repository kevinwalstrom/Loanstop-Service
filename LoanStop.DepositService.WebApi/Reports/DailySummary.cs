using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Reports
{
    public class DailySummary
    {
        private List<StoreConnectionType> Connections;
        private Colorado colorado;
        private Wyoming wyoming;

        public DailySummary(List<StoreConnectionType> connections)
        {
            this.Connections = connections;
        }

        public object Execute(DateTime theDate)
        {
            var rtrn = new List<object>();

            DateTime undepositedDate = DateTime.Today;

            // 1) - Loans Receivable

            foreach (var connection in Connections)
            {
                List<ExportEntity> loansMade = null;
                List<ExportEntity> payments = null;
                List<ExportEntity> bounced = null;

                colorado = new Colorado(connection.ConnectionString());
                wyoming = new Wyoming(connection.ConnectionString());
                loansMade = LoansMade(connection, theDate, theDate);
                payments = principalPayments(connection, theDate, theDate);
                bounced = principalBounced(connection, theDate, theDate);

                var summary = new SummaryDeltas(connection.ConnectionString());

                summary.Insert(theDate, loansMade.Sum(s => s.amount), payments.Sum(s => s.amount), bounced.Sum(s => s.amount));

                rtrn.Add(new { store = connection.StoreName, loans = loansMade, principalPayments = payments, principalBounced = bounced });
            }

            return rtrn;

        }

        /// - 
        private List<ExportEntity> LoansMade(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {

            List<ExportEntity> rtrn = new List<ExportEntity>();

            if (connection.State.ToLower() == "colorado")
            {
                rtrn = colorado.loaned(startDate, endDate);
            }
            else
            {
                //rtrn = wyoming.loaned(startDate, endDate);
            }

            return rtrn;
        }

        /// - 
        private List<ExportEntity> principalPayments(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn = new List<ExportEntity>();

            if (connection.State.ToLower() == "colorado")
            {
                rtrn = colorado.principalPayments(startDate, endDate);
            }
            else
            {
                //rtrn = Reports.AmountDisbursedWy(startDate, tempEndDate);
            }

            return rtrn;
        }

        private List<ExportEntity> principalBounced(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn = new List<ExportEntity>();

            if (connection.State.ToLower() == "colorado")
            {
                rtrn = colorado.principalBounced(startDate, endDate);
            }
            else
            {
                //rtrn = Reports.AmountDisbursedWy(startDate, tempEndDate);
            }

            return rtrn;
        }

    }
}