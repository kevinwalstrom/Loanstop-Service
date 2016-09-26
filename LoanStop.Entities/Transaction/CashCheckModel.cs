using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Transaction
{
    public class CashCheckModel
    {
        public decimal Amount { get; set; }
        public string CheckType { get; set; }
        public decimal AmountForCustomer { get; set; }
        public string CheckNumber { get; set; }
        public string SsNumber { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string Issuer { get; set; }
    }
}
