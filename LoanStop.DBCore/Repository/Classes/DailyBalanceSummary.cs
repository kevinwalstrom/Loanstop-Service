using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity = LoanStopModel;
using LoanStop.Entities.Reports;
using MySql.Data.MySqlClient;

namespace LoanStop.DBCore.Repository
{
    public class DailyBalanceSummary
    {

        private string connectionString { get; set; }

        public DailyBalanceSummary(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public decimal AmountDisbursed(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_dispursed) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status <> 'Void') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal AmountDisbursedWy(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_dispursed) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal CashedChecks(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_dispursed) 
                      FROM transactions 
                      WHERE trans_date >= '{0}' AND trans_date < '{1}'
                        AND (status <> 'Void') AND (check_type=1)
                    ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal Payments(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT sum(a.amount) as amount from (
                        SELECT (SUM(amount_paid) + SUM(balance)) as amount
                        FROM payments 
                        WHERE date_paid BETWEEN '{0}' AND '{1}'
                            AND ((description <> 'Deposit Payment')	AND description <> 'DEPOSIT SERVICE' AND description <> 'NSF')
                            AND (ss_number <> '222-22-2222')
                        union 
	                        SELECT amount_recieved as amount 
                            FROM transactions 
                                where date_cleared BETWEEN '{2}' AND '{3}'
		                        AND status = 'Pd Cash'
                    ) as a
                    ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }


    }
}
