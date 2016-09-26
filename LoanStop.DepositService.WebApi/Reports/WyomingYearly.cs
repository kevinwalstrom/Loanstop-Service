using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Services.WebApi;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.Services.WebApi.Reports
{
    public class WyomingYearly
    {
    
        private string connectionString;

        public WyomingYearly(string ConnectionString)
        {
            connectionString = ConnectionString;
        }

        public object YearlyReport(DateTime startDate, DateTime endDate)
        {

            var report = new LoanStop.DBCore.Repository.WyomingAudit(connectionString);

            var rtrn = new {
                totals = report.Totals(startDate, endDate),
                byAmount = report.ByAmount(startDate, endDate),
                bySSNumber = report.BySSNumber(startDate, endDate),
                PostDatedCheckLoanSummary = report.PostDatedCheckLoanSummary(startDate, endDate),
                ChargeOffs = report.ChargeOffs(startDate, endDate),
                AmountRecovered = report.AmountRecovered(startDate, endDate),
                Rescinded = report.Rescinded(startDate, endDate),
                Defaulted = report.Defaulted(startDate, endDate),
                ExtendedPaymentPlan = report.ExtendedPaymentPlan(startDate, endDate),
                DefaultPaymentPlan = report.DefaultPaymentPlan(startDate, endDate),
                DefaultPaymentPlanAPR = report.DefaultPaymentPlanAPR(startDate, endDate)
            };

            return  rtrn;
        }
    
    }
}