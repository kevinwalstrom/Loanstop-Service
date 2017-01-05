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
    public class DailyBalanceStoreSummary
    {

        private string connectionString { get; set; }

        public DailyBalanceStoreSummary(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public decimal CashLogLoans(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = null;

            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                SELECT SUM(a.amount) as amount
                FROM (
                    select transaction_type, SUM(amount) as amount
                        from cash_log 
                        where type = 'debit'
                        and `date` between '{0}' AND '{1}'
                        and transaction_type = 'Loan'
                        group by transaction_type
                    UNION              
                    select 'Loan' as transaction_type, SUM(amount) as amount
                        from cash_log 
                        where type = 'credit'
                        and (description = 'Payday Loan' AND category = 'VOID') 
                        and `date` between '{2}' AND '{3}'
                        group by transaction_type  
                ) as a
                group by a.transaction_type
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.HasValue ? returnValue.Value : 0m;
        }

        public decimal CashLogCashedChecks(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = null;

            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                SELECT SUM(a.amount) as amount
                FROM (
                    select transaction_type, SUM(amount) as amount
                    FROM cash_log
                    WHERE 
                        `date` between '{0}' AND '{1}'
                        and category = 'Cash Check'
                        group by transaction_type
                ) as a
                group by a.transaction_type
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.HasValue ? returnValue.Value : 0m;
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

            return returnValue.HasValue ? returnValue.Value : 0m;
        }


        public decimal CashLogPayments(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;

            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                SELECT SUM(amount) as amount
                FROM cash_log 
                WHERE 
                     (transaction_type = 'Revenue' OR transaction_type = 'Payment') and (ss_number != '222-22-2222')
                        and 
                    `date` between '{0}' AND '{1}'
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.HasValue ? returnValue.Value : 0m;
        }

        public decimal TransactionPayments(DateTime startDate, DateTime endDate, string state)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command = null;

                if (state.ToLower() == "colorado")
                {
                    command = string.Format(
                        @"SELECT sum(a.amount) as amount from (
                        (SELECT transaction_id as id, (amount_paid + balance) as amount
                        FROM payments 
                        WHERE date_paid BETWEEN '{0}' AND '{1}'
                            AND ((description <> 'Deposit Payment')	AND description <> 'DEPOSIT SERVICE' AND description <> 'NSF')
                            AND (ss_number <> '222-22-2222'))
                        union all
	                        (SELECT id, amount_recieved as amount
                            FROM transactions 
                                where date_cleared BETWEEN '{2}' AND '{3}'
		                        AND status = 'Pd Cash')
                    ) as a
                    ",
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));
                }
                else
                {
                    command = string.Format(
                        @"SELECT sum(a.amount) as amount from (
                            (SELECT transaction_id as id, (amount_paid) as amount
                            FROM payments 
                            WHERE date_paid BETWEEN '{0}' AND '{1}'
                                AND ((description <> 'Deposit Payment')	AND description <> 'DEPOSIT SERVICE' AND description <> 'NSF')
                                AND (ss_number <> '222-22-2222')
                                AND amount_paid > 0)
                            UNION all
                            (SELECT id, amount_recieved as amount 
                            FROM transactions 
                                where date_cleared BETWEEN '{2}' AND '{3}'
		                        AND status = 'Pd Cash')
                            union 
	                        (SELECT id, amount_recieved as amount 
                            FROM payment_plan_checks 
                                where date_paid BETWEEN '{4}' AND '{5}'
		                        AND status = 'Pd Cash')
                    ) as a
                    ",
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                }

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.HasValue ? returnValue.Value : 0m;
        }




    }
}
