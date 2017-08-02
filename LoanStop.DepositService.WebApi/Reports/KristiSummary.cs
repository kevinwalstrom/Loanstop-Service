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
    public class KristiSummary
    {

        private List<StoreConnectionType> Connections;
         
        public KristiSummary(List<StoreConnectionType> connections)
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
                currentStore.LoansPaid = LoansPaid(connection, startDate, endDate);
                currentStore.Undeposited = Undeposited(connection, undepositedDate, undepositedDate);
                currentStore.UndepositedQuickbooks = UndepositedQuickbooks(connection, undepositedDate, undepositedDate);
                currentStore.Bounced = Bounced(connection, startDate, endDate);
                currentStore.NetFees = NetFees(currentStore.LoansPaid, currentStore.LoansMade, currentStore.Bounced);
                currentStore.Expenses = Expenses(connection, startDate, endDate);

                currentStore.CashLogCredits = CashLogCredits(connection, startDate, endDate);
                currentStore.CashLogDebits = CashLogDebits(connection, startDate, endDate);
                //currentStore.CashTransactionsCredits = CashTransactionsCredits(connection, startDate, endDate);
                //currentStore.CashTransactionsDebits = CashTransactionsDebits(connection, startDate, endDate);


                stores.Add(currentStore);
            }

            var loansMade = new List<decimal>();
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

            loansMade.Add(0.00m);
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
                loansPaid = loansPaid.ToArray(),
                undepositedReceivables = undeposited.ToArray(),
                undepositedQuickbooks = undepositedQuickbooks.ToArray(),
                bounced = bounced.ToArray(),
                netFees = netFees.ToArray(),
                expenses = expenses.ToArray(),

                cashLogCredits = cashLogCredits.ToArray(),
                cashLogDebits = cashLogDebits.ToArray(),
                //cashTransactionsCredits = cashTransactionsCredits.ToArray(),
                //cashTransactionsDebits = cashTransactionsDebits.ToArray()

            };

            return rtrnObj;

        }

        public object Backward(DateTime startDate, DateTime endDate)
        {

            List<KristiSummaryClass> stores = new List<KristiSummaryClass>();

            DateTime undepositedDate = DateTime.Today;

            foreach (var connection in Connections)
            {
                var currentStore = new KristiSummaryClass();
                currentStore.LoansMade = LoansMade(connection, startDate, endDate);
                currentStore.LoansPaid = LoansPaid(connection, startDate, endDate);
                currentStore.Undeposited = Undeposited(connection, undepositedDate, undepositedDate);
                currentStore.UndepositedQuickbooks = UndepositedQuickbooks(connection, startDate, endDate);
                currentStore.Bounced = Bounced(connection, startDate, endDate);
                currentStore.NetFees = NetFees(currentStore.LoansPaid, currentStore.LoansMade, currentStore.Bounced);
                currentStore.Expenses = Expenses(connection, startDate, endDate);

                currentStore.CashLogCredits = CashLogCredits(connection, startDate, endDate);
                currentStore.CashLogDebits = CashLogDebits(connection, startDate, endDate);
                //currentStore.CashTransactionsCredits = CashTransactionsCredits(connection, startDate, endDate);
                //currentStore.CashTransactionsDebits = CashTransactionsDebits(connection, startDate, endDate);


                stores.Add(currentStore);
            }

            var loansMade = new List<decimal>();
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

            loansMade.Add(0.00m);
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
                loansPaid = loansPaid.ToArray(),
                undepositedReceivables = undeposited.ToArray(),
                undepositedQuickbooks = undepositedQuickbooks.ToArray(),
                bounced = bounced.ToArray(),
                netFees = netFees.ToArray(),
                expenses = expenses.ToArray(),

                cashLogCredits = cashLogCredits.ToArray(),
                cashLogDebits = cashLogDebits.ToArray(),
                //cashTransactionsCredits = cashTransactionsCredits.ToArray(),
                //cashTransactionsDebits = cashTransactionsDebits.ToArray()

            };

            return rtrnObj;

        }



        public KristiSummaryStoreClass Detail(string category, DateTime startDate, DateTime endDate)
        {

            KristiSummaryStoreClass rtrn = new KristiSummaryStoreClass();

            DateTime undepositedDate = DateTime.Today;

            var connection = Connections.FirstOrDefault();

            var currentStore = new KristiSummaryStoreClass();

            switch (category.ToLower())
            {
                case "loan":
                    rtrn.ledger.amounts = LoansCashLogDetail(connection, startDate, endDate);

                    rtrn.transactions.amounts = LoansTransactionsDetail(connection, startDate, endDate);

                    break;
                case "cashedchecks":

                    rtrn.ledger.amounts = LoansCashLogDetail(connection, startDate, endDate);

                    rtrn.transactions.amounts = LoansTransactionsDetail(connection, startDate, endDate);

                    break;
                case "payments":

                    //rtrn.ledger.amounts = PaymentsCashLogDetail(connection, startDate, endDate);

                   // rtrn.transactions.amounts = PaymentsTransactionsDetail(connection, startDate, endDate);

                    break;

                default:

                    rtrn.ledger.amounts = LoansCashLogDetail(connection, startDate, endDate);

                    rtrn.transactions.amounts = LoansTransactionsDetail(connection, startDate, endDate);

                    break;
            }


            return rtrn;

        }

        #region "summary"
        private decimal LoansMade(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            DateTime tempEndDate = endDate;
            //if (startDate == endDate)
            //    tempEndDate = startDate.AddDays(1);

            if (connection.State.ToLower() == "colorado")
            {
                rtrn = Reports.AmountDisbursed(startDate, tempEndDate);
            }
            else
            {
                rtrn = Reports.AmountDisbursedWy(startDate, tempEndDate);
            }

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
    
        private decimal Undeposited(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {

            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            if (startDate == endDate)
            {
                DateTime theDate = startDate.AddHours(23);
                if (connection.State.ToLower() == "colorado")
                {
                    rtrn = Reports.UndepositedCo(theDate);
                }
                else
                {
                    rtrn = Reports.UndepositedWy(theDate);
                }
            }

            return rtrn;

        }

        private decimal UndepositedQuickbooks(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {

            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            if (startDate == endDate)
            {
                DateTime theDate = startDate.AddHours(23);
                if (connection.State.ToLower() == "colorado")
                {
                    rtrn = Reports.UndepositedCoQuickbooks(theDate);
                }
                else
                {
                    rtrn = Reports.UndepositedWy(theDate);
                }
            }

            return rtrn;

        }

        private decimal Bounced(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            decimal rtrn = 0;

            rtrn = Reports.AmountBounced(startDate, endDate);

            return rtrn;
        }

        private decimal NetFees(decimal loansPaid, decimal amountDisbursed, decimal amountBounced)
        {
            decimal rtrn = 0;

            rtrn = loansPaid - amountDisbursed - amountBounced;

            return rtrn;
        }

        private decimal GrossFees(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            long rtrn = 0;

            //rtrn = Convert.ToInt16(Reports.GrossFees(startDate, endDate));

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
            //rtrn.

            return rtrn;
        }

        private List<CashLogCreditsDebitsEntity> CashLogDebits(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            List<CashLogCreditsDebitsEntity>  rtrn = Reports.CashLogDebits(startDate, endDate);

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
        #endregion

        #region "ledger summary"


        #endregion

        #region "store detail"

        private List<object> LoansCashLogDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            List<object> rtrn = null;

            DateTime tempEndDate = endDate;
            if (startDate == endDate)
                tempEndDate = startDate.AddDays(1);

            rtrn = Reports.LoansCashLogDetail(startDate, tempEndDate);

            return rtrn;
        }

        private List<object> LoansTransactionsDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        {
            var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

            List<object> rtrn = null;

            DateTime tempEndDate = endDate;
            if (startDate == endDate)
                tempEndDate = startDate.AddDays(1);

            rtrn = Reports.LoansTransactionsDetail(startDate, tempEndDate);

            return rtrn;
        }

        //private List<object> PaymentsCashLogDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        //{
        //    var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

        //    List<object> rtrn = null;

        //    DateTime tempEndDate = endDate;
        //    if (startDate == endDate)
        //        tempEndDate = startDate.AddDays(1);

        //    rtrn = Reports.PaymentsCashLogDetail(startDate, tempEndDate);

        //    return rtrn;
        //}

        //private List<object> PaymentsTransactionsDetail(StoreConnectionType connection, DateTime startDate, DateTime endDate)
        //{
        //    var Reports = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

        //    List<object> rtrn = null;

        //    DateTime tempEndDate = endDate;
        //    if (startDate == endDate)
        //        tempEndDate = startDate.AddDays(1);

        //    rtrn = Reports.PaymentsTransactionsDetail(startDate, tempEndDate, connection.State);

        //    return rtrn;
        //}



        #endregion



    }

    public class KristiSummaryClass
    {
        public decimal LoansMade { get; set; }
        public decimal CashedChecks { get; set; }
        public decimal Payments { get; set; }
        public decimal MoneyGram { get; set; }
        

        public decimal LoansPaid { get; set; }
        public decimal Undeposited { get; set; }
        public decimal UndepositedQuickbooks { get; set; }
        
        public decimal Bounced { get; set; }
        public decimal NetFees { get; set; }
        public decimal GrossFees { get; set; }
        public decimal Expenses { get; set; }

        public List<CashLogCreditsDebitsEntity> CashLogCredits { get; set; }
        public List<CashLogCreditsDebitsEntity> CashLogDebits { get; set; }
        public decimal CashTransactionsCredits { get; set; }
        public decimal CashTransactionsDebits { get; set; }

    }

    public class KristiSummaryStoreClass
    {
        public KristiSummaryStoreClass()
        {
            ledger = new StoreLedgerClass();
            transactions = new StoreLedgerClass();
        }

        public StoreLedgerClass ledger { get; set; }

        public StoreLedgerClass transactions { get; set; }

    }

    public class StoreLedgerClass
    {
        public object loans { get; set; }
        public object payments { get; set; }
        public object bounced { get; set; }
        public object colections { get; set; }
        public object checkCashing { get; set; }
        public object moneyGramm { get; set; }
        public object expenses { get; set; }
        public object transfers { get; set; }
        public object depositsWithdrawals { get; set; }
        public object corprate { get; set; }
        public object voided { get; set; }

        public object amounts { get; set; }

    }


}