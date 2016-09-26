using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.Reports;


namespace LoanStop.Services.WebApi.Reports
{
    public class ColoradoYearly
    {

        private string connectionString;

        public ColoradoYearly(string ConnectionString)
        {
            connectionString = ConnectionString;
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public ColoradoYearlyReport YearlyReport(DateTime startDate, DateTime endDate)
        {

            var report = new LoanStop.DBCore.Repository.ColoradoAudit(connectionString);

            var question1 = report.TotalLoans(startDate, endDate);
            var question2 = report.ByAmount(startDate, endDate);
            var question3 = report.LoansOutstanding(startDate, endDate);
            var question4 = report.Rescinded(startDate, endDate);
            var question6 = report.ActiveMilitary(startDate, endDate);
            var question7a = report.Defaulted(startDate, endDate);
            var question7b = report.AmountRecovered(startDate, endDate);
            var question7c = report.ChargeOffs(startDate, endDate);
            var question7d = report.NSFFees(startDate, endDate);
            var question8a = report.BySSNumber(startDate, endDate);
            //var question8b = report.CustomersByNumberLoans(startDate, endDate);
            var question9 = report.question9(startDate, endDate);
            var question10 = report.question10(startDate, endDate);

            var rtrn = new ColoradoYearlyReport()
            {
                totalsLoans = question1,                                // question 1
                byAmount = question2,                                   // question 2
                loansOutstanding = question3,                           // question 3
                rescinded = question4,                                  // question 4
                activeMilitary = question6,                             // question 6
                defaulted = question7a,                                 // question 7
                amountRecovered = question7b,                           // question 7
                amountchargeOffs = question7c,                          // question 7
                nsfFees = question7d,                                   // question 7
                bySSNumber = question8a,                                // question 8
                //loansPerCustomer = question8b,                          // question 8
                question9 = question9,                                  // question 9 
                question10 = question10                                 // question 10 

            };

            return rtrn;
        }
        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}