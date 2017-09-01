using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanStopModel
{
    public partial class Transaction
    {

        public List<PaymentPlanCheck> PaymentPlanChecks { get; set; }

        [NotMapped]
        public bool FeeApplied { get; set; }

        public decimal FinanceCharge
        {
        
            get
            {
                decimal financeCharge = 0;

                if (AmountDispursed == 100)
                    financeCharge = 71.0M;
                else if (AmountDispursed == 200)
                    financeCharge = 142.06M;
                else if (AmountDispursed == 300)
                    financeCharge = 213.06M;
                else if (AmountDispursed == 400)
                    financeCharge = 271.58M;
                else if (AmountDispursed == 500)
                    financeCharge = 292.66M;

                return financeCharge;
            }

        }

        public decimal MonthlyFee
        {

            get
            {
                decimal monthlyFee = 0;

                if (AmountDispursed == 100)
                    monthlyFee = 71.0M / 6;
                else if (AmountDispursed == 200)
                    monthlyFee = 142.06M / 6;
                else if (AmountDispursed == 300)
                    monthlyFee = 213.06M / 6;
                else if (AmountDispursed == 400)
                    monthlyFee = 271.58M / 6;
                else if (AmountDispursed == 500)
                    monthlyFee = 292.66M / 6;

                return monthlyFee;
            }

        }

        public DateTime FinalPaymentDay
        {

            get
            {
                DateTime finalPaymentDay = DateTime.Now;

                if (PaymentPlanChecks != null)
                {
                    foreach (var p in PaymentPlanChecks)
                    {
                        if (p.Status != "Void")
                        {
                            if (p.HoldDate.HasValue)
                                if ((DateTime)p.HoldDate > finalPaymentDay)
                                {
                                    finalPaymentDay = (DateTime)p.HoldDate;
                                }
                                else if (p.DateDue.HasValue)
                                    finalPaymentDay = (DateTime)p.DateDue;
                        }
                    }
                }
                return finalPaymentDay;
            }
        }

        public DateTime FinalPayoffPaymentDay
        {

            get
            {
                DateTime finalPaymentDay = DateTime.Now;

                if (PaymentPlanChecks != null)
                {
                    foreach (var p in PaymentPlanChecks.OrderBy(o => o.DateDue))
                    {
                        if (p.Status != "Void")
                        {
                            if (p.DateDue.HasValue)
                                finalPaymentDay = (DateTime)p.DateDue;
                        }
                    }
                }
                return finalPaymentDay;
            }
        }    

        public decimal SumAmountPaidPaymentPlaychecks
        {
            get
            {
                decimal total = 0;
                
                if (PaymentPlanChecks != null)
                {
                    foreach (var item in this.PaymentPlanChecks)
                    {
                        total = total + item.AmountPaid;
                    }
                }

                return total;
            }
        }

        public bool bPaidNSF
        {
            get
            {
                bool rtrnValue = false;

                if (this.PaymentPlanChecks != null)
                {

                    foreach (var pcc in this.PaymentPlanChecks)
                    {
                        if (pcc.Status == "Paid NSF")
                        {

                            rtrnValue = true;
                        }
                    }

                    return rtrnValue;
                }
                else
                    return rtrnValue;

            }
        }

        public bool IsBounced
        {
            get
            {
                bool rtrnValue = false;

                if (this.PaymentPlanChecks != null)
                {
                    foreach (var pcc in this.PaymentPlanChecks)
                    {
                        if (pcc.Status == "Bounced" || pcc.Status == "Partial NSF")
                        {

                            rtrnValue = true;
                        }


                    }

                    return rtrnValue;

                }
                else
                    return rtrnValue;

            }
        }

    }
}
