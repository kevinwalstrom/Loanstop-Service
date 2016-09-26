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
    public class StoreSummary
    {
        private StoreConnectionType Connection;

        public StoreSummary(StoreConnectionType connection)
        {
            this.Connection = connection;
        }


        public KristiSummaryStoreClass Execute(DateTime startDate, DateTime endDate)
        {

            KristiSummaryStoreClass rtrn = new KristiSummaryStoreClass();

            DateTime undepositedDate = DateTime.Today;

            var currentStore = new KristiSummaryStoreClass();

            rtrn.ledger.loans = LoansCashLog(this.Connection, startDate, endDate);
            rtrn.transactions.loans = LoansTransactions(this.Connection, startDate, endDate);

            rtrn.ledger.checkCashing = CashedCheckesCashLog(this.Connection, startDate, endDate);
            rtrn.transactions.checkCashing = CashedChecksTransactions(this.Connection, startDate, endDate);

            rtrn.ledger.payments = LedgerPayments(this.Connection, startDate, endDate);
            rtrn.transactions.payments = TransactionsPayments(this.Connection, startDate, endDate);

            return rtrn;

        }

        private decimal LoansCashLog(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashLogLoans(startDate, endDate);

            return rtrn;
        }

        private decimal CashedCheckesCashLog(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashLogCashedChecks(startDate, endDate);

            return rtrn;
        }

        private decimal LoansTransactions(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;


            if (connection.State.ToLower() == "colorado")
            {
                rtrn = Reports.AmountDisbursed(startDate, endDate);
            }
            else
            {
                rtrn = Reports.AmountDisbursedWy(startDate, endDate);
            }

            return rtrn;
        }

        private decimal CashedChecksTransactions(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashedChecks(startDate, endDate);

            return rtrn;
        }

        private decimal LedgerPayments(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashLogPayments(startDate, endDate);

            return rtrn;
        }

        private decimal TransactionsPayments(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceStoreSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.TransactionPayments(startDate, endDate);

            return rtrn;
        }



    }
}