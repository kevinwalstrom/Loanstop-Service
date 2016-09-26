using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Model = LoanStopModel;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.Services.WebApi.Business.Colorado
{
    public class IsNewLoanEnabled : IIsNewLoanEnabled
    {
        private string connectionString;
        private DefaultType defaults;

        public IsNewLoanEnabled(string connectionString, DefaultType defaults)
        {
            this.connectionString = connectionString;
            this.defaults = defaults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="transactionList"></param>
        /// <returns></returns>
        public bool Execute(Model.Client client, DateTime theDate, List<Model.Transaction> transactionList)
        {
            var paymentRepository = new Repository.Payment(connectionString);
            var transRepository = new Repository.Transaction(connectionString);

            bool rtrn = false;

            bool bOpenTransaction = false;

            // check client Status
            rtrn = client.Status == "0" ? true : false;

            if (!rtrn)
            {
                return false;
            }

            // check updated info date
            if (client.UpdatedInfoDate.HasValue)
            {
                rtrn = client.UpdatedInfoDate.Value.AddDays(60) <= DateTime.Now ? false : true;
                if (!rtrn)
                {
                    return false;
                }
            }
            else
            { 
                return false;
            }

            //check bank routing number
            //If Not (IsNumeric(ClientInfo.BankRoutingNumber) And ClientInfo.BankRoutingNumber.Length = 9) Then
            //    btnNewInstallmentLoan.Enabled = False
            //End If

            // check aging
            if (Aging.DetermineAging(transactionList))
            {
                rtrn = false;
            }

            // check if c.ount > 0 and transaction open  
            var openStatus = new string[] { "Open", "Payment Plan", "Deposit", "Pickup", "Pickup-NC", "Pick Up" };

            if (transactionList.Count() > 0)
            {
                bOpenTransaction = transactionList.Any(s => openStatus.Contains(s.Status));
                if (bOpenTransaction)
                {
                    // no loans for 30 days - installment
                    var status = new string[] { "Open", "Deposit", "Pick Up" };
                    var checkList = transactionList.Where(s => status.Contains(s.Status));

                    var any = checkList.Any(s => s.TransDate.Value.AddDays(30) > DateTime.Now);

                    rtrn = any ? false : rtrn; 
                    
                    if (!rtrn)
                    {
                        return false;
                    }
                    
                    // '*********** new accounts have to make a payment to open new multiple loans
                    if (rtrn)
                    {
                        rtrn = paymentRepository.HasCustomerMadePayments(client.SsNumber, theDate);
                        if (!rtrn)
                        {
                            return false;
                        }
                    }
                    else
                    { 
                        return false;
                    }


                    //'no enable for sum of loans minus customer limit.
                    //'If btnNewPayDayLoan.Enabled Then
                    if (rtrn)
                    {
                        var SumAmountDispursed = transRepository.CustomerAmountDisbursed(client.SsNumber);

                        if (SumAmountDispursed >= decimal.Parse(client.CheckLimit))
                        { 
                            rtrn = false;
                        }

                        if (SumAmountDispursed >= defaults.MaxLimit)
                        {
                            rtrn = false;
                        }
                    }
                }
            }

            return rtrn;

        }
    
    
    }
}