using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;

namespace LoanStop.Services.WebApi.Reports
{
    public class StoreDetail
    {

        private StoreConnectionType Connection;

        public StoreDetail(StoreConnectionType connection)
        {
            this.Connection = connection;
        }


        public KristiSummaryStoreClass Execute(string category, DateTime startDate, DateTime endDate)
        {

            KristiSummaryStoreClass rtrn = new KristiSummaryStoreClass();

            DateTime undepositedDate = DateTime.Today;

            var currentStore = new KristiSummaryStoreClass();

            switch (category.ToLower())
            {
                case "loan":
                    {
                        rtrn.ledger.loans = LoansCashLogDetail(this.Connection, startDate, endDate);

                        rtrn.transactions.loans = LoansTransactionsDetail(this.Connection, startDate, endDate);

                        break;
                    }
                case "cashedchecks":
                    {
                        rtrn.ledger.checkCashing = CashedCheckesCashLogDetail(this.Connection, startDate, endDate);

                        rtrn.transactions.checkCashing = CashedChecksTransactionsDetail(this.Connection, startDate, endDate);

                        break;
                    }
                case "payments":
                    {
                        rtrn.ledger.checkCashing = CashedCheckesCashLogDetail(this.Connection, startDate, endDate);

                        rtrn.transactions.checkCashing = CashedChecksTransactionsDetail(this.Connection, startDate, endDate);

                        break;
                    }
                default:
                    {
                        break;

                    }
            }

            return rtrn;

        }

        /// ------
        private List<object> LoansCashLogDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceDetail(connection.ConnectionString());

            List<object> rtrn = null;

            DateTime tempEndDate = endDate;
            if (startDate == endDate)
                tempEndDate = startDate.AddDays(1);

            rtrn = Reports.LoansCashLogDetail(startDate, tempEndDate);

            return rtrn;
        }

        /// ------
        private List<object> LoansTransactionsDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceDetail(connection.ConnectionString());

            List<object> rtrn = null;

            DateTime tempEndDate = endDate;
            if (startDate == endDate)
                tempEndDate = startDate.AddDays(1);

            rtrn = Reports.LoansTransactionsDetail(startDate, tempEndDate);

            return rtrn;
        }

        /// ------
        private List<object> CashedCheckesCashLogDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceDetail(connection.ConnectionString());

            List<object> rtrn = null;

            rtrn = Reports.CashedChecksCashLogDetail(startDate, endDate);

            return rtrn;
        }

        /// ------
        private List<object> CashedChecksTransactionsDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceDetail(connection.ConnectionString());

            List<object> rtrn = null;

            rtrn = Reports.CashedChecksTransactionsDetail(startDate, endDate);

            return  rtrn;
        }


    } // end class
} // end namespace