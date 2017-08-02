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

        public object Execute(List<StoreConnectionType> stores, DateTime startDate, DateTime endDate)
        { 
            var rtrnList = new List<MonthlySummaryEntity>();
            int newAccounts = 0;
            int numberOfLoans = 0;
            decimal amountDisbursed = 0;
            decimal amountReceived = 0;
            decimal amountBounced = 0;
            decimal amountPaid = 0;
            decimal netFees = 0;
            decimal checkCashReceived = 0;
            decimal checkCashDisbursed = 0;
            decimal checkCashBounced = 0;
            decimal checkCashPaid = 0;
            decimal checkCashNetFees = 0;
            decimal debitCardReceived = 0;
            decimal debitCardDisbursed = 0;
            decimal debitCardNet = 0;
            decimal totalNet = 0;


            foreach (var store in stores)
            { 
                var monthly = new MonthlyStoreSummary(store.StoreName, store.State,
                    store.ConnectionString());
            
                var currentStore = monthly.Store(startDate, endDate);
                
                currentStore.Name = store.StoreName;

                newAccounts += currentStore.NewAccounts;
                numberOfLoans += currentStore.NumberOfLoans;
                amountDisbursed += currentStore.AmountDisbursed;
                amountReceived += currentStore.AmountReceived;
                amountBounced += currentStore.AmountBounced;
                amountPaid += currentStore.AmountPaid;
                netFees += currentStore.NetFees;
                checkCashReceived += currentStore.CheckCashReceived;
                checkCashDisbursed += currentStore.CheckCashDisbursed;
                checkCashBounced += currentStore.CheckCashBounced;
                checkCashPaid += currentStore.CheckCashPaid;
                checkCashNetFees += currentStore.CheckCashNetFees;
                debitCardReceived += currentStore.DebitCardReceived;
                debitCardDisbursed += currentStore.DebitCardDisbursed;
                debitCardNet += currentStore.DebitCardNet;
                totalNet += currentStore.TotalNet;

                rtrnList.Add(currentStore);
            }

            var rtrnOjb = new
            {
                stores = rtrnList,
                newAccounts = newAccounts,
                numberOfLoans = numberOfLoans,
                amountDisbursed = amountDisbursed,
                amountReceived = amountReceived,
                amountBounced = amountBounced,
                amountPaid = amountPaid,
                netFees = netFees,
                checkCashReceived = checkCashReceived,
                checkCashDisbursed = checkCashDisbursed,
                checkCashBounced = checkCashBounced,
                checkCashPaid = checkCashPaid,
                checkCashNetFees = checkCashNetFees,
                debitCardReceived = debitCardReceived,
                debitCardDisbursed = debitCardDisbursed,
                debitCardNet = debitCardNet,
                totalNet = totalNet
            };


            return rtrnOjb;
        }
    }
}
