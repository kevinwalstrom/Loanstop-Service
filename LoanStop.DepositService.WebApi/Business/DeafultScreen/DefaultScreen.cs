using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Services.WebApi.Business.Colorado;

namespace LoanStop.Services.WebApi.Business
{
    public class DefaultScreen
    {
        private string connectionString;

        //private DefaultType defaults;

        /// <summary> ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        public DefaultScreen(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary> ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        public object GetTransactionAndPayments(long transId)
        {
            //transaction
            var transactionRepository = new LoanStop.DBCore.Repository.Transaction(connectionString);
            var trans = transactionRepository.GetById(transId);

            //payments
            var paymentRepository = new LoanStop.DBCore.Repository.Payment(connectionString);
            var payments = paymentRepository.GetPaymentNumberEqualsNullForTransaction(transId);

            var Item = new
            {
                transaction = trans,
                payments = payments,
                orignationFee = LoanUtilities.OrignationFee(trans.AmountDispursed),
                interest = LoanUtilities.Interest(trans.AmountDispursed),
                serviceFee = LoanUtilities.ServiceFee(trans.AmountDispursed),
                nSFFee = LoanUtilities.NSFFee(trans.PaymentPlanChecks),
                orignationEarned = LoanUtilities.OrignationEarned(trans, DateTime.Now),
                appliedInterest = LoanUtilities.AppliedInterest(trans, DateTime.Now),
                appliedServiceFees = LoanUtilities.AppliedServiceFees(trans, DateTime.Now),
                sumOfPayments = LoanUtilities.SumOfPayments(trans.PaymentPlanChecks),
                finalDueDate = trans.PaymentPlanChecks[5].DateDue,
                totalContracted = trans.AmountDispursed + LoanUtilities.OrignationFee(trans.AmountDispursed) + LoanUtilities.Interest(trans.AmountDispursed) + (LoanUtilities.ServiceFee(trans.AmountDispursed) * 5) + LoanUtilities.NSFFee(trans.PaymentPlanChecks),
                payoffAmount = LoanUtilities.PayoffAmount(trans, DateTime.Now)
            };

            var rtrn = new
            {
                error = "success",
                Item = Item
            };

            return rtrn;
        }

        /// <summary> ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// 
        public int UpdateDefaultedTransaction(
            string ssNumber,
            long transId,
            decimal amount,
            decimal balance,
            string description,
            DateTime datePaid,
            string paymentType,
            decimal otherFees,
            bool bReceivedCash,
            bool bChangeClientStatusToPdDefaut
        )
        {
            var rtrn = 0;

            var paymentRepository = new LoanStop.DBCore.Repository.Payment(connectionString);
            var cashRepository = new LoanStop.DBCore.Repository.CashLog(connectionString);
            var transactionRepository = new LoanStop.DBCore.Repository.Transaction(connectionString);
            var clientRepository = new LoanStop.DBCore.Repository.Client(connectionString);


            var client = clientRepository.GetBySSNumber(ssNumber);
            var trans = transactionRepository.GetById(transId);


            var paymentModel = new LoanStopModel.Payment();
            //paymentModel.DateDue =;
            paymentModel.TransactionId = trans.Id;
            paymentModel.SsNumber = trans.SsNumber;
            paymentModel.Name = trans.Name;
            paymentModel.Balance = balance.ToString();
            paymentModel.Description = description;
            paymentModel.DatePaid = datePaid;
            paymentModel.AmountPaid = amount.ToString();
            //paymentModel.AmountDue =;
            //paymentModel.PaymentNumber =;
            paymentModel.PaymentType = paymentType;
            paymentModel.OtherFees = otherFees.ToString();

            paymentRepository.Add(paymentModel);

            var cashModel = new LoanStopModel.CashLog();

            cashModel.TransactionType = bReceivedCash ? "Payment" : "Applied Payment";
            cashModel.Category = bReceivedCash ? "Revenue" : "Corporate";
            cashModel.Description = bReceivedCash ? "Pd Cash" : "Applied Payment";
            cashModel.Amount = bReceivedCash ? amount : 0;
            cashModel.Type = "Credit";
            cashModel.PayableTo = client.Firstname + " " + client.Lastname;
            cashModel.SsNumber = client.SsNumber;
            cashModel.TransactionNumber = trans.Id;

            cashRepository.Add(cashModel);

            //change transaction status to 'Pd Default'
            if (bChangeClientStatusToPdDefaut)
            {

                trans.Status = "Pd Default";

                transactionRepository.Update(trans);

                clientRepository.UpdateStatus(ssNumber, "1"); //"1" '  'Pd Default'
            }

            return rtrn;
        }

        public decimal CoPayoffAmount(long transId, DateTime theDate)
        {
            var transactionRepository = new LoanStop.DBCore.Repository.Transaction(connectionString);
            var trans = transactionRepository.GetById(transId);

            return LoanUtilities.PayoffAmount(trans, theDate);
        }
    }
}