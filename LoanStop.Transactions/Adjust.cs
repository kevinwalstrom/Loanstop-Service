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
    public class Adjust
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
        
        protected string updatePPCStatus;
        protected string updateTransactionStatus;
        protected string updateClientStatus;
        protected decimal updateFee = 0m;
        protected decimal updateBalance = 0m;
        protected decimal updateTransactionAmountRecieved = 0m;
        protected decimal updatePaymentPlanChecksAmount = 0m;
        protected decimal updatePpcAmountRecieved = 0m;

        protected long paymentPlanCheckId;

        protected decimal paymentAmount;
        protected decimal amountPaid;
        protected decimal orignalAmount;
        protected int paymentNumber;
        protected DateTime date;

        public Adjust(string store, string connectionString, DefaultType defaults)
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
    
        public AdjustResponse FetchColorado(int id) 
        {
            var rtrn = new AdjustResponse();

            rtrn.Transaction = transRepository.GetById(id);

            if (rtrn.Transaction == null) return null;

            var ppcs = ppcRepository.GetByTransaction(id);
            if (ppcs != null) { 
                rtrn.ppcs = new List<PaymentPlanCheckEntity>();
                foreach (var p in ppcs)
                {
                    var newItem = new PaymentPlanCheckEntity()
                    {
                        Id = p.Id,
                        TransactionId = p.TransactionId,
                        SsNumber = p.SsNumber,
                        Name = p.Name,
                        Issuer = "",
                        CheckNumber = p.CheckNumber,
                        AmountRecieved = p.AmountRecieved,
                        AmountPaid = p.AmountPaid,
                        OrignalAmount = p.OrignalAmount, 
                        TransDate = p.TransDate.Value, 
                        DateCleared = p.DateCleared.HasValue ? p.DateCleared.Value : p.DateCleared, 
                        DateReturned = p.DateReturned.HasValue ? p.DateReturned.Value : p.DateReturned, 
                        DatePaid = p.DatePaid.HasValue ? p.DatePaid.Value : p.DatePaid 
                    };

                    rtrn.ppcs.Add(newItem);
                }
            }
            var payments = paymentRepository.GetAllForTransaction(id);
            if (payments != null) { 
                rtrn.payments = new List<PaymentEntity>();
                foreach (var p in payments)
                {
                    var newItem = new PaymentEntity()
                    {
                        DateDue = p.DateDue,
                        TransactionId = p.TransactionId,
                        SsNumber = p.SsNumber, 
                        Name = p.Name,
                        Balance = p.Balance != null ? decimal.Parse(p.Balance) : 0,
                        Description = p.Description,
                        DatePaid = p.DatePaid,
                        AmountPaid = p.AmountPaid != null ? decimal.Parse(p.AmountPaid): 0,
                        AmountDue = p.AmountDue != null ? decimal.Parse(p.AmountDue): 0,
                        PaymentNumber = p.PaymentNumber.HasValue ? p.PaymentNumber.Value : (int?)null,
                        PaymentType = p.PaymentType,
                        OtherFees = p.OtherFees != null ? decimal.Parse(p.OtherFees) : (decimal?)null
                    };

                    rtrn.payments.Add(newItem);
                }
            }

            var checks = checkbookRepository.GetAllForTransaction(id);
            if (checks != null) { 
                rtrn.checks = new List<CheckEntity>();
                foreach (var c in checks)
                {
                    var newItem = new CheckEntity()
                    {
                        DateEntered = c.DateEntered,
                        DateTime = c.DateTime,
                        Category = c.Category,
                        TransactionType = c.TransactionType,
                        Description = c.Description,
                        Amount = c.Amount,
                        Type = c.Type,
                        PayableTo = c.PayableTo,
                        SsNumber = c.SsNumber,
                        TransactionNumber = c.TransactionNumber
                    };

                    rtrn.checks.Add(newItem);
                }
            }

            var cash = cashRepository.GetAllForTransaction(id);
            if (cash != null) { 
                rtrn.cash = new List<CashEntity>();
                foreach (var c in cash)
                {
                    var newItem = new CashEntity()
                    {
                        Category = c.Category,
                        TransactionType = c.TransactionType,
                        Description = c.Description,
                        Amount = c.Amount,
                        Type = c.Type,
                        PayableTo = c.PayableTo,
                        SsNumber = c.SsNumber,
                        TransactionNumber = c.TransactionNumber,
                        Date = c.Date,
                    };

                    rtrn.cash.Add(newItem);
                }
            }

            return rtrn;
        }
    
    
    
    }
}
