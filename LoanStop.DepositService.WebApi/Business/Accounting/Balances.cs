using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Business.Accounting
{
    public class Balances
    {
        private string connectionString;
            
        public Balances(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public decimal CashBalance(DateTime date)
        {
            var rep = new Repository.CashLog(connectionString);
            
            return rep.Balance(date);
        }
    
        public decimal BankBalance(DateTime date)
        {

            var rep = new Repository.Checkbook(connectionString);
            
            return rep.Balance(date);
        }
    
    }
}