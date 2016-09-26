using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace LoanStop.Entities
{
    [DataContract]
    [Serializable]
    public class DepositEntity 
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
        public TransactionEntity Transaction { get; set; }

        [DataMember]
        public ClientEntity Client { get; set; }

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
                    if (!long.TryParse(RoutingNumber, out routingNumber))
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

        //public object Clone()
        //{
        //    return new DepositEntity(this);
        //}

    }

}
