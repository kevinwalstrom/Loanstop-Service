using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public class Collect
    {
        public string ConnectionString {get; set;}

        public Collect(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        public Entity.Collect GetByTransId(long id)
        {
            Entity.Collect rtrnCollect = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Collects.Where(s => s.TransactionId == id);

                foreach (var collect in query)
                {
                    rtrnCollect = new Entity.Collect()
                    {
                        TransactionId = collect.TransactionId,
                        PaymentNumber = collect.PaymentNumber,
                        MultipleNSF = collect.MultipleNSF,
                        Name = collect.Name,
                        SsNumber = collect.SsNumber,
                        Letter1 = collect.Letter1,
                        Letter2 = collect.Letter2,
                        Letter3 = collect.Letter3,
                        RightToCure = collect.RightToCure,
                        Followup1 = collect.Followup1,
                        Followup2 = collect.Followup2,
                        NextPmtDue = collect.NextPmtDue,
                        PaymentAgreement = collect.PaymentAgreement,
                        ACHProjectedDate = collect.ACHProjectedDate,
                        ACHSubmittedDate = collect.ACHSubmittedDate,
                        ACHAmount = collect.ACHAmount,
                        OutsoucedDate = collect.OutsoucedDate,
                        OutsourcedName = collect.OutsourcedName,
                        BankVerified = collect.BankVerified,
                        EmploymentVerified = collect.EmploymentVerified,
                        EmploymentVerifiedDate = collect.EmploymentVerifiedDate,
                        ACH2ProjectedDate = collect.ACH2ProjectedDate,
                        ACH2SubmittedDate = collect.ACH2SubmittedDate,
                        ACH2Amount = collect.ACH2Amount
                    };
                }

            }

            return rtrnCollect;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(Entity.Collect collect)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Collects.Add(collect);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Update(Entity.Collect collect)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var updateItem = db.Collects.Where(s => s.TransactionId == collect.TransactionId).FirstOrDefault();

                updateItem.TransactionId = collect.TransactionId;
                updateItem.PaymentNumber = collect.PaymentNumber;
                updateItem.MultipleNSF = collect.MultipleNSF;
                updateItem.Name = collect.Name;
                updateItem.SsNumber = collect.SsNumber;
                updateItem.Letter1 = collect.Letter1;
                updateItem.Letter2 = collect.Letter2;
                updateItem.Letter3 = collect.Letter3;
                updateItem.RightToCure = collect.RightToCure;
                updateItem.Followup1 = collect.Followup1;
                updateItem.Followup2 = collect.Followup2;
                updateItem.NextPmtDue = collect.NextPmtDue;
                updateItem.PaymentAgreement = collect.PaymentAgreement;
                updateItem.ACHProjectedDate = collect.ACHProjectedDate;
                updateItem.ACHSubmittedDate = collect.ACHSubmittedDate;
                updateItem.ACHAmount = collect.ACHAmount;
                updateItem.OutsoucedDate = collect.OutsoucedDate;
                updateItem.OutsourcedName = collect.OutsourcedName;
                updateItem.BankVerified = collect.BankVerified;
                updateItem.EmploymentVerified = collect.EmploymentVerified;
                updateItem.EmploymentVerifiedDate = collect.EmploymentVerifiedDate;
                updateItem.ACH2ProjectedDate = collect.ACH2ProjectedDate;
                updateItem.ACH2SubmittedDate = collect.ACH2SubmittedDate;
                updateItem.ACH2Amount = collect.ACH2Amount;
                db.SaveChanges();
            }
        }
    }
}
