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
    public class Summary
    {

        private List<StoreConnectionType> Connections;

        public Summary(List<StoreConnectionType> connections)
        {
            this.Connections = connections;
        }

        public object Execute(DateTime startDate, DateTime endDate)
        {

            List<KristiSummaryClass> stores = new List<KristiSummaryClass>();

            DateTime undepositedDate = DateTime.Today;

            foreach (var connection in Connections)
            {

                var currentStore = new KristiSummaryClass();
                currentStore.LoansMade = LoansMade(connection, startDate, endDate);
                currentStore.CashedChecks = CashedChecks(connection, startDate, endDate);
                currentStore.Payments = Payments(connection, startDate, endDate);
                currentStore.LoansPaid = LoansPaid(connection, startDate, endDate);
                currentStore.Bounced = Bounced(connection, startDate, endDate);
                currentStore.Expenses = Expenses(connection, startDate, endDate);
                currentStore.CashLogCredits = CashLogCredits(connection, startDate, endDate);
                currentStore.CashLogDebits = CashLogDebits(connection, startDate, endDate);
                stores.Add(currentStore);

            }

            var loansMade = new List<decimal>();
            var cashedChecks = new List<decimal>();
            var payments = new List<decimal>();
            var moneyGram = new List<decimal>();

            var loansPaid = new List<decimal>();
            var undeposited = new List<decimal>();
            var undepositedQuickbooks = new List<decimal>();
            var bounced = new List<decimal>();
            var netFees = new List<decimal>();
            var expenses = new List<decimal>();

            var cashLogCredits = new List<List<CashLogCreditsDebitsEntity>>();
            var cashLogDebits = new List<List<CashLogCreditsDebitsEntity>>();
            var cashTransactionsCredits = new List<decimal>();
            var cashTransactionsDebits = new List<decimal>();

            int index = 1;

            loansMade.Add(0.00m);
            cashedChecks.Add(0.00m);
            payments.Add(0.00m);
            moneyGram.Add(0.00m);

            loansPaid.Add(0.00m);
            undeposited.Add(0.00m);
            undepositedQuickbooks.Add(0.00m);
            bounced.Add(0.00m);
            netFees.Add(0.00m);
            expenses.Add(0.00m);

            cashLogCredits.Add(new List<CashLogCreditsDebitsEntity>());
            cashLogDebits.Add(new List<CashLogCreditsDebitsEntity>());
            //cashTransactionsCredits.Add(0.00m);
            //cashTransactionsDebits.Add(0.00m);


            foreach (var store in stores)
            {
                loansMade.Add(store.LoansMade);
                cashedChecks.Add(store.CashedChecks);
                payments.Add(store.Payments);
                moneyGram.Add(store.Payments);

                loansPaid.Add(store.LoansPaid);
                undeposited.Add(store.Undeposited);
                undepositedQuickbooks.Add(store.UndepositedQuickbooks);
                bounced.Add(store.Bounced);
                netFees.Add(store.NetFees);
                expenses.Add(store.Expenses);

                cashLogCredits.Add(store.CashLogCredits);
                cashLogDebits.Add(store.CashLogDebits);
                cashTransactionsCredits.Add(store.CashTransactionsCredits);
                cashTransactionsDebits.Add(store.CashTransactionsDebits);
            }

            loansMade.Add(loansMade.Sum());
            cashedChecks.Add(cashedChecks.Sum());
            payments.Add(payments.Sum());
            moneyGram.Add(moneyGram.Sum());

            loansPaid.Add(loansPaid.Sum());
            undeposited.Add(undeposited.Sum());
            undepositedQuickbooks.Add(undepositedQuickbooks.Sum());
            bounced.Add(bounced.Sum());
            netFees.Add(netFees.Sum());
            expenses.Add(expenses.Sum());

            //cashLogCredits.Add(cashLogCredits.Sum());
            //cashLogDebits.Add(cashLogDebits.Sum());
            //cashTransactionsCredits.Add(cashTransactionsCredits.Sum());
            //cashTransactionsDebits.Add(cashTransactionsDebits.Sum());

            var rtrnObj = new
            {
                newLoansMade = loansMade.ToArray(),
                cashedChecks = cashedChecks.ToArray(),
                payments = payments.ToArray(),
                moneyGram = moneyGram.ToArray(),

                loansPaid = loansPaid.ToArray(),
                bounced = bounced.ToArray(),

                cashLogCredits = cashLogCredits.ToArray(),
                cashLogDebits = cashLogDebits.ToArray(),
                //cashTransactionsCredits = cashTransactionsCredits.ToArray(),
                //cashTransactionsDebits = cashTransactionsDebits.ToArray()

            };

            return rtrnObj;

        }

        private decimal LoansMade(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceSummary(connection.ConnectionString());

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

        private decimal CashedChecks(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashedChecks(startDate, endDate);

            return rtrn;
        }

        private decimal Payments(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.DailyBalanceSummary(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.Payments(startDate, endDate, connection.State);

            return rtrn;
        }

        private decimal LoansPaid(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            if (connection.State.ToLower() == "colorado")
            {
                rtrn = Reports.AmountReceived(startDate, endDate);
            }
            else
            {
                rtrn = Reports.AmountReceivedWy(startDate, endDate);
            }

            rtrn += Reports.AmountPaid(startDate, endDate);

            return rtrn;
        }

        private decimal Bounced(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.AmountBounced(startDate, endDate);

            return rtrn;
        }

        private decimal Expenses(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.Expenses(startDate, endDate);

            return rtrn;
        }

        private List<CashLogCreditsDebitsEntity> CashLogCredits(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            List<CashLogCreditsDebitsEntity> rtrn = Reports.CashLogCredits(startDate, endDate, connection.State);

            return rtrn;
        }

        private List<CashLogCreditsDebitsEntity> CashLogDebits(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            List<CashLogCreditsDebitsEntity> rtrn = Reports.CashLogDebits(startDate, endDate);

            Debug.Print(connection.StoreName, rtrn);

            return rtrn;
        }

        private decimal CashTransactionsCredits(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashTransactionsCredits(startDate, endDate);

            return rtrn;
        }

        private decimal CashTransactionsDebits(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.CashTransactionsDebits(startDate, endDate);

            return rtrn;
        }


    }
}