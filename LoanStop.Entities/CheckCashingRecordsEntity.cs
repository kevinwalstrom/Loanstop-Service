using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities
{
	public class CheckCashingRecordsEntity
	{
		public string Issuer { get; set;}
		public string IssuerName { get; set;}
		public DateTime TransDate { get; set;}	
		public decimal Amount { get; set;}
		public string Status { get; set;}	
	}
}
