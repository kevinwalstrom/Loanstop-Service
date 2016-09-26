using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

using LoanStop.DBCore;
using LoanStopModel;

namespace LoanStop.DepositService.DataContracts
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public partial class DepositEntity : ICloneable
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CheckNumber { get; set; }
        [DataMember]
        public decimal DepositAmount { get; set; }
        [DataMember]
        public bool Deposit { get; set; }
        [DataMember]
        public long PaymentPlanCheckId { get; set; }
        [DataMember]
        public long TransactionId { get; set; }
        [DataMember]
        public string CheckType { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public decimal AmountRecieved { get; set; }
        [DataMember]
        public DateTime? DateDue { get; set; }
        [DataMember]
        public string RoutingNumber { get; set; }

        [DataMember]
        public Transaction Transaction { get; set; }

        [DataMember]
        public Client Client { get; set; }

        public DepositEntity()
        { }

        protected DepositEntity(DepositEntity another)
        {
            this.Name = another.Name;
            this.CheckNumber = another.CheckNumber;
            this.DepositAmount = another.DepositAmount;
            this.Deposit = another.Deposit;
            this.PaymentPlanCheckId = another.PaymentPlanCheckId;
            this.TransactionId = another.TransactionId;
            this.CheckType = another.CheckType;
            this.Status = another.Status;
            this.AmountRecieved = another.AmountRecieved;
            this.DateDue = another.DateDue;
            this.RoutingNumber = another.RoutingNumber;
            //this.Transaction = another.Transaction;
            //this.Client = another.Client;
        }


        [IgnoreDataMember]
        public bool IsValidLoanForDeposit
        {
            get
            {
                bool rtrnValue = false;

                if (IsValid())
                {
                    long routingNumber;
                    if (long.TryParse(RoutingNumber, out routingNumber) && RoutingNumber.Length == 9)
                    {
                        rtrnValue = true;
                    }
                }
                return rtrnValue;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public bool IsValidCheckCashForDeposit
        {
            get
            {
                bool rtrnValue = false;

                if (IsValid())
                {
                    long routingNumber;
                    if (!long.TryParse(RoutingNumber, out routingNumber))
                    {
                        if (CheckType == "1")
                            rtrnValue = true;
                    }
                }
                return rtrnValue;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        [IgnoreDataMember]
        public bool IsClosedAccount
        {
            get
            {
                bool rtrnValue = false;

                if (IsValid())
                {
                    long routingNumber;
                    if (!long.TryParse(RoutingNumber, out routingNumber) || RoutingNumber.Length != 9)
                    {
                        if (CheckType != "1")
                            rtrnValue = true;
                    }
                }
                return rtrnValue;
            }

        }

        private bool IsValid()
        {
            bool rtrnValue = false;

            if (DateDue.HasValue)
            {
                if (Status == "Pick Up" || Status == "Deposit" || Status == "Pick Up-NC" || Status == "Partial")
                {
                    if (Status != "Voided")
                    {
                        if (Status != "Void")
                        {
                            rtrnValue = true;
                        }
                    }
                }
            }
            return rtrnValue;
        }

        public object Clone()
        {
            return new DepositEntity(this);
        }

    }

    [DataContract]
    public class GetDepositsResponse
    {
        [DataMember]
        public List<DepositEntity> ACHDeposits { get; set; }
        [DataMember]
        public List<DepositEntity> ClosedAccounts { get; set; }
    }
}

