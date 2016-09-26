using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Quickbooks
{
    public class LoanWyoming : BaseQbType
    {
        private string FEE_INCOME = "4100 · Fee Income";
        private string BANK_ACCOUNT = "1000 · Bank Accounts:{0} · {1}";
        private string CASH_ACCOUNT = "1000 · Bank Accounts:{0} · {1} Cash";
        private string LOAN_DEFAULTS = "4700 · Loan Defaults";

        public LoanWyoming(string store) : base (store)
        { }

        public override List<string> Issue(ExportEntity disbursed, ExportEntity received = null)
        {
            
            decimal fee = received.amount - disbursed.amount;

            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = disbursed.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = disbursed.memo,
                Amount = -disbursed.amount,
                Docnum = disbursed.doc_num,
                Memo = "Issue Loan",
                Class = classNumber,
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = received.export_date,
                Accnt = string.Format("1200 · Loans Receivable"),
                Name = disbursed.memo,
                Amount = received.amount,
                Docnum = disbursed.doc_num,
                Memo = "Issue Loan",
                Class = classNumber,
                Clear = ""
            };

            var splFee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = received.export_date,
                Accnt = string.Format("2100 · Deferred Fee Income"),
                Name = disbursed.memo,
                Amount = -fee,
                Docnum = disbursed.doc_num,
                Memo = "Issue Loan",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add(splFee.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }
    

        public override List<string> Default(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format("1200 · Loans Receivable"),
                Name = record.memo,//principal.memo,
                Amount = -record.amount,
                Docnum = record.doc_num,//principal.doc_num,
                Memo = "Default Loan",
                Class = classNumber,
                Clear = "N"
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(LOAN_DEFAULTS),
                Name = "",//principal.memo,
                Amount = record.amount,
                Docnum = "",//principal.doc_num,
                Memo = "Default Loan",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> CashPayment(DateTime date, decimal payment, decimal fee, decimal receivable)
        {
            decimal deferredFee = payment - receivable - fee;
            
            var cashTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = "",//payment.memo,
                Amount = payment,
                Docnum = "",//payment.doc_num,
                Memo = "Cash loan payment",
                Class = classNumber,
                Clear = "N"
            };
        
            var cashSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("1200 · Loans Receivable"),
                Name = "",//principal.memo,
                Amount = receivable * -1,
                Docnum = "",//principal.doc_num,
                Memo = "Cash loan payment",
                Class = classNumber,
                Clear = ""
            };

            var cashSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(FEE_INCOME),
                Name = "",//fee.memo,
                Amount = fee * -1,
                Docnum = "",//fee.doc_num,
                Memo = "Cash loan payment",
                Class = classNumber,
                Clear = ""
            };

            var achSplDeferredfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("2100 · Deferred Fee Income"),
                Name = "",//payment.memo,
                Amount = deferredFee * -1,
                Docnum = "",//;payment.doc_num,
                Memo = "Cash loan payment",
                Class = classNumber,
                Clear = ""
            };


            var rtrn = new List<string>();
            
            rtrn.Add(cashTrans.ToQuickbooks());
            rtrn.Add(cashSplPrincipal.ToQuickbooks());
            rtrn.Add(cashSplfee.ToQuickbooks());
            rtrn.Add(achSplDeferredfee.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        public override List<string> CombinedPayment(DateTime date, decimal achPayment, decimal cashPayment, decimal fee, decimal principal)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> ACHPayment(DateTime date, decimal payment, decimal fee, decimal principal, string docNum = null)
        {
            var adjustedPayment = payment;

            //if (fee + principal > payment)
            //{
            //    adjustedPayment = fee + principal;
            //}
            
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = "",//payment.memo,
                Amount = adjustedPayment,
                Docnum = docNum,//;payment.doc_num,
                Memo = "ACH Paid",
                Class = classNumber,
                Clear = "N"
            };
        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("4200 · Collections"),
                Name = "",//payment.memo,
                Amount = adjustedPayment * -1,
                Docnum = docNum,//;payment.doc_num,
                Memo = "ACH Paid",
                Class = classNumber,
                Clear = ""
            };

			//var splCollections = new QbEntity()
			//{
			//	Trns="SPL",
			//	TrnsType= "&DEPOSIT",
			//	Date = date,
			//	Accnt = string.Format("4200 · Collections"),
			//	Name = "payment plan",
			//	Amount = adjustedPayment,
			//	Docnum = "",
			//	Memo = "ACH Paid",
			//	Class = classNumber,
			//	Clear = ""
			//};
        
			//var splDefaults = new QbEntity()
			//{
			//	Trns="SPL",
			//	TrnsType= "&DEPOSIT",
			//	Date = date,
			//	Accnt = string.Format("4600 · Loan Defaults"),
			//	Name = "payment plan",//principal.memo,
			//	Amount = -adjustedPayment,
			//	Docnum = "",//principal.doc_num,
			//	Memo = "ACH Paid",
			//	Class = classNumber,
			//	Clear = ""
			//};

            //var achSplfee = new QbEntity()
            //{
            //    Trns="SPL",
            //    TrnsType= "&DEPOSIT",
            //    Date = date,
            //    Accnt = string.Format(FEE_INCOME),
            //    Name = "",//payment.memo,
            //    Amount = fee * -1,
            //    Docnum = docNum,//;payment.doc_num,
            //    Memo = "ACH Loan payment",
            //    Class = classNumber,
            //    Clear = ""
            //};

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            //rtrn.Add(splCollections.ToQuickbooks());
            //rtrn.Add(splDefaults.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> CashPaidPaymentPlan(DateTime date, decimal payment, decimal fee, decimal principal, string docNum = null)
        {
            var adjustedPayment = payment;

            //if (fee + principal > payment)
            //{
            //    adjustedPayment = fee + principal;
            //}
            
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = "",//payment.memo,
                Amount = adjustedPayment,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Cash loan payment PP",
                Class = classNumber,
                Clear = "N"
            };
        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("1200 · Loans Receivable"),
                Name = "",//payment.memo,
                Amount = adjustedPayment * -1,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Cash loan payment PP",
                Class = classNumber,
                Clear = ""
            };

			//var splCollections = new QbEntity()
			//{
			//	Trns="SPL",
			//	TrnsType= "&DEPOSIT",
			//	Date = date,
			//	Accnt = string.Format("4200 · Collections"),
			//	Name = "payment plan",
			//	Amount = adjustedPayment,
			//	Docnum = "",
			//	Memo = "Cash Paid PP",
			//	Class = classNumber,
			//	Clear = ""
			//};
        
			var splDeferred = new QbEntity()
			{
				Trns="SPL",
				TrnsType= "&DEPOSIT",
				Date = date,
				Accnt = string.Format("2100 · Deferred Fee Income"),
				Name = "payment plan",//principal.memo,
				Amount = fee,
				Docnum = "",//principal.doc_num,
				Memo = "Cash Paid PP",
				Class = classNumber,
				Clear = ""
			};

            var achSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(FEE_INCOME),
                Name = "",//payment.memo,
                Amount = fee * -1,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Cash Paid PP",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            rtrn.Add(splDeferred.ToQuickbooks());
            //rtrn.Add(splDefaults.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> CheckPayment(DateTime date, decimal payment, decimal fee, decimal deferredFee, decimal principal, decimal cash, string docNum = null)
        {
            var adjustedPayment = payment;

            //if (fee + principal > payment)
            //{
            //    adjustedPayment = fee + principal;
            //}
            
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = "",//payment.memo,
                Amount = adjustedPayment,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Checks Deposited",
                Class = classNumber,
                Clear = "N"
            };
        
            var splCash = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = "",
                Amount = -cash,
                Docnum = "",
                Memo = "Cash Disbursed Check/Gold",
                Class = classNumber,
                Clear = "N"
            };

            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("1200 · Loans Receivable"),
                Name = "",//payment.memo,
                Amount = principal * -1,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Checks Deposited",
                Class = classNumber,
                Clear = ""
            };

            var achSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format(FEE_INCOME),
                Name = "",//payment.memo,
                Amount = fee * -1,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Checks Deposited",
                Class = classNumber,
                Clear = ""
            };


            var achSplDeferredfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = date,
                Accnt = string.Format("2100 · Deferred Fee Income"),
                Name = "",//payment.memo,
                Amount = deferredFee,
                Docnum = docNum,//;payment.doc_num,
                Memo = "Checks Deposited",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(splCash.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplDeferredfee.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override  List<string> Bounce(ExportEntity bounce, ExportEntity fee, ExportEntity principal )
        {

            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = bounce.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = "",//payment.memo,
                Amount = -bounce.amount,
                Docnum = "",//;payment.doc_num,
                Memo = "Bounced",
                Class = classNumber,
                Clear = "N"
            };

        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = principal.export_date,
                Accnt = string.Format(LOAN_DEFAULTS),
                Name = "",//payment.memo,
                Amount = principal.amount * -1,
                Docnum = "",//;payment.doc_num,
                Memo = "Bounced",
                Class = classNumber,
                Clear = ""
            };

            //var achSplDeferredFee = new QbEntity()
            //{
            //    Trns="SPL",
            //    TrnsType= "&CHECK",
            //    Date = bounce.export_date,
            //    Accnt = string.Format("4200 · Collections"),
            //    Name = "",//payment.memo,
            //    Amount = -fee.amount,
            //    Docnum = "",//;payment.doc_num,
            //    Memo = "Bounced",
            //    Class = classNumber,
            //    Clear = "N"
            //};

            
            QbEntity achSplfee = null;
            if (fee != null)
            { 
                achSplfee = new QbEntity()
                {
                    Trns="SPL",
                    TrnsType= "&CHECK",
                    Date = fee.export_date,
                    Accnt = string.Format(FEE_INCOME),
                    Name = "",//payment.memo,
                    Amount = fee.amount,
                    Docnum = "",//;payment.doc_num,
                    Memo = "Bounced",
                    Class = classNumber,
                    Clear = ""
                };
            }
            else { 
                achSplfee = new QbEntity()
                {
                    Trns="SPL",
                    TrnsType= "&CHECK",
                    Date = bounce.export_date,
                    Accnt = string.Format(FEE_INCOME),
                    Name = "",//payment.memo,
                    Amount = 0,
                    Docnum = "",//;payment.doc_num,
                    Memo = "Bounced",
                    Class = classNumber,
                    Clear = ""
                };
            }

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            //rtrn.Add(achSplDeferredFee.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> DefaultPayment(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = record.export_date,
                Accnt = string.Format(CASH_ACCOUNT, bankAccount, storeName),
                Name = record.catagory,
                Amount = record.amount,
                Docnum = record.doc_num,
                Memo = record.memo,
                Class = classNumber,
                Clear = ""
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = record.export_date,
                Accnt = string.Format("4200 · Collections"),
                Name = "",//principal.memo,
                Amount = -record.amount,
                Docnum = "",//principal.doc_num,
                Memo = "Default Loan",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> ExpenseCASH(ExportEntity record)
        {
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = record.memo,
                Amount = record.amount,
                Docnum = "",//;payment.doc_num,
                Memo = record.catagory,
                Class = classNumber,
                Clear = "N"
            };
        
            var expenseAccount = GetExpenseAccount(record.catagory);

            var achSpl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format("5000 · Operating Expenses:{0}", expenseAccount),
                Name = "",//payment.memo,
                Amount = record.amount * -1,
                Docnum = "",//;payment.doc_num,
                Memo = record.memo,
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSpl.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

                /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> ExpenseCHECK(ExportEntity record)
        {
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = "",//record.memo,
                Amount = record.amount,
                Docnum = record.doc_num,
                Memo = record.catagory,
                Class = classNumber,
                Clear = "N"
            };
        
            var expenseAccount = GetExpenseAccount(record.catagory);

            var achSpl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format("5000 · Operating Expenses:{0}", expenseAccount),
                Name = "",//record.memo,
                Amount = record.amount * -1,
                Docnum = "",//;payment.doc_num,
                Memo = record.catagory,
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSpl.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> TransferToCorprate(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = record.catagory,
                Amount = record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Transfer To CORPORATE"
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format("1000 · Bank Accounts:1012 · Intercompany Bank Tr"),
                Name = record.catagory,
                Amount = -record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Transfer To CORPORATE"
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> TransferFromCorprate(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = record.catagory,
                Amount = -record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Transfer From CORPORATE"
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format("1000 · Bank Accounts:1012 · Intercompany Bank Tr"),
                Name = record.catagory,
                Amount = record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Transfer From CORPORATE"
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> GetCash(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = record.catagory,
                Amount = -record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Get cash"
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = record.catagory,
                Amount = record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Get cash"
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }
        
                /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override List<string> MiscDeposits(ExportEntity record)
        {
            var item = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = record.catagory,
                Amount = -record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Misc Deposit"
            };
        
            var spl = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = record.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = record.catagory,
                Amount = record.amount,
                Docnum = "",//record.doc_num,
                Memo = "Misc Deposit"
            };

            var rtrn = new List<string>();
            
            rtrn.Add(item.ToQuickbooks());
            rtrn.Add(spl.ToQuickbooks());
            rtrn.Add("ENDTRNS");
            
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override  List<string> MoneyGram(ExportEntity cash, ExportEntity check)
        {
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = cash.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = cash.memo,
                Amount = cash.amount,
                Docnum = "",//cash.doc_num,
                Memo = "Moneygram",
                Class = classNumber,
                Clear = "N"
            };
        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = check.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = check.memo,
                Amount = check.amount,
                Docnum = "",//cash.doc_num,
                Memo = "Moneygram",
                Class = classNumber,
                Clear = ""
            };

            var achSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = cash.export_date,
                Accnt = string.Format(FEE_INCOME),
                Name = cash.memo,
                Amount = -check.amount - cash.amount,
                Docnum = "",//cash.doc_num,
                Memo = "Moneygram",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override  List<string> Check(ExportEntity cash, ExportEntity check)
        {
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&DEPOSIT",
                Date = cash.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = cash.memo,
                Amount = cash.amount * -1,
                Docnum = cash.doc_num,
                Memo = "Cash Check",
                Class = classNumber,
                Clear = "N"
            };
        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&CHECK",
                Date = check.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = check.memo,
                Amount = check.amount,
                Docnum = check.doc_num,
                Memo = "Cash Check",
                Class = classNumber,
                Clear = ""
            };

            var achSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = cash.export_date,
                Accnt = string.Format(FEE_INCOME),
                Name = cash.memo,
                Amount = (cash.amount - check.amount),
                Docnum = cash.doc_num,
                Memo = "Cash Check",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public override  List<string> Gold(ExportEntity cash, ExportEntity check)
        {
            var achTrans = new QbEntity()
            {
                Trns="TRNS",
                TrnsType= "&CHECK",
                Date = cash.export_date,
                Accnt = string.Format(CASH_ACCOUNT, cashAccount, storeName),
                Name = cash.memo,
                Amount = cash.amount,
                Docnum = cash.doc_num,
                Memo = "Gold",
                Class = classNumber,
                Clear = "N"
            };
        
            var achSplPrincipal = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = check.export_date,
                Accnt = string.Format(BANK_ACCOUNT, bankAccount, storeName),
                Name = check.memo,
                Amount = check.amount,
                Docnum = check.doc_num,
                Memo = "Gold",
                Class = classNumber,
                Clear = ""
            };

            var achSplfee = new QbEntity()
            {
                Trns="SPL",
                TrnsType= "&DEPOSIT",
                Date = cash.export_date,
                Accnt = string.Format(FEE_INCOME),
                Name = cash.memo,
                Amount = cash.amount - check.amount,
                Docnum = cash.doc_num,
                Memo = "Gold",
                Class = classNumber,
                Clear = ""
            };

            var rtrn = new List<string>();
            
            rtrn.Add(achTrans.ToQuickbooks());
            rtrn.Add(achSplPrincipal.ToQuickbooks());
            rtrn.Add(achSplfee.ToQuickbooks());
            rtrn.Add("ENDTRNS");

            return rtrn;
        }

        
        public string GetExpenseAccount(string category)
        {
            string rtrn = null;

            switch (category)
            {
                case "Advertising" :
                    rtrn = "5010 · Advertising";
                    break;
                case "Bank Fees" :
                    rtrn = "5030 · Bank Fees";
                    break;
                case "Auto" :
                    rtrn = "5020 · Auto Expenses";
                    break;
                case "Collection Costs" :
                    rtrn = "5040 · Collection Costs";
                    break;
                case "Computer Equipment (Over $500 Only)" :
                    rtrn = "5050 · Computers and Equipment";
                    break;
                case "Food/Entertainment" :
                    rtrn = "5070 · Food and Entertainment";
                    break;
                case "Insurance" :
                    rtrn = "5090 · Insurance";
                    break;
                case "Miscellaneous" :
                    rtrn = "5110 · Miscellaneous";
                    break;
                case "Office Lease" :
                    rtrn = "5190 · Rent";
                    break;
                case "Office Supplies" :
                    rtrn = "5120 · Office Supplies";
                    break;
                case "Payroll" :
                    rtrn = "5130 · Payroll:5140 · Payroll Expense";
                    break;
                case "Postage" :
                    rtrn = "5170 · Postage";
                    break;
                case "Professional Services" :
                    rtrn = "5180 · Professional Services";
                    break;
                case "Select Catagory" :
                    rtrn = "5110 · Miscellaneous";
                    break;
                case "Utilities" :
                    rtrn = "5250 · Utilities";
                    break;
                case "Training" :
                    rtrn = "5230 · Training";
                    break;
                case "Travel" :
                    rtrn = "5240 · Travel";
                    break;
            }

            return rtrn;
        }

    }
}