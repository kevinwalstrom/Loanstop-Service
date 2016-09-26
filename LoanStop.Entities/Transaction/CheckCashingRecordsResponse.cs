using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
	public class CheckCashingRecordsResponse
	{
		public decimal TotalCashed { get;set;}
		public decimal TotalCashedLast5Days { get;set;}
		public decimal DifferenceCashed { get;set;}

		public List<CheckCashingRecordsEntity> Records { get; set;}
	
	}
}
