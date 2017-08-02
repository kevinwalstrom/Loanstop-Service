using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStopModel;
using Microsoft.VisualBasic;

namespace LoanStop.Services.WebApi.Business.Colorado
{
    public static class LoanUtilities
    {

        public static decimal OrignationFee(decimal amountDispursed)
        {
            if (amountDispursed <= 300)
            {
                return amountDispursed * 0.2m;
            }
            else
            {
                return (300 * 0.2m) + ((amountDispursed - 300m) * 0.075m);
            }
        }

        public static decimal Interest(decimal amountDispursed)
        {
            decimal rtrn = 0m;

            if (amountDispursed == 100m)
            {
                rtrn = 71.0m - (ServiceFee(amountDispursed) * 5) - OrignationFee(amountDispursed);
            }
            else if (amountDispursed == 200m)
            {
                rtrn = 142.06m - (ServiceFee(amountDispursed) * 5) - OrignationFee(amountDispursed);
            }
            else if (amountDispursed == 300m)
            {
                rtrn = 213.06m - (ServiceFee(amountDispursed) * 5) - OrignationFee(amountDispursed);
            }
            else if (amountDispursed == 400m)
            {
                rtrn = 271.58m - (ServiceFee(amountDispursed) * 5) - OrignationFee(amountDispursed);
            }
            else if (amountDispursed == 500m)
            {
                rtrn = 292.66m - (ServiceFee(amountDispursed) * 5) - OrignationFee(amountDispursed);
            }

            return rtrn;

        }

        public static decimal ServiceFee(decimal amountDispursed)
        {
            decimal rtrn = 0m;
            rtrn = Math.Truncate(amountDispursed / 100m) * 7.5m;
            if (rtrn > 30) {
                rtrn = 30;
            }
            return rtrn;
        }

        public static decimal NSFFee(List<PaymentPlanCheck> ppcs)
        {
            decimal rtrn = 0m;

            foreach(var payment in ppcs)
            {
                if (payment.Status == "Bounced" || payment.Status == "Paid NSF" || payment.Status == "Partial NSF")
                {
                    return 25m;
                }
            }

            return rtrn;
        }

        public static decimal OrignationEarned(Transaction trans, DateTime theDate)
        {
            decimal rtrn = 0m;

            int daysOfLoan = DaysOfLoan(trans);

            int daysOutstanding = DaysOutstanding(trans, theDate);

            var orignationFee = OrignationFee(trans.AmountDispursed);

            if ((orignationFee / daysOfLoan) * daysOutstanding > orignationFee)
               rtrn = orignationFee;
            else
               rtrn = (orignationFee / daysOfLoan) * daysOutstanding;

            return rtrn;

        }

        public static decimal AppliedInterest(Transaction trans, DateTime theDate)
        {

            int daysOfLoan = DaysOfLoan(trans);

            int daysOutstanding = DaysOutstanding(trans, theDate);

            return (Interest(trans.AmountDispursed) / daysOfLoan) * daysOutstanding;
        }

        public static decimal AppliedServiceFees(Transaction trans, DateTime theDate)
        {
            decimal rtrn = 0m;

            var StartingDate = trans.TransDate.Value.AddDays(30);

            int differenceInMonths =Convert.ToInt16( Math.Truncate(GetDifferenceInMonths(StartingDate, theDate)));

            if (ServiceFee(trans.AmountDispursed) * 5 < differenceInMonths * ServiceFee(trans.AmountDispursed))
                rtrn = ServiceFee(trans.AmountDispursed) * 5;
            else
                rtrn = differenceInMonths * ServiceFee(trans.AmountDispursed);

            return rtrn;
        }

        public static int DaysOfLoan(Transaction trans)
        {
            return Convert.ToInt16((trans.TransDate.Value.Date - trans.FinalPaymentDay.Date).TotalDays);
        }

        public static int DaysOutstanding(Transaction trans, DateTime theDate)
        {
            return Convert.ToInt16((trans.TransDate.Value.Date - theDate).TotalDays);
        }


        public static double GetDifferenceInMonths(DateTime startdate, DateTime enddate)
        {
            int RtrnNumb = 0;
            DateTime currentDate = startdate;

            while (currentDate >= enddate) {
                switch (currentDate.Month)
                {
                    case 1:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 2:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 3:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 4:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 5:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 6:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 7:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 8:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 9:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 10:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 11:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                    case 12:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= enddate) RtrnNumb = RtrnNumb + 1;
                        break;
                }

            }

            return RtrnNumb;
        }

        /// <summary>
        public static decimal SumOfPayments(List<PaymentPlanCheck> ppcs)
        {
            decimal rtrn = 0;

            foreach (var payment in ppcs)
            {
                rtrn = rtrn + payment.AmountPaid;
            }

            return rtrn;

        }

        public static decimal SumOriginalPayments(List<PaymentPlanCheck> ppcs)
        {
            decimal rtrn = 0;

            foreach (var payment in ppcs)
            {
                rtrn = rtrn + decimal.Parse(payment.OrignalAmount);
            }

            return rtrn;

        }

        public static decimal PayoffAmount(Transaction trans, DateTime theDate)
        {
            //double interest;
            decimal balance = 0m;
            decimal totalPayments = 0m;
            decimal orignationFee = 0m;
            decimal totalFee = 0m;
            decimal totalOrignalPayments = 0m;

            if (((DateTime)trans.TransDate - theDate).TotalDays <= 1)
            {
                balance = trans.AmountDispursed;
            }
            else
            {

                orignationFee = OrignationFee(trans.AmountDispursed);
                totalOrignalPayments = SumOriginalPayments(trans.PaymentPlanChecks);
                totalPayments = SumOfPayments(trans.PaymentPlanChecks);
                if (NSFFee(trans.PaymentPlanChecks) > 0)

                {
                    totalOrignalPayments = totalOrignalPayments + NSFFee(trans.PaymentPlanChecks);
                    totalFee = NSFFee(trans.PaymentPlanChecks);
                }

                //double amountFinanced = 0;
                int daysOutstanding = 0;
                int daysOfLoan = 0;
                decimal serviceFeeEarned = 0m;
                decimal serviceFee = 30m;
                
                serviceFee = ServiceFee(trans.AmountDispursed);

                var amountFinanced = Convert.ToDouble(trans.AmountDispursed);

                var interest = (Financial.Pmt((0.45 / 12), 6, (double)amountFinanced) * -6D) - (double)amountFinanced;

                DateTime startingDate = ((DateTime)trans.TransDate).AddDays(30);

                daysOutstanding = DaysOutstanding(trans, theDate);
                daysOfLoan = DaysOfLoan(trans);

                var differenceInMonths = GetDifferenceInMonths(startingDate, theDate);

                serviceFeeEarned = Convert.ToDecimal(Math.Truncate(differenceInMonths)) * serviceFee;

                orignationFee = (orignationFee / daysOfLoan) * daysOutstanding;

                if (amountFinanced <= 300)
                {
                    if (((DateTime)trans.TransDate - theDate).TotalDays <= 31)
                        balance = Convert.ToDecimal(amountFinanced) + orignationFee + ((Convert.ToDecimal(interest) / daysOfLoan) * daysOutstanding + serviceFeeEarned) - totalPayments + totalFee;
                    else
                        balance = Convert.ToDecimal(amountFinanced) + orignationFee + ((Convert.ToDecimal(interest) / daysOfLoan) * daysOutstanding + serviceFeeEarned) - totalPayments + totalFee;
                }
                else
                    balance = Convert.ToDecimal(amountFinanced) + orignationFee + ((Convert.ToDecimal(interest) / daysOfLoan) * daysOutstanding + serviceFeeEarned) - totalPayments + totalFee;

                if (balance > totalOrignalPayments - totalPayments)
                {
                    balance = totalOrignalPayments - totalPayments;
                }
            }

            if (trans.Status == "Closed")
            {
                balance = 0m;
            }


            return balance;

        }


    }





}