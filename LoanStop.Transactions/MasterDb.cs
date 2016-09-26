using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanStop.Entities.Transaction;
using LoanStop.DBCore.Repository;
using LoanStop.Entities;

namespace LoanStop.Transactions
{
	public class MasterDb
	{
	
		public MasterDb()
		{ }


		public CheckCashingRecordsResponse CheckGetCheckCashingRecords(string accountNumber)
		{
			var rtrn = new CheckCashingRecordsResponse();

			var exe = new Tracking();

			try { 
				rtrn.Records = exe.ClientCheckCashing(accountNumber);

				rtrn.TotalCashed = rtrn.Records.Select(t => t.Amount).Sum();
				rtrn.TotalCashedLast5Days = rtrn.Records.Where(s => s.TransDate > DateTime.Now.Date.AddDays(-5)).Select(t => t.Amount).Sum();
				rtrn.DifferenceCashed =rtrn.TotalCashed - rtrn.TotalCashedLast5Days;
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return rtrn;
		}
	
	
	
	}
}
