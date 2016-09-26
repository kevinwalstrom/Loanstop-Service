using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Business.Wyoming
{
    
    //IF Status in (Closed, Bounced PP, Default PP, Paid Default PP) AND either date_cleared or a date_returned within one year.  NOT ELIGIBLE (yellow)
    //IF no status or Closed, Bounced PP, Default PP, Paid Default PP with a date_cleared or date_returned within the last year.  ELIGIBLE  (green)
    //    examples:
    //        Cyndi/Heath Brainard -eliglable
    //        RAY/ANGELINE BIRKLE -not eligible cause "Closed" within 1 year exists
    public class PaymentPlan
    {

        public string ConnectionString { get; set; }

        public bool IsPaymentPlanEnabled(string ssNumber)
        {
            bool rtrn = true;

            Repository.Transaction transRepository = new Repository.Transaction(ConnectionString);

            var transactionList = transRepository.GetBySSNumber(ssNumber);

            foreach (var transaction in transactionList)
            {
                var notEligibleList = new string[] {"Closed", "Bounced PP", "Default PP", "Paid Default PP"};

                if (notEligibleList.Contains(transaction.Status))
                {
                    if (transaction.DateCleared.HasValue)
                    {
                        if (transaction.DateCleared.Value.AddYears(1) > DateTime.Now && transaction.Issuer != "pre pmt plan")
                        {
                            rtrn = false;
                        }
                    }

                    if (transaction.DateReturned.HasValue)
                    {
                        if (transaction.DateReturned.Value.AddYears(1) > DateTime.Now && transaction.Issuer != "pre pmt plan")
                        {
                            rtrn = false;
                        }
                    }
                }
            }

            return rtrn;
        }


        public bool IsPaymentPlanEnabled(ICollection<LoanStopModel.Transaction> list)
        {
            bool rtrn = true;

            foreach (var transaction in list)
            {
                var notEligibleList1 = new string[] { "Bounced PP", "Default PP", "Paid Default PP" };
                var notEligibleList2 = new string[] { "Closed" };

                if (notEligibleList1.Contains(transaction.Status))
                {
                    if (transaction.DateReturned.Value.AddYears(1) > DateTime.Now)
                    {
                        rtrn = false;
                    }
                }

                if (notEligibleList2.Contains(transaction.Status))
                {
                    if (transaction.DateCleared.Value.AddYears(1) > DateTime.Now)
                    {
                        rtrn = false;
                    }
                }

            }

            return rtrn;
        }

    
    }
}