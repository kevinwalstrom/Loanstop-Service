using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data;
using System.Diagnostics;

using MySql.Data.MySqlClient;

using Entity = LoanStopModel;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.DBCore.Repository
{
    public class Colorado : IStateQuries
    {
    
        public string ConnectionString {get; set;}

        public Colorado(string connectionString)
        {
            ConnectionString = connectionString;
        }

    
        public List<ExportEntity> CallExport(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            rtrn.AddRange(CallExpenses(startDate,endDate));
            rtrn.AddRange(CallTransfers(startDate,endDate));
            rtrn.AddRange(CallCustomerAllStatus(startDate,endDate));
            rtrn.AddRange(CallMiscActivities(startDate,endDate));
            rtrn.AddRange(CallCheckGold(startDate,endDate));
            
            return rtrn;

        }

        public List<ExportEntity> ExportLineItem(string lineItem, DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn = new List<ExportEntity>();

            switch (lineItem)
            { 
                case "loaned" :
                    rtrn.AddRange(loaned(startDate,endDate));
                    break;        
                case "paymentstoloans" :
                    rtrn.AddRange(paymentstoLoans(startDate,endDate));
                    rtrn.AddRange(feeincome(startDate,endDate));
                    rtrn.AddRange(principalPayments(startDate,endDate));
                    rtrn.AddRange(bankDeposits(startDate,endDate));
                    rtrn = AdjustACHLastPayment(rtrn);               
                    break;     
                case "closedpayments" : // closed stores payments
                    rtrn.AddRange(closedpayments(startDate,endDate));
                    rtrn.AddRange(feeincome(startDate,endDate));
                    rtrn.AddRange(principalPayments(startDate,endDate));
                    break;     
                case "bounced" :
                    rtrn.AddRange(bounced(startDate,endDate));
                    rtrn.AddRange(feeIncomeBounced(startDate,endDate));
                    rtrn.AddRange(principalBounced(startDate,endDate));
                    break;        
                case "defaulted" :
                    rtrn.AddRange(defaulted(startDate,endDate));
                    break;        
                case "paymentstodefault" :
                    rtrn.AddRange(paymentstodefault(startDate,endDate));
                    break;        
                case "feeincome" :
                    rtrn.AddRange(feeincome(startDate,endDate));
                    break;        
                case "principal" :
                    rtrn.AddRange(principalPayments(startDate,endDate));
                    rtrn.AddRange(principalBounced(startDate,endDate));
                    break;        
                case "moneygram" :
                    rtrn.AddRange(moneygram(startDate,endDate));
                    break;        
                case "checkgold" :
                    rtrn.AddRange(checkgold(startDate,endDate));
                    break;        
                case "transfers" :
                    rtrn.AddRange(transfers(startDate,endDate));
                    break;        
                case "getcash" :
                    rtrn.AddRange(getcash(startDate,endDate));
                    break;        
                case "expenses" :
                    rtrn.AddRange(expenses(startDate,endDate));
                    break;
                case "corpcash":
                    rtrn.AddRange(corpcash(startDate, endDate));
                    break;
                default:
                    rtrn.AddRange(corpcash(startDate,endDate));
                    break;        
            }

            return rtrn;

        }

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> CallExpenses(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format("CAll expenses('{0}','{1}')", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-ddT23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> CallTransfers(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format("CAll transfers('{0}','{1}')", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-ddT23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> CallCustomerAllStatus(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format("CAll customer_all_status('{0}','{1}')", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-ddT23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> CallMiscActivities(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format("CAll misc_activities('{0}','{1}')", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-ddT23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> CallCheckGold(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format("CAll check_gold('{0}','{1}')", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-ddT23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExportEntity> loaned(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'loan' as category, amount_dispursed as amount, 'cash' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
	            FROM transactions 	
	            WHERE 
		            transactions.status != 'Void' 
			            AND 
		            transactions.check_type = 0
			            AND
                    transactions.trans_date between '{0}' AND '{1}'", startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExportEntity> paymentstoLoans(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'ach-payment' as category, payment_plan_checks.amount_recieved as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            (payment_plan_checks.status = 'Cleared') 
			            AND 
                    payment_plan_checks.date_cleared between '{0}' AND '{1}' 
            UNION
                SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            (payment_plan_checks.status = 'Pd Cash') 
			            AND 
                    payment_plan_checks.date_cleared between '{2}' AND '{3}' 
            UNION
	            SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            payment_plan_checks.status = 'Bounced' 
			            AND 
                    payment_plan_checks.date_cleared between '{4}' AND '{5}' 
            UNION
	            SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            payment_plan_checks.status = 'Paid NSF'
			            AND 
                    payment_plan_checks.date_cleared between '{6}' AND '{7}' 
            UNION
	            SELECT 'cash-payment' as category, amount, 'Pd Cash' as export_status, date as export_date, transaction_number as doc_num, payable_to as memo, 'payment' as transaction_type
	            FROM cash_log	
	            WHERE 
                    (transaction_type = 'Revenue' AND description = 'Pd Cash')
                        AND
                    date between '{8}' AND '{9}' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> closedpayments(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'ach-payment' as category, payment_plan_checks.amount_recieved as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            (payment_plan_checks.status = 'Cleared') 
			            AND 
                    payment_plan_checks.date_cleared between '{0}' AND '{1}' 
            UNION
                SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            (payment_plan_checks.status = 'Pd Cash') 
			            AND 
                    payment_plan_checks.date_cleared between '{2}' AND '{3}' 
            UNION
	            SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            payment_plan_checks.status = 'Bounced' 
			            AND 
                    payment_plan_checks.date_cleared between '{4}' AND '{5}' 
            UNION
	            SELECT 'ach-payment' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status as export_status,  payment_plan_checks.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
		            payment_plan_checks.status = 'Paid NSF'
			            AND 
                    payment_plan_checks.date_cleared between '{6}' AND '{7}' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        private List<ExportEntity> bounced(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'bounced' as category, payment_plan_checks.orignal_amount as amount, payment_plan_checks.status  as export_status, payment_plan_checks.date_returned as export_date, transactions.id as doc_num, transactions.name as memo, 'bounced' as transaction_type 
	            FROM transactions join payment_plan_checks on transactions.id = payment_plan_checks.transaction_id	
	            WHERE 
                    payment_plan_checks.date_returned between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> defaulted(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'default' as category, transactions.amount_recieved  as amount, transactions.status  as export_status, transactions.date_returned as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type 
	            FROM transactions 
	            WHERE 
		            (transactions.status = 'Default' || transactions.status = 'Pd Default') 
			            AND 
                    transactions.date_returned between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> paymentstodefault(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'payment-default' as category, payments.amount_paid  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
	            FROM transactions, payments	
                WHERE 
		            transactions.id=payments.transaction_id
			            AND 
		            transactions.check_type = 0
			            AND 
		            payments.payment_number IS NULL
			            AND 
                    payments.date_paid between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> feeincome(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'fee-income' as category, payments.amount_paid  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions, payments	
                WHERE 
		            transactions.id=payments.transaction_id
			            AND 
		            transactions.check_type = 0
			            AND 
		            payments.payment_number IS NOT NULL
			            AND 
		            INSTR(payments.description,'NSF') = 0 
			            AND 
                    payments.date_paid between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> feeIncomeBounced(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'fee-income' as category, payments.amount_paid  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'bounced' as transaction_type
	            FROM transactions, payments	
                WHERE 
		            transactions.id=payments.transaction_id
			            AND 
		            transactions.check_type = 0
			            AND 
		            payments.payment_number IS NOT NULL
			            AND 
		            INSTR(payments.description,'NSF') > 0 
			            AND 
                    payments.date_paid between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExportEntity> principalPayments(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'principal' as category, payments.balance  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions, payments	
                WHERE 
		            transactions.id=payments.transaction_id
			            AND 
		            transactions.check_type = 0
			            AND 
		            payments.payment_number IS NOT NULL
			            AND 
		            INSTR(payments.description,'NSF') = 0 
			            AND 
                    payments.date_paid between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExportEntity> principalBounced(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'principal' as category, payments.balance  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'bounced' as transaction_type
	            FROM transactions, payments	
                WHERE 
		            transactions.id=payments.transaction_id
			            AND 
		            transactions.check_type = 0
			            AND 
		            payments.payment_number IS NOT NULL
			            AND 
		            INSTR(payments.description,'NSF') > 0 
			            AND 
                    payments.date_paid between '{0}' AND '{1}'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> moneygram(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'bill-pay' as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Bill Pay' 
            UNION   
                SELECT 'bill-pay' as category, amount, 'check' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Bill Pay' 
            UNION
	            SELECT 'money-order'  as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{4}' AND '{5}' AND transaction_type = 'Money Order' 
            UNION 
	            SELECT 'money-order'  as category, amount, 'check' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{6}' AND '{7}' AND transaction_type = 'Money Order' 
            UNION
	            SELECT 'send-wire'  as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{8}' AND '{9}' AND transaction_type = 'Send Wire' 
            UNION 
	            SELECT 'send-wire'  as category, amount, 'check' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{10}' AND '{11}' AND transaction_type = 'Send Wire' 

            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> checkgold(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'cash-check' as category, transactions.amount_recieved as amount, 'recieved' as export_status, transactions.date_due as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_due between '{0}' AND '{1}' 
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name !='Gold Gold'
            UNION   
	            SELECT 'cash-check' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status, transactions.date_due as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_due between '{0}' AND '{1}'  
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name !='Gold Gold'
            UNION
	            SELECT 'gold' as category, transactions.amount_recieved as amount, 'recieved' as export_status, transactions.date_due as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_due between '{0}' AND '{1}' 
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name = 'Gold Gold'
            UNION 
	            SELECT 'gold' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status, transactions.date_due as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_due between '{0}' AND '{1}' 
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name = 'Gold Gold'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> transfers(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'transfer' as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'transfer' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Transfer' 
            UNION   
                SELECT 'transfer' as category, amount, 'check' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'transfer' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Transfer' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> getcash(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'get-cash' as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'get-cash' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Get Cash' 
            UNION   
                SELECT 'get-cash' as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'get-cash' as transaction_type
	            FROM cash_log  
	            WHERE date BETWEEN '{2}' AND '{3}' AND transaction_type = 'Misc. Deposit' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> expenses(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT category, amount, 'cash' as export_status, date as export_date, null as doc_num, description as memo, 'expense' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'Expense' 
            UNION   
                SELECT category, amount, 'check' as export_status, date_entered as export_date, check_number as doc_num, description as memo, 'expense' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Expense' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        private List<ExportEntity> corpcash(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn = new List<ExportEntity>();

            string command = string.Format(@"
                SELECT category, amount, 'cash' as export_status, date as export_date, null as doc_num, description as memo, 'corp' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{0}' AND '{1}' AND transaction_type = 'corp' 
            ",
             startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"), startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"), startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")
             );

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            {
                while (reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<ExportEntity> bankDeposits(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'payment' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{0}' AND '{1}' AND transaction_type = 'Deposit' AND description = 'ACH DEPOSIT' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = DateTime.Parse(reader["export_date"].ToString());
                    st.export_status = reader["export_status"].ToString();
                    st.doc_num = reader["doc_num"].ToString();
                    st.memo = reader["memo"].ToString();
                    st.transactiontype = reader["transaction_type"].ToString();
                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retrnList"></param>
        /// <returns></returns>
        public List<ExportEntity> AdjustACHLastPayment(List<ExportEntity> retrnList)
        {

            var achPayments = retrnList.Where(w => w.catagory == "ach-payment");

            foreach (var ach in achPayments)
            {
                try { 
                        var fee = retrnList.Where(w => w.catagory == "fee-income" && w.doc_num == ach.doc_num).FirstOrDefault();       
                        var principal = retrnList.Where(w => w.catagory == "principal" && w.doc_num == ach.doc_num).FirstOrDefault();       
            
                        if( (ach.amount < fee.amount + principal.amount) && ach.export_status == "Paid NSF")
                        {
                            var adjust = retrnList.Where(w => w.doc_num == ach.doc_num && w.catagory == "ach-payment" ).FirstOrDefault(); 
                            adjust.amount = fee.amount + principal.amount;
                        }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            
            }
        

            return retrnList;
        }
    
    }
}
