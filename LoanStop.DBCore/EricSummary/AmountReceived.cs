using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Entity = LoanStopModel;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.DBCore.EricSummary
{
    public class AmountReceived
    {

        private string connectionString { get; set; }

        public AmountReceived(string connectionString)
        {

            this.connectionString = connectionString;
        }

        public decimal ExecutePayments(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command2 = string.Format(
                    @"SELECT SUM(amount_due) 
                      FROM transactions, payments 
                      WHERE transactions.id=payments.transaction_id
                        AND check_type=0 AND (payment_number is not null) AND Description <> 'Payoff' and Description <> 'Partial'
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command2 = command2.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command2);
    
                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue += query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;

        }

        public decimal ExecutePayoffs(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command2 = string.Format(
                    @"SELECT SUM(amount_paid + balance) 
                      FROM transactions, payments 
                      WHERE transactions.id=payments.transaction_id
                        AND check_type=0 AND (payment_number is not null) AND (Description = 'Payoff' or Description = 'Partial')
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command2 = command2.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command2);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue += query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;

        }

    }
}
