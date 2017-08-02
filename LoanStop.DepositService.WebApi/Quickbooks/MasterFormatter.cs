using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.Export;
using LoanStop.Services.WebApi.Quickbooks;
using LoanStopModel;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.DBCore.Repository.Interfaces;
using System.Diagnostics;

namespace LoanStop.Services.WebApi.Quickbooks
{
    public class MasterFormatter
    {
        private static string tabDelimiter = "\t";

        private BaseQbType stateFormatter; 
        private IStateQuries rep;
        private string state;

        public MasterFormatter(string store, string state, string connectionString)
        {
            this.state = state;

            if (state.ToLower() == "colorado")
            { 
                stateFormatter = new LoanColorado(store);
                rep = new Repository.Colorado(connectionString);
            }
            else
            {
                stateFormatter = new LoanWyoming(store);
                rep = new Repository.Wyoming(connectionString);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<string> ExecuteCO (DateTime startDate, DateTime endDate)
        {
            var rtrn = new List<string>();

            //loans
            var cashLoans = rep.ExportLineItem("loaned",startDate, endDate);
            rtrn.AddRange(loanCash(cashLoans));
            
            //ach payments - by day
            var payments = rep.ExportLineItem("paymentstoloans",startDate, endDate);
            //rtrn.AddRange(achPayment(payments));
            //cash payments
            //rtrn.AddRange(cashPayment(payments));
            rtrn.AddRange(combinedPayment(payments));

            //bounced
            var bouncers = rep.ExportLineItem("bounced",startDate, endDate);
            rtrn.AddRange(coloradoBounced(bouncers));
            
            //money gram
            var moneygrams = rep.ExportLineItem("moneygram",startDate, endDate);
            rtrn.AddRange(moneygram(moneygrams));
            
            //check gold
            var checkgolds = rep.ExportLineItem("checkgold",startDate, endDate);
            rtrn.AddRange(checkgold(checkgolds));

            //defaulted
            var defaulted = rep.ExportLineItem("defaulted",startDate, endDate);
            rtrn.AddRange(writedownloan(defaulted));

            //paymentstodefault
            var paymentstodefault = rep.ExportLineItem("paymentstodefault",startDate, endDate);
            rtrn.AddRange(paidtodefault(paymentstodefault));

            //transfers Cash
            var transfers = rep.ExportLineItem("transfers",startDate, endDate);
            rtrn.AddRange(transfer(transfers));
            
            //getcash
            var getcashs = rep.ExportLineItem("getcash",startDate, endDate);
            rtrn.AddRange(getcash(getcashs));

            //expenses
            var expenses = rep.ExportLineItem("expenses",startDate, endDate);
            rtrn.AddRange(expense(expenses));

            //corp cash
            var corpCashs = rep.ExportLineItem("corpcash", startDate, endDate);
            rtrn.AddRange(corpCash(corpCashs));

            var adjustmentsCashs = rep.ExportLineItem("adjustmentscash", startDate, endDate);
            rtrn.AddRange(adjustmentCash(adjustmentsCashs));

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<string> ExecuteClosed (DateTime startDate, DateTime endDate)
        {
            var rtrn = new List<string>();

            //loans
            var cashLoans = rep.ExportLineItem("loaned",startDate, endDate);
            rtrn.AddRange(loanCash(cashLoans));
            
            //ach payments - by day
            var payments = rep.ExportLineItem("closedpayments",startDate, endDate);
            //rtrn.AddRange(achPayment(payments));
            //cash payments
            //rtrn.AddRange(cashPayment(payments));
            rtrn.AddRange(combinedPayment(payments));

            //bounced
            var bouncers = rep.ExportLineItem("bounced",startDate, endDate);
            rtrn.AddRange(wyomingBounced(bouncers));
            
            ////money gram
            //var moneygrams = rep.ExportLineItem("moneygram",startDate, endDate);
            //rtrn.AddRange(moneygram(moneygrams));
            
            ////check gold
            //var checkgolds = rep.ExportLineItem("checkgold",startDate, endDate);
            //rtrn.AddRange(checkgold(checkgolds));

            //defaulted
            var defaulted = rep.ExportLineItem("defaulted",startDate, endDate);
            rtrn.AddRange(writedownloan(defaulted));

            //paymentstodefault
            var paymentstodefault = rep.ExportLineItem("paymentstodefault",startDate, endDate);
            rtrn.AddRange(paidtodefault(paymentstodefault));

            //transfers
            //var transfers = rep.ExportLineItem("transfers",startDate, endDate);
            //rtrn.AddRange(transfer(transfers));
            
            //getcash
            //var getcashs = rep.ExportLineItem("getcash",startDate, endDate);
            //rtrn.AddRange(getcash(getcashs));

            //expenses
            //var expenses = rep.ExportLineItem("expenses",startDate, endDate);
            //rtrn.AddRange(expense(expenses));

            
            return rtrn;
        }

        public List<string> ExecuteWY (DateTime startDate, DateTime endDate)
        {
            var rtrn = new List<string>();

            //loans
            var cashLoans = rep.ExportLineItem("loaned",startDate, endDate);
            rtrn.AddRange(loanCash(cashLoans));
            
            //check deposit - by day && check gold
            var checkDeposit = rep.ExportLineItem("checkDeposits",startDate, endDate);
            rtrn.AddRange(wyomingCheckPayment(checkDeposit));

            //cash deposit
            var cashDeposit = rep.ExportLineItem("cashDeposits",startDate, endDate);
            rtrn.AddRange(wyomingCashPayment(cashDeposit));
            
            //ach paid - by day
            //var achDeposit = rep.ExportLineItem("achDeposits",startDate, endDate);
            //rtrn.AddRange(wyomingAchPayment(achDeposit));

            //ach paid - by day
            var cashPaidPaymentPlans = rep.ExportLineItem("cashpaidpp",startDate, endDate);
            rtrn.AddRange(wyomingCashPaidPaymentPlan(cashPaidPaymentPlans));

			//money gram
            var moneygrams = rep.ExportLineItem("moneygram",startDate, endDate);
            rtrn.AddRange(moneygram(moneygrams));
            
            //bounced - date returned != null 
            var bouncers = rep.ExportLineItem("bouncedwy",startDate, endDate);
            rtrn.AddRange(wyomingBounced(bouncers));

            //defaulted - rest of payment plan
            var defaulted = rep.ExportLineItem("defaultwy",startDate, endDate);
            rtrn.AddRange(wyomingDefault(defaulted));

			//paymentstodefault
            //var paymentstodefault = rep.ExportLineItem("paymentstodefault",startDate, endDate);
            //rtrn.AddRange(paidtodefault(paymentstodefault));

            //transfers
            var transfers = rep.ExportLineItem("transfers",startDate, endDate);
            rtrn.AddRange(transfer(transfers));
            
            //getcash
            var getcashs = rep.ExportLineItem("getcash",startDate, endDate);
            rtrn.AddRange(getcash(getcashs));

            //expenses
            var expenses = rep.ExportLineItem("expenses",startDate, endDate);
            rtrn.AddRange(expense(expenses));

            //expenses
            var cashcorps = rep.ExportLineItem("corpcash", startDate, endDate);
            rtrn.AddRange(corpCash(cashcorps));

            return rtrn;
        }

        
        private List<string> loanCash (List<ExportEntity> results)
        {
            var rtrn = new List<string>();

            decimal sumCashDisbursed = 0.0m;

            if (this.state.ToLower() == "colorado")
            {
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var loan in results)
                {
                    sumCashDisbursed += loan.amount;
                    rtrn.AddRange(stateFormatter.Issue(loan));
                }

            }
            else
            {
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);

                var received = results.Where( w=>w.export_status == "received").ToList();

                foreach(var loan in received)
                {
                    var disbursed = results.Where(w => w.export_status == "disbursed" && w.doc_num == loan.doc_num ).FirstOrDefault();
                    sumCashDisbursed += disbursed.amount;
                    rtrn.AddRange(stateFormatter.Issue(disbursed, loan));
                }
            }

            Debug.Print("sumCashDisbursed : " + sumCashDisbursed.ToString());

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ach"></param>
        /// <param name="cash"></param>
        /// <returns></returns>
        private List<string> combinedPayment (List<ExportEntity> results)
        {

            var rtrn = new List<string>();
            
            var colorado = results.Where(w => w.transactiontype == "payment" && (w.catagory == "deposit")).ToList();
            var cashPayments = results.Where(w => w.transactiontype == "payment" && (w.catagory == "cash-payment")).ToList(); 
            colorado.AddRange(cashPayments);
            
            var sumCashPayments = cashPayments.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();
            Debug.WriteLine("sumCashPayments : " + sumCashPayments);

            var loanPayments = colorado.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();

            string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader5 = "!ENDTRNS";
            rtrn.Add(achHeader1);
            rtrn.Add(achHeader2);
            rtrn.Add(achHeader3);
            rtrn.Add(achHeader4);
            rtrn.Add(achHeader5);


                var fees = results.Where(w => w.transactiontype == "payment" && (w.catagory == "fee-income")).ToList();
                var principals = results.Where(w => w.transactiontype == "payment" && (w.catagory == "principal")).ToList();
            
                foreach(var payment in loanPayments)
                {
                    var deposit = results.Where(w => w.transactiontype == "payment" && (w.catagory == "deposit") && payment.Key == w.export_date.Date).FirstOrDefault();
                    
                    var cash = sumCashPayments.Where(w => payment.Key == w.Key).FirstOrDefault();

                    decimal fee = fees.Where(w => w.export_date ==payment.Key && w.catagory == "fee-income").Sum(a => a.amount);
                    decimal principal = principals.Where(w => w.export_date == payment.Key && w.catagory == "principal").Sum(a => a.amount);

                    if (deposit == null) deposit = new ExportEntity(){ amount = 0, export_date = payment.Key};

                    decimal cashAmount = 0;
                    if (cash == null)
                        cashAmount = 0;
                    else
                        cashAmount = cash.Amount;

                    rtrn.AddRange(stateFormatter.CombinedPayment(payment.Key, deposit.amount, cashAmount, fee, principal));
                }

            return rtrn;
        }


        private List<string> achPayment (List<ExportEntity> results)
        {

            var rtrn = new List<string>();
            var coloradoAch = results.Where(w => w.transactiontype == "loan" && (w.catagory == "deposit")).ToList();
            var loanPayments = coloradoAch.GroupBy(g => g.export_date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();

                        


                if (state.ToLower() == "colorado")
                { 
                    string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                    string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                    string achHeader3 = "!ENDTRNS";
                    rtrn.Add(achHeader1);
                    rtrn.Add(achHeader2);
                    rtrn.Add(achHeader3);

                    var fees = results.Where(w => w.transactiontype == "loan" && (w.catagory == "fee-income")).ToList();
                    var principals = results.Where(w => w.transactiontype == "loan" && (w.catagory == "principal")).ToList();
            
                    foreach(var payment in loanPayments)
                    {
                        decimal fee = fees.Where(w => w.export_date ==payment.Key && w.catagory == "fee-income").Sum(a => a.amount);
                        decimal principal = principals.Where(w => w.export_date == payment.Key && w.catagory == "principal").Sum(a => a.amount);

                        var paid = new ExportEntity()
                        {
                            export_date = payment.Key,
                            amount = payment.Amount,
                            doc_num = "",
                            memo = "ACH Payment"
                        };

                        //rtrn.AddRange(stateFormatter.DefaultPayment(paid));

                        rtrn.AddRange(stateFormatter.ACHPayment(payment.Key, payment.Amount, fee, principal));
                    }
                }
                else
                {
                    string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                    string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                    string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                    string achHeader4 = "!ENDTRNS";
                    rtrn.Add(achHeader1);
                    rtrn.Add(achHeader2);
                    rtrn.Add(achHeader3);
                    rtrn.Add(achHeader4);

                    var achs = results.Where(w => w.transactiontype == "loan" && (w.catagory == "ach-payment") && w.export_status == "received" ).ToList();
                    foreach(var payment in achs)
                    {
                        var disbursed = results.Where(w => w.transactiontype == "loan" && (w.catagory == "disbursed") && w.export_status == "disbursed" && w.doc_num == payment.doc_num).FirstOrDefault();
                        decimal fee = payment.amount - disbursed.amount;
                        decimal principal = disbursed.amount;
                        rtrn.AddRange(stateFormatter.ACHPayment(payment.export_date.Date, payment.amount, fee, principal));
                    }

                }

            return rtrn;
        }

        private List<string> wyomingAchPayment (List<ExportEntity> results)
        {

            var rtrn = new List<string>();
            var wyomingAch = results.Where(w => w.transactiontype == "payment" && (w.catagory == "ach-payment")).ToList();
            var loanPayments = wyomingAch.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();

                        
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader5 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);
                rtrn.Add(achHeader5);

                foreach(var payment in loanPayments)
                {
                    decimal fee = 0;
                    rtrn.AddRange(stateFormatter.ACHPayment(payment.Key, payment.Amount, fee,  payment.Amount));
                }


            return rtrn;
        }

        private List<string> wyomingCashPaidPaymentPlan (List<ExportEntity> results)
        {

            var rtrn = new List<string>();
            var wyomingCashPP = results.Where(w => w.transactiontype == "payment" && (w.catagory == "cash-payment-pp") && w.export_status == "received").ToList();
            
            var disbursed = results.Where(w => w.transactiontype == "payment" && (w.catagory == "cash-payment-pp") && w.export_status == "disbursed").ToList();
			//var wyomingCashPPIncome = results.Where(w => w.transactiontype == "payment-plan" && (w.catagory == "cash-pp-income")).ToList();
            //var loanIncome = wyomingCashPPIncome.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList().OrderBy(ob => ob.Key.Date);

                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader5 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);
                rtrn.Add(achHeader5);

                foreach(var payment in wyomingCashPP)
                {
                    decimal fee = 0;
					var d = disbursed.Where(w => w.doc_num == payment.doc_num).FirstOrDefault();
                    if (d != null)
                    { 
                        fee = payment.amount - d.amount;
                    }
                    else
                    {
                        d.amount = 0;
                    }

                    fee = payment.amount - d.amount;

                    rtrn.AddRange(stateFormatter.CashPaidPaymentPlan(payment.export_date.Date, payment.amount, fee,  d.amount));
                }

            return rtrn;
        }

        private List<string> wyomingCheckPayment (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var checkgold = results.Where(w => w.transactiontype == "misc").ToList();
            var wyomingDeposit = results.Where(w => w.transactiontype == "payment" && (w.catagory == "check-deposit")).ToList();
            var wyomingACH = results.Where(w => w.transactiontype == "payment" && (w.catagory == "ach-deposit")).ToList();
            wyomingDeposit.AddRange(wyomingACH);
            var loanPayments = wyomingDeposit.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();
                        
            string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            string achHeader5 = "!ENDTRNS";
            rtrn.Add(achHeader1);
            rtrn.Add(achHeader2);
            rtrn.Add(achHeader3);
            rtrn.Add(achHeader4);
            rtrn.Add(achHeader5);

            decimal sumCashDisbursed = 0.0m;

            foreach(var deposit in loanPayments)
            {
                decimal disbursed = results.Where(w => w.transactiontype == "payment" && (w.catagory == "check-payment") && w.export_status == "disbursed" && w.export_date == deposit.Key).Sum(s => s.amount);
                
                decimal received = results.Where(w => w.transactiontype == "payment" && (w.catagory == "check-payment") && w.export_status == "received"  && w.export_date == deposit.Key ).Sum(s => s.amount);

                decimal deferredFee = received - disbursed;
                
                decimal principal = disbursed;


                decimal checkFee = 0;
                decimal cashAmount = 0;
                if (deposit.Amount != received)
                { 
                    //decimal difference = deposit.Amount - received;
                    
                    decimal checkAmount = checkgold.Where(w => w.transactiontype == "misc" && w.export_date.Date == deposit.Key && w.export_status == "recieved").Sum(s => s.amount);

                    cashAmount = results.Where(w => w.transactiontype == "misc" && w.export_date.Date == deposit.Key && w.export_status == "disbursed").Sum(s => s.amount);

                    //cashAmount = deposit.Amount - received;
                    sumCashDisbursed += cashAmount;

                    checkFee = checkAmount - cashAmount;
                }
                else
                {
                    cashAmount = results.Where(w => w.transactiontype == "misc" && w.export_date.Date == deposit.Key && w.export_status == "disbursed").Sum(s => s.amount);
                    sumCashDisbursed += cashAmount;
                }

                decimal fee = deferredFee + checkFee;
                
                rtrn.AddRange(stateFormatter.CheckPayment(deposit.Key, deposit.Amount, fee, deferredFee, received, cashAmount));
            }

            Debug.Print("CASH -cashed checked : " + sumCashDisbursed);

            return rtrn;
        }
		
		
		private List<string> wyomingDefault (List<ExportEntity> results)
        {

            var rtrn = new List<string>();
            var wyomingDefault = results.Where(w => w.transactiontype == "default").ToList();
                        
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var d in wyomingDefault)
                {
                    rtrn.AddRange(stateFormatter.Default(d));
                }


            return rtrn;
        }


        private List<string> cashPayment (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var cash = results.Where(w => w.transactiontype == "loan" && (w.catagory == "cash-payment")).ToList();
            var loanPayments = cash.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();

            if (loanPayments != null && loanPayments.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader5 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);
                rtrn.Add(achHeader5);


                var fees = results.Where(w => w.transactiontype == "loan" && (w.catagory == "fee-income")).ToList();
                var principals = results.Where(w => w.transactiontype == "loan" && (w.catagory == "principal")).ToList();
            
                foreach(var payment in loanPayments)
                {
                    decimal fee = fees.Where(w => w.export_date.Date == payment.Key && w.catagory == "fee-income").Sum(a => a.amount);
                    decimal principal = principals.Where(w => w.export_date.Date == payment.Key && w.catagory == "principal").Sum(a => a.amount);

                    rtrn.AddRange(stateFormatter.CashPayment(payment.Key, payment.Amount, fee, principal));
                }
            }

            return rtrn;
        }

        private List<string> wyomingCashPayment (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var cash = results.Where(w => w.transactiontype == "payment" && (w.catagory == "cash-payment")).ToList();
            var loanPayments = cash.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();

            if (loanPayments != null && loanPayments.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader5 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);
                rtrn.Add(achHeader5);


            
                foreach(var payment in loanPayments)
                {
                    decimal disbursed = results.Where(w => w.transactiontype == "payment" && (w.catagory == "disbursed") && w.export_status == "disbursed" && w.export_date == payment.Key).Sum(s => s.amount);
                
                    decimal received = results.Where(w => w.transactiontype == "payment" && (w.catagory == "received") && w.export_status == "received"  && w.export_date == payment.Key ).Sum(s => s.amount);

                    decimal paid = results.Where(w => w.transactiontype == "payment" && (w.catagory == "cash-payment") && w.export_status == "paid"  && w.export_date == payment.Key ).Sum(s => s.amount);

                    decimal fee = paid - disbursed;
                
                    decimal receivable = received;

                    rtrn.AddRange(stateFormatter.CashPayment(payment.Key, paid, fee, receivable));
                }
            }

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> coloradoBounced (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var bouncers = results.Where(w => w.transactiontype == "bounced" && w.catagory == "bounced").ToList();

            if (bouncers != null && bouncers.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);

                foreach(var bounce in bouncers)
                {
                    ExportEntity principal = results.Where(w => w.doc_num == bounce.doc_num && w.catagory == "principal").FirstOrDefault();

					if (principal == null)
					{ 
						principal = new ExportEntity()
						{
						    export_date = bounce.export_date,
						    amount = GetColoradoPrincipal(bounce.amount),
						    doc_num = bounce.doc_num,
						    memo = bounce.memo
						};
                    }

                    ExportEntity fee = results.Where(w => w.doc_num == bounce.doc_num && w.catagory == "fee-income").FirstOrDefault();
                    
					if (principal == null)
					{ 
						principal = new ExportEntity()
						{
						    export_date = bounce.export_date,
						    amount = GetColoradoPrincipal(bounce.amount),
						    doc_num = bounce.doc_num,
						    memo = bounce.memo
						};
						
						fee = new ExportEntity()
						{
							export_date = bounce.export_date,
							amount =Math.Abs(bounce.amount) - Math.Abs(principal.amount),
							doc_num = bounce.doc_num,
							memo = bounce.memo
						};

					}

					if (fee == null)
					{
						fee = new ExportEntity()
						{
							export_date = bounce.export_date,
							amount =Math.Abs(bounce.amount) - Math.Abs(principal.amount),
							doc_num = bounce.doc_num,
							memo = bounce.memo
						};
					}
					fee.amount = Math.Abs(fee.amount);
					principal.amount = Math.Abs(principal.amount);

					if (principal.amount > bounce.amount)
					{
						bounce.amount = principal.amount + fee.amount;
					}

					if (bounce.amount != principal.amount + fee.amount)
					{
						bounce.amount = principal.amount + fee.amount;
					}

                    
					rtrn.AddRange(stateFormatter.Bounce(bounce, fee, principal));
                }
            }

            return rtrn;
        }
    
                /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> wyomingBounced (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var bouncers = results.Where(w => w.transactiontype == "bounced" && w.export_status == "received").ToList();

            if (bouncers != null && bouncers.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);

                foreach(var bounce in bouncers)
                {
                    var disbursed = results.Where(w => w.doc_num == bounce.doc_num && w.export_status == "disbursed" && w.catagory == bounce.catagory).FirstOrDefault();

					ExportEntity principal = new ExportEntity();
					ExportEntity fee = new ExportEntity();
					if (disbursed != null)
					{ 
						principal = new ExportEntity()
						{
							export_date = disbursed.export_date,
							amount = -disbursed.amount,
							doc_num = disbursed.doc_num,
							memo = disbursed.memo
						};
                    
                    
						fee = new ExportEntity()
						{
							export_date = disbursed.export_date,
							amount =Math.Abs(bounce.amount) - Math.Abs(disbursed.amount),
							doc_num = disbursed.doc_num,
							memo = disbursed.memo
						};
					}
					else
					{
						principal.amount = -bounce.amount;
						fee.amount = 0;

					}

                    rtrn.AddRange(stateFormatter.Bounce(bounce, fee, principal));
                }
            }

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> moneygram(List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var billpay = results.Where(w => w.transactiontype == "money-gram" && w.catagory == "bill-pay" && w.export_status == "cash" ).ToList();
            var sendwire = results.Where(w => w.transactiontype == "money-gram" && w.catagory == "send-wire" && w.export_status == "cash" ).ToList();
            var moneyorder = results.Where(w => w.transactiontype == "money-gram" && w.catagory == "money-order" && w.export_status == "cash" ).ToList();
            var debitcard = results.Where(w => w.transactiontype == "money-gram" && w.catagory == "debit-card" && w.export_status == "cash").ToList();

            var billPayByDay = billpay.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();
            var sendwireByDay = sendwire.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();
            var moneyorderByDay = moneyorder.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList();
            var debitcardByDay = debitcard.GroupBy(g => g.export_date.Date).Select(group => new { Amount = group.Sum(g => g.amount), Key = group.Key }).ToList();

            decimal sumBillPay = 0.0m;
            decimal sumWire = 0.0m;
            decimal sumMoneyOrder = 0.0m;
            decimal sumDebitCard = 0.0m;
            if (
                (billPayByDay != null && billPayByDay.Count() > 0) ||
                (sendwireByDay != null && sendwireByDay.Count() > 0) ||
                (moneyorderByDay != null && moneyorderByDay.Count() > 0)
            )
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);

                foreach(var cash in billPayByDay)
                {
                    //var cash = results.Where(w => w.doc_num == item.Key && w.export_status == "cash").FirstOrDefault();
                    var cashEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(cash.Key.ToString()),
                        amount = cash.Amount,
                        doc_num = "0",
                        memo = "Bill Pay"
                    };

                    var checks = results.Where(w => w.export_date.Date == cash.Key && w.catagory=="bill-pay" && w.export_status == "check").ToList();

                    var check = checks.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList().FirstOrDefault();
                    var checkEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(check.Key.ToString()),
                        amount = check.Amount,
                        doc_num = "0",
                        memo = "Bill Pay"
                    };
                    rtrn.AddRange(stateFormatter.MoneyGram(cashEntity, checkEntity));
                    sumBillPay += cash.Amount;
                }

                foreach (var cash in sendwireByDay)
                {
                    //var cash = results.Where(w => w.doc_num == item.Key && w.export_status == "cash").FirstOrDefault();
                    var cashEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(cash.Key.ToString()),
                        amount = cash.Amount,
                        doc_num = "0",
                        memo = "Send Wire"
                    };

                    var checks = results.Where(w => w.export_date.Date == cash.Key && w.catagory=="send-wire" && w.export_status == "check").ToList();

                    var check = checks.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList().FirstOrDefault();
                    var checkEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(check.Key.ToString()),
                        amount = check.Amount,
                        doc_num = "0",
                        memo = "Send Wire"
                    };
                    rtrn.AddRange(stateFormatter.MoneyGram(cashEntity, checkEntity));
                    sumWire += cash.Amount;
                }

                foreach (var cash in moneyorderByDay)
                {
                    //var cash = results.Where(w => w.doc_num == item.Key && w.export_status == "cash").FirstOrDefault();
                    var cashEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(cash.Key.ToString()),
                        amount = cash.Amount,
                        doc_num = "0",
                        memo = "Money Order"
                    };

                    var checks = results.Where(w => w.export_date.Date == cash.Key && w.catagory=="money-order" && w.export_status == "check").ToList();

                    var check = checks.GroupBy(g => g.export_date.Date).Select(group => new {Amount = group.Sum(g => g.amount), Key = group.Key}).ToList().FirstOrDefault();
                    var checkEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(check.Key.ToString()),
                        amount = check.Amount,
                        doc_num = "0",
                        memo = "Money Order"
                    };
                    rtrn.AddRange(stateFormatter.MoneyGram(cashEntity, checkEntity));
                    sumMoneyOrder  += cash.Amount;
                }

                foreach (var cash in debitcardByDay)
                {
                    //var cash = results.Where(w => w.doc_num == item.Key && w.export_status == "cash").FirstOrDefault();
                    var cashEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(cash.Key.ToString()),
                        amount = cash.Amount,
                        doc_num = "0",
                        memo = "debit card"
                    };

                    var checks = results.Where(w => w.export_date.Date == cash.Key && w.catagory == "debit-card" && w.export_status == "check").ToList();

                    var check = checks.GroupBy(g => g.export_date.Date).Select(group => new { Amount = group.Sum(g => g.amount), Key = group.Key }).ToList().FirstOrDefault();
                    var checkEntity = new ExportEntity()
                    {
                        export_date = DateTime.Parse(check.Key.ToString()),
                        amount = check.Amount,
                        doc_num = "0",
                        memo = "debit card"
                    };
                    rtrn.AddRange(stateFormatter.MoneyGram(cashEntity, checkEntity));
                    sumDebitCard += cash.Amount;
                }


            }

            Debug.Print(" Sum bill pay : " + sumBillPay);
            Debug.Print(" Sum wire : " + sumWire);
            Debug.Print(" Sum money order : " + sumMoneyOrder);
            Debug.Print(" Sum debit: " + sumDebitCard);

            return rtrn;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> checkgold(List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var checkgold = results.Where(w => w.transactiontype == "misc").ToList();
                
            if (checkgold != null && checkgold.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader4 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);
                rtrn.Add(achHeader4);

                var checks = results.Where(w => w.catagory == "cash-check" && w.export_status == "recieved").ToList();

                foreach(var item in checks)
                {
                    var check = results.Where(w => w.doc_num == item.doc_num && w.export_status == "recieved").FirstOrDefault();

                    var cash = results.Where(w => w.doc_num == item.doc_num && w.export_status == "disbursed").FirstOrDefault();

                    rtrn.AddRange(stateFormatter.Check(cash, check));
                }

                var golds = results.Where(w => w.catagory == "gold" && w.export_status == "recieved").ToList();

                foreach(var item in golds)
                {
                    var check = results.Where(w => w.doc_num == item.doc_num && w.export_status == "recieved").FirstOrDefault();

                    var cash = results.Where(w => w.doc_num == item.doc_num && w.export_status == "disbursed").FirstOrDefault();

                    rtrn.AddRange(stateFormatter.Gold(cash, check));
                }
            }

            return rtrn;
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> writedownloan (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
                
            if (results != null && results.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var loan in results)
                {
                    rtrn.AddRange(stateFormatter.Default(loan));
                }
            }
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> paidtodefault (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var paidtodefault = results.Where(w => w.transactiontype == "loan" && w.catagory=="payment-default" && w.amount > 0).ToList();
                
            if (paidtodefault != null && paidtodefault.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var loan in paidtodefault)
                {
                    rtrn.AddRange(stateFormatter.DefaultPayment(loan));
                }
            }

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> transfer (List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            decimal sumToCorp = 0.0m;
            decimal sumFromCorp = 0.0m;
            //var toCorprate = results.Where(w => w.transactiontype == "transfer" && w.export_status =="check" && w.amount < 0).ToList();
            //var fromCorprate = results.Where(w => w.transactiontype == "transfer"  && w.export_status =="check" && w.amount > 0).ToList();

            //if (fromCorprate.Count() > 0 || toCorprate.Count() > 0)
            //{ 
            //    string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            //    string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
            //    string achHeader3 = "!ENDTRNS";
            //    rtrn.Add(achHeader1);
            //    rtrn.Add(achHeader2);
            //    rtrn.Add(achHeader3);

            //    foreach(var item in toCorprate)
            //    {
            //        rtrn.AddRange(stateFormatter.TransferToCorprate(item));
            //    }

            //    foreach(var item in fromCorprate)
            //    {
            //        rtrn.AddRange(stateFormatter.TransferFromCorprate(item));
            //    }

            //}

            var toCorprate = results.Where(w => w.transactiontype == "transfer" && w.export_status == "cash" && w.amount < 0).ToList();
            var fromCorprate = results.Where(w => w.transactiontype == "transfer" && w.export_status == "cash" && w.amount > 0).ToList();

            if (fromCorprate.Count() > 0 || toCorprate.Count() > 0)
            {
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach (var item in toCorprate)
                {
                    rtrn.AddRange(stateFormatter.TransferToCorprate(item));
                    sumToCorp += item.amount;
                }

                foreach (var item in fromCorprate)
                {
                    rtrn.AddRange(stateFormatter.TransferFromCorprate(item));
                    sumFromCorp += item.amount;
                }

            }

            Debug.Print("sum cash transfer to corp : " + sumToCorp);
            Debug.Print("sum cash transfer from corp : " + sumFromCorp);

            return rtrn;
        }
    
                /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> getcash(List<ExportEntity> results)
        {
            var rtrn = new List<string>();
            var transfer = results.Where(w => w.transactiontype == "get-cash" && w.amount > 0).ToList();
                
            var miscDeposits = results.Where(w => w.transactiontype == "get-cash" && w.amount < 0).ToList();
            
            if (transfer != null && transfer.Count() > 0)
            { 
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var cash in transfer)
                {
                    rtrn.AddRange(stateFormatter.GetCash(cash));
                }

                foreach(var cash in miscDeposits)
                {
                    rtrn.AddRange(stateFormatter.MiscDeposits(cash));
                }

            }

            return rtrn;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        private List<string> expense (List<ExportEntity> results)
        {
            var rtrn = new List<string>();

            var cash = results.Where(w => w.transactiontype == "expense" && w.export_status == "cash").ToList();
            var check = results.Where(w => w.transactiontype == "expense"  && w.export_status == "check").ToList();

            if (cash.Count() > 0 || check.Count() > 0){
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach(var c in cash)
                {
                    rtrn.AddRange(stateFormatter.ExpenseCASH(c));
                }

                foreach(var c in check)
                {
                    rtrn.AddRange(stateFormatter.ExpenseCHECK(c));
                }
            }
            return rtrn;
        }

        private List<string> corpCash(List<ExportEntity> results)
        {
            var rtrn = new List<string>();

            var cash = results.Where(w => w.transactiontype == "corp" && w.export_status == "cash").ToList();

            if (cash.Count() > 0)
            {
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach (var c in cash)
                {
                    rtrn.AddRange(stateFormatter.ExpenseCASH(c));
                }

            }
            return rtrn;
        }

        private List<string> adjustmentCash(List<ExportEntity> results)
        {
            var rtrn = new List<string>();

            var cash = results.Where(w => w.transactiontype == "" && w.export_status == "cash").ToList();

            if (cash.Count() > 0)
            {
                string achHeader1 = "!TRNS	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader2 = "!SPL	TRNSTYPE	DATE	ACCNT	NAME	AMOUNT	DOCNUM	MEMO	CLASS	CLEAR";
                string achHeader3 = "!ENDTRNS";
                rtrn.Add(achHeader1);
                rtrn.Add(achHeader2);
                rtrn.Add(achHeader3);

                foreach (var c in cash)
                {
                    //rtrn.AddRange(stateFormatter.ExpenseCASH(c));
                }

            }
            return rtrn;
        }


        private decimal GetColoradoPrincipal(decimal amount)
		{
			decimal principal = 50m;
			string stringAmount = amount.ToString();
			
			if (amount > 0m && amount < 29m)
			{
				principal = 16.67m;
			}
			else if (amount >= 29m && amount < 58m  )
			{
				principal = 33.33m;
			}
			else if (amount >= 58m && amount < 86m  )
			{
				principal = 50m;
			}
			else if (amount >= 86m && amount < 112  )
			{
				principal = 66.67m;
			}
			else if (amount >= 112)
			{
				principal = 83.33m;
			}

			return principal;

		}
    
    }
}