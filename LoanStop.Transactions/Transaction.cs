using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualBasic;

using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Entities;
using LoanStop.Entities.TeleTrack;
using LoanStop.Entities.CommonTypes;
using LoanStop.Entities.Transaction;
using LoanStop.Entities.logsene;
using LoanStop.TeleTrackClient;

using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Transactions
{
    public class Transaction 
    {

        protected string connectionString;
        protected string store;
        protected DefaultType defaults;

        protected Repository.Transaction transRepository;
        protected Repository.Client clientRepository;
        protected Repository.PaymentPlanCheck ppcRepository;
        protected Repository.Payment paymentRepository;
        protected Repository.Checkbook checkbookRepository;
        protected Repository.Collect collectRepository;
        protected Repository.CashLog cashRepository;
        protected Repository.Tracking trackingRepository;

        protected long paymentPlanCheckId;

        protected decimal paymentAmount;
        protected decimal amountPaid;
        protected decimal orignalAmount;
        protected int paymentNumber;
        protected DateTime date;

		#region constructor
		public Transaction(string store, string connectionString, DefaultType defaults) 
        {
            this.connectionString = connectionString;
            this.store = store;
            this.defaults = defaults;

            transRepository = new Repository.Transaction(connectionString);
            clientRepository = new Repository.Client(connectionString);
            ppcRepository = new Repository.PaymentPlanCheck(connectionString);
            paymentRepository = new Repository.Payment(connectionString);
            checkbookRepository = new Repository.Checkbook(connectionString);
            collectRepository = new Repository.Collect(connectionString);
            cashRepository = new Repository.CashLog(connectionString);
            trackingRepository = new Repository.Tracking();
        }
		#endregion

		#region public new loans
        public NewInstallmentLoanResponse NewInstallmentLoan(NewInstallmentLoanModel model)
        {
            
            //logAction(store, "NewInstallmentLoan", model);
            
            var rtrn = new NewInstallmentLoanResponse(false);

            var client = clientRepository.GetBySSNumber(model.SsNumber);

            var transaction = new LoanStopModel.Transaction();
            transaction.CheckType = 0;
            transaction.AmountDispursed = model.AmountDisbursed;
            transaction.AmountRecieved = model.AmountReceived;
            transaction.CheckNumber = 0;
            transaction.DispursedAs = "CASH";
            transaction.SsNumber = client.SsNumber;
            transaction.Name = client.Firstname + " " + client.Lastname;
            transaction.TransDate = model.TransDate;
            transaction.DateDue = null;
            transaction.Status = "Open";
            transaction.Location = defaults.StoreCode;
            transaction = transRepository.Add(transaction);
            
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 0), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[0].HoldDate);
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 1), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[1].HoldDate);
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 2), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[2].HoldDate);
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 3), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[3].HoldDate);
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 4), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[4].HoldDate);
            CreateInstallmentRecord(client, transaction.Id, GetPaydayLoanNextDueDate(model.FirstPaymentDay, 5), model.PaymentAmount, "ACH", model.PaymentPreference, model.Records[5].HoldDate);

            var cash = new LoanStopModel.CashLog();
            cash.Amount = -model.AmountDisbursed;
            cash.TransactionType = "Loan";
            cash.Category = "Loan";
            cash.Type = "debit";
            cash.Description = "Installment Loan";
            cash.Employee = " ";
            cash.PayableTo = client.Firstname + " " + client.Lastname;
            cash.SsNumber = client.SsNumber;
            cash.TransactionNumber = transaction.Id;
            cash.Date = model.TransDate;
            cashRepository.Add(cash);

            var ppcs = ppcRepository.GetByTransaction(transaction.Id);
            
            rtrn.Ppcs = new List<object>();
            foreach (var ppc in ppcs.OrderBy( s => s.DateDue))
            {
                var newItem = new
                {
                    Id = ppc.Id,
                    TransactionId = ppc.TransactionId,
                    SsNumber = ppc.SsNumber,
                    Name = ppc.Name,
                    CheckNumber  = ppc.CheckNumber,
                    AmountRecieved  = ppc.AmountRecieved,
                    AmountPaid  = ppc.AmountPaid,
                    OrignalAmount  = ppc.OrignalAmount,
                    DateReturned = ppc.DateReturned,
                    HoldDate = ppc.HoldDate,
                    TransDate = ppc.TransDate,
                    DateCleared = ppc.DateCleared,
                    DatePaid = ppc.DatePaid,
                    DateDue = ppc.DateDue,
                    Status = ppc.Status
                };

                rtrn.Ppcs.Add(newItem);
            }

            try
            { 
                rtrn.APR = APR(model.TransDate, model.FirstPaymentDay, Convert.ToDouble(model.AmountDisbursed),  Convert.ToDouble( model.PaymentAmount));
            }
            catch (Exception APRexception)
            { 
                logException(store, "arp", APRexception);
            }
            
            try
            { 
                rtrn.FinanceCharge = FinanceCharge(Convert.ToInt16(model.AmountDisbursed)); 
            }
            catch (Exception FinanceChargeException)
            { 
                logException(store, "FinanceCharge", FinanceChargeException);
            }
            
            try
            { 
                rtrn.PrepaidFinanceCharge = PrepaidFinanceCharge(Convert.ToInt16(model.AmountDisbursed)); 
            }
            catch (Exception PrepaidFinanceChargeException)
            { 
                logException(store, "PrepaidFinanceCharge", PrepaidFinanceChargeException);
            }


            var teletrack = new ColoradoTeleTrack();
            try
            {
                if (client.InquireCode != "TESTA")
                {
                    teletrack.IssueLoan(client, transaction, ppcs.FirstOrDefault(),  store);
                }
            }
            catch(Exception teletrackException)
            {
                logException(store, "Installment-Teletrack", teletrackException);
            }


            return rtrn;

        }
		
		#endregion

		#region public transactions
		public BounceResponse Bounce(BounceModel model)
        {

            BounceResponse result = new BounceResponse(false);
            logAction(store, "Bounce", model);

            try
            {
                string ssNumber = model.SsNumber;
                long transactionId = model.TransactionId;
                long paymentPlanCheckId = model.PaymentPlanCheckId;
                decimal paymentAmount = model.PaymentAmount;
                decimal monthlyFee = model.MonthlyFee;
                DateTime dateReturned = model.DateReturned;
                int paymentNumber = model.PaymentNumber;

                var client = clientRepository.GetBySSNumber(ssNumber);
                var transaction = transRepository.GetById(transactionId);
                var paymentPlanCheck = ppcRepository.GetById(paymentPlanCheckId);
                var ppcs = ppcRepository.GetByTransaction(transactionId);

                decimal AmountReceivedPaymentPlanChecks = paymentPlanCheck.AmountRecieved;
                decimal AmountPaidPaymentPlanChecks = paymentPlanCheck.AmountPaid;

                decimal TransactionsUpdateAmount = 0;
                decimal PaymentPlanChecksUpdateAmount = 0;
                decimal PaymentsBalanceUpdateAmount = 0;
                decimal PaymentsAmountPaidUpdateAmount = 0;

                if (AmountReceivedPaymentPlanChecks < paymentAmount)
                {
                    TransactionsUpdateAmount = decimal.Round(transaction.AmountRecieved + AmountReceivedPaymentPlanChecks, 2);
                    PaymentPlanChecksUpdateAmount = decimal.Round(AmountPaidPaymentPlanChecks - AmountReceivedPaymentPlanChecks, 2);
                    PaymentsBalanceUpdateAmount = AmountReceivedPaymentPlanChecks;
                    PaymentsAmountPaidUpdateAmount = 0;
                }
                else
                {
                    PaymentsBalanceUpdateAmount = decimal.Round(AmountReceivedPaymentPlanChecks - transaction.MonthlyFee, 2);
                    TransactionsUpdateAmount = decimal.Round(transaction.AmountRecieved + PaymentsBalanceUpdateAmount, 2);
                    PaymentPlanChecksUpdateAmount = 0;
                    PaymentsAmountPaidUpdateAmount = monthlyFee;
                }

                // only charge one fee
                decimal OtherFee = 25;

                if (ppcs != null)
                {
                    foreach (var pcc in ppcs)
                    {
                        if (pcc.Status == "Bounced" || pcc.Status == "Partial NSF" || pcc.Status == "Paid NSF")
                        {
                            if (pcc.Id != paymentPlanCheckId)
                            {
                                OtherFee = 0;
                                //logAction(store, "Bounce", string.Format("OtherFee Bounce = {0}", OtherFee.ToString("F")));
                                //logAction(store, "Bounce", string.Format("PPC id = {0}; Status = {1}", pcc.Id.ToString(), pcc.Status));
                            }
                        }
                    }
                }

                //payment table
                var payment = new LoanStopModel.Payment();
                payment.TransactionId = (int)transaction.Id;
                payment.SsNumber = client.SsNumber;
                payment.Name = client.Firstname + " " + client.Lastname;
                payment.Description = "NSF";
                payment.DatePaid = dateReturned;
                payment.AmountPaid = "-" + PaymentsAmountPaidUpdateAmount.ToString("F");
                payment.OtherFees = OtherFee.ToString("F");
                payment.PaymentNumber = paymentNumber;
                payment.PaymentType = "Fee Income";
                payment.Balance = "-" + PaymentsBalanceUpdateAmount.ToString("F");
                paymentRepository.Add(payment);

                //transaction table
                transaction.AmountRecieved = TransactionsUpdateAmount;
                transaction.Status = "Late";
                transaction.DateCleared = null;
                transRepository.Update(transaction);
                
                //paymentPlanChecks table
                paymentPlanCheck.AmountPaid = PaymentPlanChecksUpdateAmount;
                paymentPlanCheck.DatePaid = null;
                paymentPlanCheck.DateReturned = dateReturned;
                paymentPlanCheck.Status = "Bounced";
                ppcRepository.Update(paymentPlanCheck);

                if (paymentNumber < 6)
                {
                    foreach (var pcc in ppcs)
                    {
                        if (pcc.Status == "Void")
                        {
                            pcc.Status = "Pick Up";
                            ppcRepository.Update(pcc);
                        }
                    }
                }
                
                //collect table     
                var collect = collectRepository.GetByTransId(transaction.Id);
                if (collect != null)
                {
                    collect.TransactionId = transaction.Id;
                    collect.PaymentNumber = paymentNumber;
                    collect.MultipleNSF = collect.MultipleNSF + 1;
                    collect.Name = client.Firstname + " " + client.Lastname;
                    collect.SsNumber = client.SsNumber;
                    collect.Letter1 = dateReturned;
                    collect.Letter2 = null;
                    collect.Letter3 = null;
                    collect.Followup1 = null;
                    collect.Followup2 = null;
                    collect.PaymentAgreement = null;
                    collect.ACHProjectedDate = null;
                    collect.ACHSubmittedDate = null;
                    collect.ACHAmount = null;
                    collect.ACH2ProjectedDate = null;
                    collect.ACH2SubmittedDate = null;
                    collect.ACH2Amount = null;
                    collect.EmploymentVerified = null;
                    collect.EmploymentVerifiedDate = null;
                    collectRepository.Update(collect);
                }
                else
                {
                    collect = new LoanStopModel.Collect();
                    collect.TransactionId = transaction.Id;
                    collect.PaymentNumber = paymentNumber;
                    collect.MultipleNSF = 1;
                    collect.Name = client.Firstname + " " + client.Lastname;
                    collect.SsNumber = client.SsNumber;
                    collect.Letter1 = dateReturned;
                    collect.NextPmtDue = dateReturned.AddMonths(1);
                    collectRepository.Add(collect);
                }

                //client table
                client.Status = "5";
                clientRepository.Update(client);

                //checkbook
                var check = new LoanStopModel.Checkbook();
                check.CheckNumber = 0;
                check.PayableTo = "NSF " + client.Firstname + " " + client.Lastname + "  " + client.SsNumber.Substring(7);
                check.TransactionType = "Bounced";
                check.Category = "Bounced";
                check.Description = "Bounced";
                check.Amount = -AmountReceivedPaymentPlanChecks;
                check.Type = "debit";
                check.TransactionNumber = transaction.Id;
                check.SsNumber = client.SsNumber;
                checkbookRepository.Add(check);

                var teletrack = new ColoradoTeleTrack();
                try
                {
                    if (client.InquireCode != "TESTA")
                    {
                        teletrack.BouncedInstallment(client, transaction, paymentPlanCheck, store);
                    }
                }
                catch(Exception teletrackException)
                {
                    logException(store, "Bounce-Teletrack", teletrackException);
                }
                
                ppcs = ppcRepository.GetByTransaction(transactionId);

                result.Ppcs = new List<object>();
                foreach (var ppc in ppcs)
                {
                    var newItem = new
                    {
                        Id = ppc.Id,
                        TransactionId = ppc.TransactionId,
                        SsNumber = ppc.SsNumber,
                        Name = ppc.Name,
                        CheckNumber  = ppc.CheckNumber,
                        AmountRecieved  = ppc.AmountRecieved,
                        AmountPaid  = ppc.AmountPaid,
                        OrignalAmount  = ppc.OrignalAmount,
                        DateReturned = ppc.DateReturned,
                        HoldDate = ppc.HoldDate,
                        Timestamp = ppc.TimeStamp,
                        DateCleared = ppc.DateCleared,
                        DatePaid = ppc.DatePaid,
                        DateDue = ppc.DateDue,
                        Status = ppc.Status
                    };

                    result.Ppcs.Add(newItem);
                }

            }
            catch (Exception bounceException)
            {
                logException(store, "bounce", bounceException.InnerException);
                result.Error = true;
            }

            return result;
        }

        public CashCheckResponse CashCheck(CashCheckModel model)
        {
            var result = new CashCheckResponse(false);

            try
            {
                var transaction = new LoanStopModel.Transaction();
                transaction.CheckType = 1;
                transaction.DateDue = model.Date;
                if (model.CheckType == "Gold")
                {
                    transaction.AmountDispursed = model.Amount;
                    transaction.AmountRecieved = model.AmountForCustomer;
                }
                else
                {
                    transaction.AmountDispursed = model.AmountForCustomer;
                    transaction.AmountRecieved = model.Amount;
                }
                transaction.CheckNumber = int.Parse(model.CheckNumber);
                transaction.DispursedAs = "CASH";
                transaction.SsNumber = model.SsNumber;
                transaction.Name = model.Name;
                transaction.TransDate = model.Date;
                transaction.DateDue = model.Date;
                transaction.Status = "Deposit";
                transaction.Consecutive = 0;
                transaction = transRepository.Add(transaction);

                var cash = new LoanStopModel.CashLog();
                cash.Amount = -transaction.AmountDispursed;
                cash.TransactionType = "Cash Check";
                cash.Category = "Cash Check";
                cash.Type = "debit";
                cash.Description = "Cash Check";
                cash.Employee = " ";
                cash.PayableTo = model.Name;
                cash.SsNumber = model.SsNumber;
                cash.TransactionNumber = transaction.Id;
                cash.Date = DateTime.Now.Date;
                cashRepository.Add(cash);

                var checkCash = new LoanStop.DBCore.CheckCashing();
                checkCash.AccountNumber = model.AccountNumber;
                checkCash.RoutingNumber = model.RoutingNumber;
                checkCash.Name = model.Name;
                checkCash.StoreCode = store;
                checkCash.SsNumber = model.SsNumber;
                checkCash.TransId = transaction.Id;
                checkCash.Amount = model.Amount;
                checkCash.Issuer = model.Issuer;
                checkCash.Status = "OPEN";
                checkCash.Date =  DateTime.Now.Date;
                trackingRepository.SaveCheckCashing(checkCash);
            }
            catch (Exception bounceException)
            {
                logException(store, "cashcheck", bounceException);
                result.Error = true;
            }

            return result;
        
        }
 
        public void MinumimPayment(PaymentModel model)
        {

			var client = clientRepository.GetBySSNumber(model.SsNumber);
            var trans = transRepository.GetById(model.TransactionId);
			var ppc = ppcRepository.GetById(model.PpcId);
	
			decimal fee = 0;
			decimal principle = 0;
			decimal partialPaymentAmount = model.PartialPaymentAmount;

            if (model.PayoffAmount == model.AmountPaid)
            {
                fee = model.PayoffAmount - trans.AmountRecieved;
            }
            else
            {
                if (model.Status == "Partial" || model.Status == "Partial NSF")
                    fee = 0;
                else
                    fee = model.MonthlyFee + model.NSFFee;
            }

			principle = model.AmountPaid - fee;

			// payment table
			var payment = new LoanStopModel.Payment();
			payment.TransactionId = (int)trans.Id;
			payment.SsNumber = client.SsNumber;
			payment.Name = client.Firstname + " " + client.Lastname;
			payment.PaymentNumber = model.PaymentNumber;
			payment.PaymentType = "Minimum";
            payment.DatePaid = model.PaymentDate;
			payment.Balance = principle.ToString("###0.00");
			payment.Description = "Partial";
			payment.AmountPaid = fee.ToString("###0.00");
			payment.AmountDue = model.PaymentAmount.ToString("###0.00");
			paymentRepository.Add(payment);

			// cash log;
			var cashLog = new LoanStopModel.CashLog();
			cashLog.TransactionType = "Revenue";
			cashLog.Category = "Revenue";
			cashLog.Description = "Pd Cash";
			cashLog.Amount = model.AmountPaid;
			cashLog.Type = "credit";
			cashLog.PayableTo = client.Firstname + " " + client.Lastname;
			cashLog.SsNumber = client.SsNumber;
			cashLog.TransactionNumber = trans.Id;
			cashLog.Date = model.PaymentDate;

			cashRepository.Add(cashLog);

			// transaction 
			trans.AmountRecieved = trans.AmountRecieved - principle;
			transRepository.Update(trans);

			// payment plan checks
			ppc.AmountRecieved = model.PaymentAmount - model.AmountPaid - model.NSFFee;

            if (model.PaymentNumber < 6)
            {
                if (model.Status == "Bounced")
                {
                    ppc.Status = "Paid NSF";
                }
                else if (model.Status == "Partial NSF")
                {
                    ppc.Status = "Paid NSF";
                }
                else
                {
                    ppc.Status = "Pd Cash";
                }
            }
            else if (model.Status == "Bounced") 
            {
                ppc.Status = "Partial NSF";
            }

            ppc.AmountPaid = model.AmountPaid + ppc.AmountPaid;
            
			var addAmountRecieved  = 0m;
            if (model.AmountPaid < model.PaymentAmount){
                ppc.AmountRecieved = model.PaymentAmount - model.AmountPaid - model.NSFFee;
				addAmountRecieved = model.PaymentAmount - model.AmountPaid;
			}
            else
                ppc.AmountRecieved =  model.PaymentAmount;

			ppc.DatePaid = model.PaymentDate;
			ppcRepository.Update(ppc);

			// reduce amount received for last transaction
            var ppcs = ppcRepository.GetByTransaction(model.TransactionId) as List<LoanStopModel.PaymentPlanCheck>;

            var p = ppcs.ElementAt(5);
			p.AmountRecieved = p.AmountRecieved  + reCalcPPC(ppcs);
			ppcRepository.Update(p);

            // update transaction
            SetClientStatusForPaidBounce(client.SsNumber, (int)trans.Id);

            //close transaction
            if (trans.AmountRecieved < 0.05m) 
            {
                CloseTransaction(trans, model.PaymentDate, model.PpcId);
            }
        }

        public void PartialPayment(PaymentModel model)
        {
			var client = clientRepository.GetBySSNumber(model.SsNumber);
            var trans = transRepository.GetById(model.TransactionId);
			var ppc = ppcRepository.GetById(model.PpcId);
				
	
			decimal fee = 0;
			decimal principle = 0;
			decimal paymentTableBalanceAmount = 0;
			decimal partialPaymentAmount = model.PartialPaymentAmount;

            if (model.PayoffAmount == model.AmountPaid)
            {
                fee = model.PayoffAmount - trans.AmountRecieved;
                paymentTableBalanceAmount = trans.AmountRecieved;
            }
            else
            {
                //if (model.Status == "Partial" || model.Status == "Partial NSF")
                    fee = 0;
                //else
                //    fee = model.MonthlyFee + model.NSFFee;
    
                paymentTableBalanceAmount = model.AmountPaid;
            }

            principle = model.AmountPaid - fee;

	        // payment table
			var payment = new LoanStopModel.Payment();
			payment.TransactionId = (int)trans.Id;
			payment.SsNumber = client.SsNumber;
			payment.Name = client.Firstname + " " + client.Lastname;
			payment.PaymentNumber = model.PaymentNumber;
			payment.PaymentType = "Fee Income";
            payment.DatePaid = model.PaymentDate;
			
			//if (model.Status == "Partial" || model.Status == "Partial NSF"){
				payment.Balance = paymentTableBalanceAmount.ToString("###0.00");
				payment.Description = "Partial";
				payment.AmountPaid = fee.ToString("###0.00");
				payment.AmountDue = model.PaymentAmount.ToString("###0.00");
            //}
            //else{
            //    payment.Balance = principle.ToString("####0.00");
            //    payment.Description = " ";
            //    payment.AmountPaid = fee.ToString("###0.00");
            //    payment.AmountDue = model.PaymentAmount.ToString("###0.00");
            //}
			paymentRepository.Add(payment);

			// cash log;
			var cashLog = new LoanStopModel.CashLog();
			cashLog.TransactionType = "Revenue";
			cashLog.Category = "Revenue";
			cashLog.Description = "Pd Cash";
			cashLog.Amount = model.AmountPaid;
			cashLog.Type = "credit";
			cashLog.PayableTo = client.Firstname + " " + client.Lastname;
			cashLog.SsNumber = client.SsNumber;
			cashLog.TransactionNumber = trans.Id;
			cashLog.Date = model.PaymentDate;

			cashRepository.Add(cashLog);

			// transaction
			// transaction Amount Received
			if (Math.Abs((model.AmountPaid + model.PartialPaymentAmount) - (model.OrignialAmount + model.NSFFee)) <= 0.05m)
			{
				if (model.Status == "Bounced" || model.Status == "Partial NSF"){
					ppc.Status = "Paid NSF"; 
				}
				else {
					ppc.Status = "Pd Cash"; 
				}
				
				trans.Status = "Open";
				clientRepository.UpdateStatus(client.SsNumber, "0");
			}
			else{
				if (model.Status == "Pick Up" || model.Status == "Deposit"){
					ppc.Status = "Partial"; 
				}
				else if (model.Status == "Bounced"){
					ppc.Status = "Partial NSF";
				}
			}

			if (model.PaymentAmount < model.AmountPaid){
				ppc.AmountRecieved = model.PaymentAmount - model.AmountPaid - model.NSFFee;
			}
			else{
				ppc.AmountRecieved = model.AmountPaid;
			}
	
			ppc.AmountPaid = model.AmountPaid + model.PartialPaymentAmount;
            ppc.DatePaid = model.PaymentDate;

			trans.AmountRecieved = trans.AmountRecieved- principle;

			ppcRepository.Update(ppc);
			transRepository.Update(trans);

            var ppcs = ppcRepository.GetByTransaction(model.TransactionId) as List<LoanStopModel.PaymentPlanCheck>;

			var p = ppcs.ElementAt(5);
            p.AmountRecieved = p.AmountRecieved + reCalcPPC(ppcs);
			ppcRepository.Update(p);



            //teletrackPost(client, transaction, ppc, store);

            if (trans.AmountRecieved < 0.05m) 
            {
                CloseTransaction(trans, model.PaymentDate, model.PpcId);
            }

        }
        
        public void PaidCash(PaymentModel model)
        {

            var client = clientRepository.GetBySSNumber(model.SsNumber);
            var trans = transRepository.GetById(model.TransactionId);
			var ppc = ppcRepository.GetById(model.PpcId);
	
			decimal fee = 0;
			decimal principle = 0;
			decimal partialPaymentAmount = model.PartialPaymentAmount;

            if (model.PayoffAmount == model.AmountPaid)
            {
                fee = model.PayoffAmount - trans.AmountRecieved;
            }
            else
            {
                if (model.Status == "Partial" || model.Status == "Partial NSF")
                    fee = 0;
                else
                    fee = model.MonthlyFee + model.NSFFee;
   
            }

            principle = model.AmountPaid - fee;

	        // payment table
			var payment = new LoanStopModel.Payment();
			payment.TransactionId = (int)trans.Id;
			payment.SsNumber = client.SsNumber;
			payment.Name = client.Firstname + " " + client.Lastname;
			payment.PaymentNumber = model.PaymentNumber;
			payment.PaymentType = "Fee Income";
            payment.DatePaid = model.PaymentDate;
			payment.Description = "Pd Cash";

			payment.Balance = principle.ToString("###0.00");
			payment.AmountPaid = fee.ToString("###0.00");
			payment.AmountDue = model.PaymentAmount.ToString("###0.00");

			paymentRepository.Add(payment);

			// cash log;
			var cashLog = new LoanStopModel.CashLog();
			cashLog.TransactionType = "Revenue";
			cashLog.Category = "Revenue";
			cashLog.Description = "Pd Cash";
			cashLog.Amount = model.AmountPaid;
			cashLog.Type = "credit";
			cashLog.PayableTo = client.Firstname + " " + client.Lastname;
			cashLog.SsNumber = client.SsNumber;
			cashLog.TransactionNumber = trans.Id;
			cashLog.Date = model.PaymentDate;
			cashRepository.Add(cashLog);


			trans.AmountRecieved = trans.AmountRecieved - principle;
			transRepository.Update(trans);

			if (model.Status == "Bounced" || model.Status == "Partial NSF")
			{
				ppc.Status = "Paid NSF";
				SetClientStatusForPaidBounce(client.SsNumber, (int)trans.Id);
			}
			else
			{ 
				ppc.Status = "Pd Cash";
			}
			
			ppc.AmountPaid = model.AmountPaid + model.PartialPaymentAmount;
            ppc.DatePaid = model.PaymentDate;
			ppcRepository.Update(ppc);

            if (trans.AmountRecieved < 0.05m) 
            {
                CloseTransaction(trans, model.PaymentDate, model.PpcId);
            }

                       



			// close transactions
            if (trans.AmountRecieved < 0.05m) 
            {
                CloseTransaction(trans, model.PaymentDate, model.PpcId);
            }

        }

        public bool PaidPayoffAmount(PaymentModel model)
        {
			bool rtrn = true;

			var client = clientRepository.GetBySSNumber(model.SsNumber);
            var trans = transRepository.GetById(model.TransactionId);
			var ppc = ppcRepository.GetById(model.PpcId);
	
			decimal fee = 0;
			decimal principle = 0;
			decimal partialPaymentAmount = model.PartialPaymentAmount;

			try { 
				if (model.PayoffAmount == model.AmountPaid)
				{
					fee = model.PayoffAmount - trans.AmountRecieved;
				}
				else
				{
					if (model.Status == "Partial" || model.Status == "Partial NSF")
						fee = 0;
					else
						fee = model.MonthlyFee + model.NSFFee;
   
				}

				principle = model.AmountPaid - fee;

				// payment table
				var payment = new LoanStopModel.Payment();
				payment.TransactionId = (int)trans.Id;
				payment.SsNumber = client.SsNumber;
				payment.Name = client.Firstname + " " + client.Lastname;
				payment.PaymentNumber = model.PaymentNumber;
				payment.PaymentType = "Fee Income";
				payment.DatePaid = model.PaymentDate;
				payment.Description = "Payoff";

				payment.Balance = principle.ToString("###0.00");
				payment.AmountPaid = fee.ToString("###0.00");
				payment.AmountDue = model.PaymentAmount.ToString("###0.00");

				paymentRepository.Add(payment);

				// cash log;
				var cashLog = new LoanStopModel.CashLog();
				cashLog.TransactionType = "Revenue";
				cashLog.Category = "Revenue";
				cashLog.Description = "Pd Cash";
				cashLog.Amount = model.AmountPaid;
				cashLog.Type = "credit";
				cashLog.PayableTo = client.Firstname + " " + client.Lastname;
				cashLog.SsNumber = client.SsNumber;
				cashLog.TransactionNumber = trans.Id;
				cashLog.Date = model.PaymentDate;
				cashRepository.Add(cashLog);


				trans.AmountRecieved = trans.AmountRecieved - principle;
				transRepository.Update(trans);

				if (model.Status == "Bounced" || model.Status == "Partial NSF")
				{
					ppc.Status = "Paid NSF";
					SetClientStatusForPaidBounce(client.SsNumber, (int)trans.Id);
				}
				else
				{ 
					ppc.Status = "Pd Cash";
				}
			
				ppc.AmountPaid = model.AmountPaid + model.PartialPaymentAmount;
				ppc.DatePaid = model.PaymentDate;
				ppcRepository.Update(ppc);

				if (trans.AmountRecieved < 0.05m) 
				{
					CloseTransaction(trans, model.PaymentDate, model.PpcId);
				}
			}
			catch (Exception e)
			{
				logException(store, "PaidPayoffAmount", e);
				rtrn = false;
			}

			return rtrn;
        }

        public void OverPayment(PaymentModel model)
        {
            var client = clientRepository.GetBySSNumber(model.SsNumber);
            var trans = transRepository.GetById(model.TransactionId);
			var ppcs = ppcRepository.GetByTransaction(model.TransactionId);
			var ppc = ppcs.ToArray()[model.PaymentNumber - 1];

			decimal fee = 0;
			decimal principle = 0;
			decimal overPaymentAmount = 0;
			decimal paymentTableAmountPaid = 0;
			decimal paymentTableBalanceAmount = 0;
			decimal partialPaymentAmount = 0;

			if (model.Status == "Partial" || model.Status == "Partial NSF" )
			{
				partialPaymentAmount = ppc.AmountPaid;
			}

			overPaymentAmount = (model.AmountPaid + partialPaymentAmount) - model.PaymentAmount; 

			if (model.PayoffAmount == model.AmountPaid){ 
				fee = model.PayoffAmount - trans.AmountRecieved;
				paymentTableAmountPaid = model.AmountPaid - trans.AmountRecieved;
				paymentTableBalanceAmount = trans.AmountRecieved;
			}
			else{
				fee = model.MonthlyFee + model.NSFFee;
				paymentTableAmountPaid = 0;
				paymentTableBalanceAmount = model.AmountPaid;
			}

			principle = model.AmountPaid - fee;

	        // payment table
			var payment = new LoanStopModel.Payment();
			payment.TransactionId = (int)trans.Id;
			payment.SsNumber = client.SsNumber;
			payment.Name = client.Firstname + " " + client.Lastname;
			payment.PaymentNumber = model.PaymentNumber;
			payment.PaymentType = "Fee Income";
            payment.DatePaid = model.PaymentDate;
			
			if (model.Status == "Partial" || model.Status == "Partial NSF" ){
				payment.Balance = paymentTableBalanceAmount.ToString("###0.00");
				payment.Description = "Partial";
				payment.AmountPaid = paymentTableAmountPaid.ToString("###0.00");
				payment.AmountDue = model.PaymentAmount.ToString("###0.00");
			}
			else{
				payment.Balance = principle.ToString("####0.00");
				payment.Description = " ";
				payment.AmountPaid = fee.ToString("###0.00");
				payment.AmountDue = model.PaymentAmount.ToString("###0.00");
			}
			
			paymentRepository.Add(payment);


			// cash log;
			var cashLog = new LoanStopModel.CashLog();
			cashLog.TransactionType = "Revenue";
			cashLog.Category = "Revenue";
			cashLog.Description = "Pd Cash";
			cashLog.Amount = model.AmountPaid;
			cashLog.Type = "credit";
			cashLog.PayableTo = client.Firstname + " " + client.Lastname;
			cashLog.SsNumber = client.SsNumber;
			cashLog.TransactionNumber = trans.Id;
			cashLog.Date = model.PaymentDate;
			cashRepository.Add(cashLog);

			// transaction
			// transaction Amount Received
			if (model.Status == "Partial" || model.Status == "Partial NSF") {
				trans.AmountRecieved =  trans.AmountRecieved  - model.AmountPaid;
			}
			else{
				trans.AmountRecieved  = trans.AmountRecieved - principle;
			}
			
			if (model.PayoffAmount == model.AmountPaid) 
			{
				trans.AmountRecieved = 0;
                trans.DateCleared = model.PaymentDate;
				trans.Status = "Closed";
			}
			
			if (model.AmountPaid + partialPaymentAmount >= model.PaymentAmount)
			{
				if (model.Status == "Bounced" || model.Status == "Partial NSF" || model.Status == "Partial") 
				{
					trans.Status = "Open";
				}
			}
			
			transRepository.Update(trans);


			// payment plan checks
			if (model.AmountPaid + partialPaymentAmount >= model.PaymentAmount)
			{
				if (model.Status == "Bounced") 
				{
					ppc.Status = "Paid NSF";

					clientRepository.UpdateStatus(client.SsNumber, "0");

				}
				else if (model.Status == "Partial NSF")
				{
					ppc.Status = "Paid NSF";

					clientRepository.UpdateStatus(client.SsNumber, "0");

				}
				else
				{
					ppc.Status = "Pd Cash";
				}
			}
			else
			{
				if (model.Status == "Pick Up" || model.Status == "Deposit" )
				{
					ppc.Status = "Partial";
				}
				else if (model.Status == "Bounced" )
				{
					ppc.Status = "Partial NSF";
				}
			}

			decimal reduceAmountRecieved = overPaymentAmount;

			for (var i = 5; i>0; i-- ){
				if ((overPaymentAmount - decimal.Parse(ppcs.ElementAt(i).OrignalAmount)) > 0) {
										
					ppcs.ElementAt(i).AmountRecieved = 0m;
					
					overPaymentAmount = overPaymentAmount - decimal.Parse(ppcs.ElementAt(i).OrignalAmount);
									
					ppcRepository.Update(ppcs.ElementAt(i));

				}
				else{
                    reduceAmountRecieved = decimal.Parse(ppcs.ElementAt(i).OrignalAmount) - overPaymentAmount;
					
					ppcs.ElementAt(i).AmountRecieved = reduceAmountRecieved;

					ppcRepository.Update(ppcs.ElementAt(i));

					break;
				}
				

			}


			ppc.AmountPaid = model.AmountPaid + partialPaymentAmount;

            ppc.DatePaid = model.PaymentDate;
			ppcRepository.Update(ppc);


			// close transaction 
			if (trans.AmountRecieved < 0.05m )
			{
                CloseTransaction(trans, model.PaymentDate, model.PpcId);
			}

        }
		
		#endregion

		#region public methods
		public NewInstallmentLoanResponse AdjustHoldDates(string store, AdjustHoldDatesModel model)
        {
            //logAction(store, "AdjustHoldDates", model);
            
            var rtrn = new NewInstallmentLoanResponse(false);

            foreach (var holdDate in model.Records)
            {
                var item = ppcRepository.GetById(holdDate.Id.Value);
                item.HoldDate = holdDate.HoldDate;
                ppcRepository.Update(item);
            }

            return rtrn;
        }

        public List<object> GetPaymentPlanRecords(long transactionId)
        {
            var result = new List<object>();

            var ppcs = ppcRepository.GetByTransaction(transactionId);

            foreach (var ppc in ppcs.OrderBy( s=> s.DateDue))
            {
                var newItem = new
                {
                    Id = ppc.Id,
                    TransactionId = ppc.TransactionId,
                    SsNumber = ppc.SsNumber,
                    Name = ppc.Name,
                    CheckNumber  = ppc.CheckNumber,
                    AmountRecieved  = ppc.AmountRecieved,
                    AmountPaid  = ppc.AmountPaid,
                    OrignalAmount  = ppc.OrignalAmount,
                    DateReturned = ppc.DateReturned,
                    HoldDate = ppc.HoldDate,
                    Timestamp = ppc.TimeStamp,
                    DateCleared = ppc.DateCleared,
                    DatePaid = ppc.DatePaid,
                    DateDue = ppc.DateDue,
                    Status = ppc.Status
                };

                result.Add(newItem);
            }

            return result;

        }
		
		public void SetClientStatusForPaidBounce(string ssNumber, int transId)
        {
            var trans = transRepository.GetById(transId);

            bool bStillBounced = false;
            
            var ppcs = ppcRepository.GetByTransaction(transId);

            var bounced = ppcs.Where(w => w.Status == "Bounced").FirstOrDefault(); 

            if (bounced != null) bStillBounced = true;

            if (bStillBounced)
            {
                trans.Status = "Late";
                transRepository.Update(trans);
            }
            else
            {
                trans.Status = "Open";
                transRepository.Update(trans);

                var client = clientRepository.GetBySSNumber(ssNumber);
                client.Status = "0";
                clientRepository.Update(client);
            }
        }

        public void SetTransactionStatus(string ssNumber, int transId)
        {
            var trans = transRepository.GetById(transId);

            if (trans.Status == "Late")
            { 
                bool bStillBounced = false;
            
                var ppcs = ppcRepository.GetByTransaction(transId);

                var bounced = ppcs.Where(w => w.Status == "Bounced").FirstOrDefault(); 

                if (bounced != null) bStillBounced = true;

                if (bStillBounced)
                {
                    trans.Status = "Late";
                    transRepository.Update(trans);
                }
                else
                {
                    trans.Status = "Open";
                    transRepository.Update(trans);

                    var client = clientRepository.GetBySSNumber(ssNumber);
                    client.Status = "0";
                    clientRepository.Update(client);
                }
            }
        }

		public List<PaymentEntity> GetPayments(long transactionId)
        {
            var result = new List<PaymentEntity>();

//            var paymentRepository = new Repository.Payment(connectionString);

            var payments = paymentRepository.GetAllForTransaction(transactionId);

            foreach (var payment in payments)
            {
                var newItem = new PaymentEntity()
                {
                    Id = payment.Id,
                    TransactionId = payment.TransactionId,
                    Name  = payment.Name,
                    SsNumber = payment.SsNumber,
                    Description = payment.Description,
                    DateDue = payment.DateDue,
                    DatePaid = payment.DatePaid,
                    AmountDue = payment.AmountDue != "" ? decimal.Parse(payment.AmountDue) : 0,
                    AmountPaid = payment.AmountPaid != "" ? decimal.Parse(payment.AmountPaid) : 0,
                    OtherFees = payment.OtherFees != "" ? decimal.Parse(payment.OtherFees) : 0,
                    Balance = payment.Balance != "" ? decimal.Parse(payment.Balance) : 0,
                };

                result.Add(newItem);
            }

            return result;

        }

		//return bounced amount from check when status set to bounced
		public decimal GetBouncedAmount(int ppcId)
		{
			var ppc = ppcRepository.GetById(ppcId);
			return decimal.Parse(ppc.OrignalAmount);
		}

		#endregion

		#region private
		//private TransactionResponse GetFullTransaction(long TransId)

		//private void payment(LoanStopModel.Client client, LoanStopModel.Transaction trans, string type)
		//{

		//	//payment table
		//	var payment = new LoanStopModel.Payment();
		//	payment.TransactionId = (int)trans.Id;
		//	payment.SsNumber = client.SsNumber;
		//	payment.Name = client.Firstname + " " + client.Lastname;
		//	payment.Description = type; //"Minimum";
		//	payment.DatePaid = this.date;
		//	payment.AmountPaid = this.updateFee.ToString();
		//	payment.AmountDue = this.paymentAmount.ToString();
		//	payment.Balance = updateBalance.ToString();
		//	payment.PaymentNumber = (int)this.paymentNumber;
		//	payment.PaymentType = "Fee Income";
		//	paymentRepository.Add(payment);

		//	//Cash Log
		//	var cash = new LoanStopModel.CashLog();
		//	cash.TransactionType = "Revenue";
		//	cash.Category = "Revenue";
		//	cash.Description = "Pd Cash";
		//	cash.Amount =  this.amountPaid;
		//	cash.TransactionType  = "credit";
		//	cash.PayableTo = client.Firstname + " " + client.Lastname;
		//	cash.SsNumber =client.SsNumber;
		//	cash.TransactionNumber = trans.Id;
		//	cashRepository.Add(cash);

		//	// update transaction
		//	trans.AmountRecieved = this.updateTransactionAmountRecieved;
		//	transRepository.Update(trans);

		//	// update paymentPlanChecks table
		//	for (var i=0; i < 6; i++ )
		//	{ 
		//		var ppc = trans.PaymentPlanChecks[i];

		//		if (i == this.paymentNumber)
		//		{ 
		//			ppc.AmountRecieved = this.updatePpcAmountRecieved;
		//			ppc.AmountPaid = this.updatePaymentPlanChecksAmount;
		//			ppc.DatePaid =  this.date;
		//			ppc.Status = this.updatePPCStatus;
		//			ppcRepository.Update(ppc);
		//		}
		//		else 
		//		{ 
		//			ppcRepository.Update(ppc);
		//		}

		//	}

		//	//update client
		//	clientRepository.Update(client);

		//}
        
        private void CloseTransaction(LoanStopModel.Transaction trans, DateTime closedDate, long ppcId)
        {
            trans.Status =  "Closed";
            trans.DateCleared = closedDate;
            transRepository.Update(trans);

            var ppcs = ppcRepository.GetByTransaction(trans.Id);

            foreach (var ppc in ppcs)
            {
                if (ppc.Status == "Pick Up" || ppc.Status == "Deposit")
                {
                    ppc.Status = "Void";
                }

                if (ppc.Id == ppcId)
                {
                    ppc.Status = "Pd Cash";
                }

                ppcRepository.Update(ppc);
            }
        }

        private decimal APR(DateTime TransDate, DateTime FirstPaymentDay, double AmountFinanced, double Payment)
        {
            
            int N3;
            double Rtrn;

            N3 = TransDate.AddMonths(1).Subtract(FirstPaymentDay).Days;
            //N3 = FirstPaymentDay.Subtract(TransDate.AddMonths(1)).Days;

            double Adjustment = Financial.Rate(6, -Payment, AmountFinanced, 0);

            Rtrn = (Financial.Rate(6, -Payment, AmountFinanced * (Adjustment * 12) / 365 * N3 + AmountFinanced, 0) * 12) * 100;

            return Convert.ToDecimal(Rtrn);
        }

        private decimal FinanceCharge(int AmountDispursed)
        { 
            decimal financeCharge = 0;

            switch (AmountDispursed)
            {
                case 100:
                    financeCharge = 71.0m;
                    break;
                case 200:
                    financeCharge = 142.06m;
                    break;
                case 300:
                    financeCharge = 213.06m;
                    break;
                case 400:
                    financeCharge = 271.58m;
                    break;
                case 500:
                    financeCharge = 292.66m;
                    break;
            }

            return financeCharge;
        }

        private decimal PrepaidFinanceCharge(int AmountDispursed)
        {
            decimal prepaidFinanceCharge = 0;
            
            if (AmountDispursed <= 300)
            {
                prepaidFinanceCharge = (AmountDispursed * 0.2m);
            }
            else
                prepaidFinanceCharge = (300m * 0.2m) + ((AmountDispursed - 300m) * 0.075m);

            return prepaidFinanceCharge;
        }
 
        private void CreateInstallmentRecord(LoanStopModel.Client client, long TransactionId, DateTime PaymentDay, decimal MonthlyPayment,string CheckNumber, string PaymentPreference, DateTime? holdDate)
        {
//            Repository.PaymentPlanCheck ppcRepository = new Repository.PaymentPlanCheck(ConnectionString);

            var ppc = new LoanStopModel.PaymentPlanCheck();

            ppc.TransactionId = TransactionId;
            ppc.SsNumber = client.SsNumber;
            ppc.Name =  client.Firstname + " " + client.Lastname;
            ppc.CheckNumber  = CheckNumber;
            ppc.AmountRecieved  = MonthlyPayment;
            ppc.AmountPaid  = 0.00m;
            ppc.OrignalAmount  = MonthlyPayment.ToString();
            ppc.DateReturned = null;
            ppc.HoldDate = holdDate;
            ppc.DateCleared = null;
            ppc.DatePaid = null;
            ppc.DateDue = PaymentDay;
            ppc.Status = PaymentPreference;
            ppc.TransDate = DateTime.Now.Date;

            ppcRepository.Add(ppc);

        }

        private DateTime GetPaydayLoanNextDueDate(DateTime date, int paymentNumber)
        {
            return date.AddMonths(paymentNumber);
        }      

        private void teletrackPost(ClientEntity client, Transaction transaction, string store)
        {

            var teletrack = new ColoradoTeleTrack();
            try
            {
                //if (client.InquireCode != "TESTA")
                //{
                //    teletrack.Payment(client, transaction, ppc,  store);
                //}
            }
            catch(Exception teletrackException)
            {
                logException(store, "Installment-Teletrack", teletrackException);
            }

        }       


		private decimal reCalcPPC(List<LoanStopModel.PaymentPlanCheck> ppcs)
		{
			decimal difference = 0;
			bool dFeeSet = false;
			decimal sumOfPayments = 0;
			decimal sumDue = 0;
			int paymentNumber = 0;

			foreach(var p in ppcs)
			{
				paymentNumber++;
				decimal fee = 0m;

				if (p.Status == "Bounced" || p.Status == "Paid NSF" || p.Status == "Partial NSF" ){
                    if (!dFeeSet){
						fee = 25m;
						dFeeSet = true;
					}
                }

                if (p.Status == "Deposit" || p.Status == "Pick Up"){
                    sumOfPayments = sumOfPayments + decimal.Parse(p.OrignalAmount);
				}
                else if (p.Status == "Partial")
                {
                    if (paymentNumber < 6)
                        sumOfPayments = sumOfPayments + decimal.Parse(p.OrignalAmount);
                    else if (paymentNumber == 6)
                        sumOfPayments = sumOfPayments + p.AmountPaid;
                }
                else if (p.Status == "Partial NSF" || p.Status == "Bounced")
                {
                    if (paymentNumber < 6)
                        sumOfPayments = sumOfPayments + decimal.Parse(p.OrignalAmount) + fee;
                    else if (paymentNumber == 6)
                        sumOfPayments = sumOfPayments + p.AmountPaid + fee;
                }
                else if (p.Status == "Paid NSF")
                    sumOfPayments = sumOfPayments + p.AmountPaid;
                else
                    sumOfPayments = sumOfPayments + p.AmountPaid;


                sumDue = sumDue + decimal.Parse(p.OrignalAmount)+ fee;


			}

			difference = sumDue - sumOfPayments;

			return difference;

		}


        private void logAction(string store, string action, object model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}-{1}","INFORMATION", action);
            string jsonData = JsonConvert.SerializeObject(model);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = store;
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch
            {}

        }
 
        private void logException(string store, string action, Exception model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}-{1}", "ERROR", action);
            string jsonData = JsonConvert.SerializeObject(model.Message);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = store;
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch
            { }
        }
		#endregion

    }
}
