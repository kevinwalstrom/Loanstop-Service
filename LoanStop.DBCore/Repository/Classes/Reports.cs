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
    public class Reports
    {
    
        private string connectionString {get; set;}

        public Reports(string connectionString)
        {
            this.connectionString = connectionString;
        }
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int NewAccounts(DateTime startDate, DateTime endDate)
        {
            int returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT count(id) 
                      FROM client 
                      WHERE date_opened BETWEEN '{0}' AND '{1}'
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<int>(command);

                returnValue = query.FirstOrDefault();
            }

            return returnValue;
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int NumberOfLoans(DateTime startDate, DateTime endDate)
        {
            int returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT count(id) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<int>(command);

                returnValue = query.FirstOrDefault();
            }

            return returnValue;
        }

        public int NumberOfLoansWy(DateTime startDate, DateTime endDate)
        {
            int returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT count(id) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<int>(command);

                returnValue = query.FirstOrDefault();
            }

            return returnValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
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
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault() != null)
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
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal AmountReceived(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command1 = string.Format(
                    @"SELECT SUM(amount_recieved) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') AND (status!='Open') AND (status!='Closed') AND (status!='Late') AND (status!='Default') AND (status!='Pd Default') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command1 = command1.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command1);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            
                string command2 = string.Format(
                    @"SELECT SUM(amount_paid) 
                      FROM transactions, payments 
                      WHERE transactions.id=payments.transaction_id
                        AND check_type=0 AND (payment_number is not null)
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command2 = command2.Replace("\r\n","");

                query = db.Database.SqlQuery<decimal?>(command2);
            
                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue += query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal AmountReceivedWy(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_recieved) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal AmountBounced(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_recieved) 
                      FROM transactions 
                      WHERE date_returned BETWEEN '{0}' AND '{1}'
                        AND status != 'void' AND (check_type=0)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal AmountPaid(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
           
                string command = string.Format(
                    @"SELECT SUM(amount_paid) 
                      FROM transactions, payments 
                      WHERE transactions.id=payments.transaction_id
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'
                        AND 
                     (transactions.check_type=0) AND (payment_number is NULL)
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal CheckCashReceived(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_recieved) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND
                      status != 'void' AND transactions.check_type>=1
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal CheckCashDisbursed(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_dispursed) 
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND
                      status != 'void' AND transactions.check_type>=1
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal CheckCashBounced(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_recieved) 
                      FROM transactions 
                      WHERE date_returned BETWEEN '{0}' AND '{1}'
                        AND
                      status != 'void' AND transactions.check_type>=1
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal CheckCashPaid(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(amount_paid)
                      FROM transactions, payments 
                      WHERE transactions.id=payments.transaction_id
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'
                        AND 
                     (transactions.check_type>=1) 
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal DebitCardReceived(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(total) 
                      FROM card_transactions                      
                      WHERE datetime
                      BETWEEN '{0}' AND '{1}'
                        AND 
                     (status != 'void') 
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public decimal DebitCardDisbursed(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                    @"SELECT SUM(total_due) 
                      FROM card_transactions                      
                      WHERE datetime
                      BETWEEN '{0}' AND '{1}'
                        AND 
                     (status != 'void') 
                    ", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n","");

                var query = db.Database.SqlQuery<decimal?>(command);

                if(query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal Expenses(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"SELECT SUM(expenses) FROM ( 
                    SELECT SUM(amount) as expenses 
                        FROM cash_log 
                        WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Expense' 
                    UNION 
                    SELECT sum(amount) as expenses 
                        FROM checkbook 
                        WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Expense' 
                    ) AS A 
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal UndepositedCo(DateTime theDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command = string.Format(@"
                SELECT
                    SUM(amount_dispursed)-SUM(issuer) as amount_dispursed 
                FROM transactions 
                WHERE
                ( 
                  (
                    trans_date <= '{0}' 
                        AND 
                    date_cleared > '{1}'
                        AND 
                    (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default'))  
                  ) 
                  OR 
                  ( 
                    trans_date <= '{2}' 
                        AND 
                    date_returned > '{3}' 
                        AND 
                    (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default') ) 
                  ) 
                ) 
                OR 
                (trans_date <= '{4}') AND (status = 'Open' OR status = 'Late' ) 
                ",
                theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal UndepositedCoQuickbooks(DateTime theDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command = string.Format(@"
                SELECT
                    SUM(amount_recieved)-SUM(issuer) as amount_dispursed 
                FROM transactions 
                WHERE
                ( 
                  (
                    trans_date <= '{0}' 
                        AND 
                    date_cleared > '{1}'
                        AND 
                    (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default'))  
                  ) 
                  OR 
                  ( 
                    trans_date <= '{2}' 
                        AND 
                    date_returned > '{3}' 
                        AND 
                    (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default') ) 
                  ) 
                ) 
                OR 
                (trans_date <= '{4}') AND (status = 'Open' OR status = 'Late' ) 
                ",
                theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"), theDate.ToString("yyyy-MM-dd hh:mm:ss"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }


        public decimal UndepositedWy(DateTime theDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command = string.Format(@"
                    SELECT
                        SUM(amount_recieved) as amount_recieved
                    FROM transactions
                    WHERE 
                    ( 
                	    ( 
                		    trans_date <= '{0}'  
                			    AND  
                		    date_cleared > '{1}'
                	    ) 
                    	AND (status != 'Void' OR status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default' ) 
                    )  
                	OR  
                    ( 
                	    trans_date <= '{2}'
                	      AND  
                	    (status = 'Pickup' OR status = 'Deposit' OR status = 'Pickup-NC' OR status = 'Pick Up' OR  status ='Payment Plan') 
                    ) 
                ",
                theDate.ToString("yyyy-MM-dd"), theDate.ToString("yyyy-MM-dd"), theDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public List<CashLogCreditsDebitsEntity> CashLogCredits(DateTime startDate, DateTime endDate, string state)
        {
            List<CashLogCreditsDebitsEntity> rtrn=null;

            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command = null;


                    command = string.Format(
                    @"
                    select transaction_type, SUM(amount) as amount
                        from cash_log 
                        where type = 'credit' AND (ss_number != '222-22-2222' )
                        and `date` between '{0}' AND '{1}'
                        group by transaction_type  
                    ",
                        startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));
                    command = command.Replace("\r\n", "");

                    var query = db.Database.SqlQuery<CashLogCreditsDebitsEntity>(command);

                    rtrn = query.ToList();
            }

            return rtrn;
        }

        public List<CashLogCreditsDebitsEntity> CashLogDebits(DateTime startDate, DateTime endDate)
        {
            List<CashLogCreditsDebitsEntity> rtrn;

            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                SELECT  a.transaction_type as transaction_type,  SUM(a.amount) as amount
                FROM (
                    select transaction_type, SUM(amount) as amount
                        from cash_log 
                        where type = 'debit' AND NOT (transaction_type = 'Transfer' AND category = 'Withdrawl')
                        and `date` between '{0}' AND '{1}'
                        group by transaction_type
                    UNION              
                    select 'Loan' as transaction_type, SUM(amount) as amount
                        from cash_log 
                        where type = 'credit'
                        and (description = 'Payday Loan' AND category = 'VOID') 
                        and `date` between '{0}' AND '{1}'
                        group by transaction_type  
                ) as a
                group by a.transaction_type
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<CashLogCreditsDebitsEntity>(command);

                rtrn = query.ToList();
                    
            }

            return rtrn;
        }


        #region "store summary"
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

            return returnValue.Value;
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
                        from cash_log 
                        where type = 'debit'
                        and `date` between '{0}' AND '{1}'
                        and transaction_type = 'Loan'
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

            return returnValue.Value;
        }

        #endregion

        public List<object> LoansCashLogDetail(DateTime startDate, DateTime endDate)
        {
            List<object> returnValue = new List<object>();

            string command = string.Format(
            @"
            select transaction_number as id, payable_to as name, amount
            from cash_log 
            where type = 'debit'
            and description <> 'Void'
            and `date` between '{0}' AND '{1}'
            and transaction_type = 'loan'    
            order by transaction_number  
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

            command = command.Replace("\r\n", "");
            command = command.Replace("\n", "");

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var obj = new
                    {
                        id = reader["id"].ToString(),
                        name = reader["name"].ToString(),
                        amount = decimal.Parse(reader["amount"].ToString())
                    };

                    returnValue.Add(obj);
                }
                reader.Close();
            }

            return returnValue;

        }

        public List<object> LoansTransactionsDetail(DateTime startDate, DateTime endDate)
        {
            List<object> returnValue = new List<object>();

            string command = string.Format(
            @"
            select id, name, amount_dispursed, status
            from transactions 
            where check_type = 0
            and `trans_date` between '{0}' AND '{1}'
            order by id  
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

            command = command.Replace("\r\n", "");

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var obj = new
                    {
                        id = reader["id"].ToString(),
                        name = reader["name"].ToString(),
                        amount = decimal.Parse(reader["amount_dispursed"].ToString()),
                        status = reader["status"].ToString()
                    };

                    returnValue.Add(obj);
                }
                reader.Close();
            }

            return returnValue;


        }

        //public List<object> PaymentsCashLogDetail(DateTime startDate, DateTime endDate)
        //{
        //    List<object> returnValue = new List<object>();

        //    string command = string.Format(
        //    @"
        //    SELECT transaction_number as id, payable_to as name, amount
        //    FROM cash_log 
        //    WHERE 
        //            (transaction_type = 'Revenue' OR transaction_type = 'Payment') and (ss_number != '222-22-2222')
        //            and 
        //        `date` between '{0}' AND '{1}' 
        //    ORDER BY name
        //    ",
        //    startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

        //    command = command.Replace("\r\n", "");
        //    command = command.Replace("\n", "");

        //    using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
        //    {
        //        while (reader.Read())
        //        {
        //            var obj = new
        //            {
        //                id = reader["id"].ToString(),
        //                name = reader["name"].ToString(),
        //                amount = decimal.Parse(reader["amount"].ToString())
        //            };

        //            returnValue.Add(obj);
        //        }
        //        reader.Close();
        //    }

        //    return returnValue;
        //}




        public decimal CashTransactionsCredits(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"SELECT SUM(expenses) FROM ( 
                    SELECT SUM(amount_paid) 
                        FROM payment_plan_checks 
                        WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Expense' 
                    UNION 
                    SELECT sum(amount) as expenses 
                        FROM checkbook 
                        WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Expense' 
                    ) AS A 
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal CashTransactionsDebits(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"SELECT SUM(expenses) FROM ( 
                    SELECT SUM(amount) as expenses 
                        FROM cash_log 
                        WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Expense' 
                    UNION 
                    SELECT sum(amount) as expenses 
                        FROM checkbook 
                        WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Expense' 
                    ) AS A 
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }


        public decimal StoreCollect(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                    SELECT count(*)   
                    FROM transactions
                    JOIN payment_plan_checks on transactions.id = payment_plan_checks.transaction_id
                    WHERE 
                        payment_plan_checks.date_returned IS NOT NULL and transactions.date_returned IS NULL
                            AND 
                        payment_plan_checks.date_returned >= '{0}' AND payment_plan_checks.date_returned < '{1}'
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal Outsourced(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                    SELECT count(*)   
                    FROM transactions
                    JOIN payment_plan_checks on transactions.id = payment_plan_checks.transaction_id
                    WHERE 
                        payment_plan_checks.date_returned IS NOT NULL and transactions.date_returned IS NOT NULL
                            AND 
                        payment_plan_checks.date_returned >= '{0}' AND payment_plan_checks.date_returned < '{1}'
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public List<AgingEntity> Aging()
        {
            List<AgingEntity> returnValue = new List<AgingEntity>();

            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                    SELECT 'Current', count(*), aging from 
                    (  
                      select case    
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 0 and 30 then '0-30'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 31 and 60   then '31-60'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 61 and 90  then '61-90'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 91 and 120  then '91-120'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 121 and 150  then '121-150'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 151 and 180  then '151-180'
                        else 'greater than 180'
                        end as aging
                      from (
	                    select * FROM transactions where status IN ('Deposit', 'Open', 'Late')
                      ) as subset
                    )  t  
                    group by aging

                    UNION

                    SELECT 'Default', count(*), aging from 
                    (  
                      select case    
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 0 and 180 then 'default current'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 181 and 210   then '30 days past'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 211 and 240 then '60 days past'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 241 and 360 then '3 - 6 months past'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 361 and 540 then '6 - 12 months past'  
                        when DATEDIFF(CURRENT_DATE(), trans_date)  between 541 and 900 then '1 - 2 years past'  
                        else 'greater than 2 years past'
                        end as aging
                      from (
	                    select * FROM transactions where status IN ('Default')
                      ) as subset
                    )  t  
                    group by aging
                ");

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    while (reader.Read())
                    {
                        var obj = new AgingEntity()
                        {
                            TransactionState = reader[0].ToString(),
                            NumberOfTransactions = int.Parse( reader[1].ToString()),
                            AgingRange = reader[2].ToString(),
                        };

                        returnValue.Add(obj);
                    }
                    reader.Close();
                }

            }

            return returnValue;
        }

        public decimal AverageLoanAmountColorado(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                    SELECT SUM(amount_dispursed)/Count(*) from transactions WHERE status <> 'Void' AND trans_date >= '{0}' and trans_date < '{1}'
                ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

        public decimal AverageLoanAmountWyoming(DateTime startDate, DateTime endDate)
        {
            decimal? returnValue = 0;
            using (var db = new Entity.LoanStopEntities(connectionString))
            {
                string command = string.Format(
                @"
                    SELECT SUM(amount_dispursed)/Count(*) from transactions WHERE status <> 'Void' AND trans_date >= '{0}' and trans_date < '{1}'
                ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                command = command.Replace("\r\n", "");
                command = command.Replace("\n", "");

                var query = db.Database.SqlQuery<decimal?>(command);

                if (query.FirstOrDefault<decimal?>() != null)
                    returnValue = query.FirstOrDefault<decimal?>();
            }

            return returnValue.Value;
        }

    }
}
