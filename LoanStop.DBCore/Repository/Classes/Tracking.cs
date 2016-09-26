using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanStop.Entities;
using LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public class Tracking
    {

        public CheckCashing GetCheckCashing(long id)
        {

            var item = new CheckCashing();

            using (var db = new TrackingDb())
            {

                item = db.CheckCashings.Find(id);
            }

            return item;
        }

        public CustomerInquirie GetCustomerInquirie(long id)
        {

            var item = new CustomerInquirie();

            using (var db = new TrackingDb())
            {

                item = db.CustomerInquiries.Find(id);
            }

            return item;
        }

        public long SaveCheckCashing(CheckCashing entity)
        {

            var item = new CheckCashing();

            using (var db = new TrackingDb())
            {

                db.CheckCashings.Add(entity);
                db.SaveChanges();

            }

            return entity.Id;
        }


        public long SaveCustomerInquirie(CustomerInquirie entity)
        {

            using (var db = new TrackingDb())
            {

                db.CustomerInquiries.Add(entity);
                db.SaveChanges();

            }

            return entity.Id;
        }

        public List<CheckCashingRecordsEntity> ClientCheckCashing(string accountNumber)
        {

            var returnList = new List<CheckCashingRecordsEntity>();

            using (var db = new TrackingDb())
            {

                var results = db.CheckCashings.Where(s => s.AccountNumber == accountNumber);

                foreach (var item in results)
                {
                    var addItem = new CheckCashingRecordsEntity() {
                        Issuer = item.Issuer,
                        IssuerName = item.Name,
                        TransDate = item.Date,
                        Amount = item.Amount,
                        Status = item.Status,
                    };

                    returnList.Add(addItem);
                }

            }

            return returnList;
        }

        public decimal TotalCashed(string accountNumber)
        {

            var returnValue = 0m;

            using (var db = new TrackingDb())
            {
                returnValue = db.CheckCashings.Where(s => s.AccountNumber == accountNumber).Select(t => t.Amount).Sum();
            }

            return returnValue;
        }

        public decimal TotalCashedLast5Days(string accountNumber, DateTime theDate)
        {

            var returnValue = 0m;

            using (var db = new TrackingDb())
            {
                returnValue = db.CheckCashings.Where(s => s.AccountNumber == accountNumber && s.Date > theDate.AddDays(-5)).Select(t => t.Amount).Sum();
            }

            return returnValue;
        }

        public IDictionary<string, string> CheckSsNumber(string ssNumber)
        {
            IDictionary<string, string> rtrn = new Dictionary<string, string>();

            using (var db = new MasterDb())
            {
                var client = db.MasterClients.Where(s => s.SsNumber == ssNumber);
            
                if (client.Count() > 0)
                {
                    rtrn.Add("IsClient","true");
                    rtrn.Add("Store",client.FirstOrDefault().Store);
                }
                else
                {
                    rtrn.Add("IsClient","false");
                    rtrn.Add("Store","none");
                }
            }

            return rtrn;
        }


		//public IDictionary<string, string> GetCheckCashingRecords(string accountNumber)
		//{
		//	IDictionary<string, string> rtrn = new Dictionary<string, string>();

		//	using (var db = new TrackingDb())
		//	{
		//		var records = db.CheckCashings.Where(s => s.AccountNumber == accountNumber);
            
		//		if (client.Count() > 0)
		//		{
		//			rtrn.Add("IsClient","true");
		//			rtrn.Add("Store",client.FirstOrDefault().Store);
		//		}
		//		else
		//		{
		//			rtrn.Add("IsClient","false");
		//			rtrn.Add("Store","none");
		//		}
		//	}

		//	return rtrn;
		//}


    }
}
