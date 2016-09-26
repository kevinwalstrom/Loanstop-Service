using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;

namespace LoanStop.Reports
{
    public class MonthlyStoreSummary : BaseActivity
    {
        private string state;
        
        public MonthlyStoreSummary(string store, string state, string connectionString) : base(store, connectionString)
        {  this.state = state;}

        public MonthlySummaryEntity Store(DateTime startDate, DateTime endDate)
        { 

            var rep = new Repository.Reports(connectionString);

            var summary = new MonthlySummaryEntity();

            summary.NewAccounts = rep.NewAccounts(startDate, endDate);
            if (this.state.ToLower() == "colorado")
            { 
                summary.NumberOfLoans = rep.NumberOfLoans(startDate, endDate);
            }
            else
            { 
                summary.NumberOfLoans = rep.NumberOfLoansWy(startDate, endDate);
            }

            if (this.state.ToLower() == "colorado")
            { 
                summary.AmountDisbursed = rep.AmountDisbursed(startDate, endDate);
            }
            else
            { 
                summary.AmountDisbursed = rep.AmountDisbursedWy(startDate, endDate);
            }
            
            if (this.state.ToLower() == "colorado")
            { 
                summary.AmountReceived = rep.AmountReceived(startDate, endDate);
            }
            else
            { 
                summary.AmountReceived = rep.AmountReceivedWy(startDate, endDate);
            }
            
            summary.AmountBounced = rep.AmountBounced(startDate, endDate);
            summary.AmountPaid = rep.AmountPaid(startDate, endDate);
            summary.CheckCashReceived = rep.CheckCashReceived(startDate, endDate);
            summary.CheckCashDisbursed = rep.CheckCashDisbursed(startDate, endDate);
            summary.CheckCashBounced = rep.CheckCashBounced(startDate, endDate);
            summary.CheckCashPaid = rep.CheckCashPaid(startDate, endDate);
            summary.DebitCardReceived = rep.DebitCardReceived(startDate, endDate);
            summary.DebitCardDisbursed = rep.DebitCardDisbursed(startDate, endDate);

            summary.NetFees = summary.AmountReceived - summary.AmountDisbursed - summary.AmountBounced + summary.AmountPaid;
            summary.CheckCashNetFees = summary.CheckCashReceived - summary.CheckCashDisbursed - summary.CheckCashBounced + summary.CheckCashPaid;
            summary.DebitCardNet = summary.DebitCardReceived - summary.DebitCardDisbursed;
            
            decimal num = summary.AmountBounced  - summary.AmountPaid;
            decimal den = summary.AmountReceived - summary.AmountDisbursed;

            if (den != 0) 
                summary.NSFRate = num / den * 100;
            else
                summary.NSFRate = 0;

            num = summary.CheckCashBounced  - summary.CheckCashPaid;
            den = summary.CheckCashReceived - summary.CheckCashDisbursed;

            if (den != 0) 
                summary.CheckCashNSFRate = num / den * 100;
            else
                summary.CheckCashNSFRate = 0;

            summary.TotalNet = summary.NetFees + summary.CheckCashNetFees + summary.DebitCardNet;

            return summary;
        }

    }
}
