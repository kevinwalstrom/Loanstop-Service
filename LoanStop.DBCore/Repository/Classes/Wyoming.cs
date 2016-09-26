using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data;
using MySql.Data.MySqlClient;

using Entity = LoanStopModel;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.DBCore.Repository
{
    public class Wyoming : IStateQuries
    {
        public string ConnectionString {get; set;}

        public Wyoming(string connectionString)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineItem"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ExportEntity> ExportLineItem(string lineItem, DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn = new List<ExportEntity>();

            switch (lineItem)
            { 
                case "loaned" :
                    rtrn.AddRange(loaned(startDate,endDate));
                    break;        
                case "paymentstoloans" :
                    //rtrn.AddRange(amountReceived(startDate,endDate));
                    //rtrn.AddRange(amountDisbursed(startDate,endDate));
                    //rtrn.AddRange(feeincome(startDate,endDate));
                    //rtrn.AddRange(principalPayments(startDate,endDate));
                    rtrn.AddRange(bankDeposits(startDate,endDate));
                    break;        
                case "checkDeposits" :
                    rtrn.AddRange(amountCheckReceived(startDate,endDate));
                    rtrn.AddRange(amountCheckDisbursed(startDate,endDate));
                    rtrn.AddRange(bankCheckDeposits(startDate,endDate));
                    rtrn.AddRange(checkgold(startDate,endDate));
                    rtrn.AddRange(bankAchDeposits(startDate,endDate));
                    break;        
                case "achDeposits" :
                    rtrn.AddRange(amountAchReceived(startDate,endDate));
                    //rtrn.AddRange(amountAchDisbursed(startDate,endDate));
                    break;        
                case "cashDeposits" :
                    rtrn.AddRange(amountCashReceived(startDate,endDate));
                    rtrn.AddRange(amountCashDisbursed(startDate,endDate));
                    rtrn.AddRange(amountCashPaid(startDate,endDate));
                    rtrn.AddRange(bankCashDeposits(startDate,endDate));
                    break;        
                case "cashpaidpp" :
                    rtrn.AddRange(amountCashPaidPaymentPlan(startDate,endDate));
                    rtrn.AddRange(amountCashIncomePaymentPlan(startDate,endDate));
                    break;        
                case "wyomingfeeincome" :
                    rtrn.AddRange(amountCheckReceived(startDate,endDate));
                    rtrn.AddRange(amountCheckDisbursed(startDate,endDate));
                    rtrn.AddRange(bankCheckDeposits(startDate,endDate));
                    rtrn.AddRange(amountCashReceived(startDate,endDate));
                    rtrn.AddRange(amountCashDisbursed(startDate,endDate));
                    rtrn.AddRange(amountCashPaid(startDate,endDate));
                    rtrn.AddRange(amountCashPaidPaymentPlan(startDate,endDate));
                    rtrn.AddRange(amountCashIncomePaymentPlan(startDate,endDate));
                    rtrn.AddRange(bankCashDeposits(startDate,endDate));
                    break;        
                case "bouncedwy" :
                    rtrn.AddRange(bounced(startDate,endDate));
                    rtrn.AddRange(bouncedDisbursed(startDate,endDate));
                    break;        
                case "defaultwy" :
                    rtrn.AddRange(defaulted(startDate,endDate));
                    break;        
                case "defaulted" :
                    rtrn.AddRange(defaulted(startDate,endDate));
                    break;        
                case "paymentstodefault" :
                    rtrn.AddRange(paymentstodefault(startDate,endDate));
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
                    rtrn.AddRange(loaned(startDate,endDate));
                    break;        
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
        private List<ExportEntity> loaned(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                    SELECT 'loan' as category, amount_dispursed as amount, 'disbursed' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
	                FROM transactions 	
	                WHERE 
		                transactions.status != 'Void' 
			                AND 
		                transactions.check_type = 0
			                AND
                        transactions.trans_date between '{0}' AND '{1}'
                UNION
                    SELECT 'loan' as category, amount_recieved as amount, 'received' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
	                FROM transactions 	
	                WHERE 
		                transactions.status != 'Void' 
			                AND 
		                transactions.check_type = 0
			                AND
                        transactions.trans_date between '{2}' AND '{3}'
                ", 
                startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"), 	
                startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")); 	

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
        private List<ExportEntity> amountReceived(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'ach-payment' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND transactions.status ='Cleared'
			            AND
                    transactions.date_cleared between '{0}' AND '{1}' 
            UNION
	            SELECT 'cash-payment' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status = 'Pd Cash'
			            AND
                    transactions.date_cleared between '{2}' AND '{3}' 
            UNION
	            SELECT 'ach-payment' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 1
			            AND
		            transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND transactions.status ='Cleared'
			            AND
                    transactions.date_cleared between '{4}' AND '{5}' 
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	
//	            SELECT 'cash-payment' as category, amount, 'Pd Cash' as export_status, date as export_date, transaction_number as doc_num, payable_to as memo, 'loan' as transaction_type
//	            FROM cash_log	
//	            WHERE 
//                    (transaction_type = 'Revenue' AND description = 'Pd Cash')
//                        AND
//                    date between '{2}' AND '{3}' 

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
        private List<ExportEntity> amountCheckReceived(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'check-payment' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
                    transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND (transactions.status ='Cleared' || transactions.status ='Paid' || transactions.status ='Default' || transactions.status ='Bounced')
			            AND
                    transactions.date_cleared between '{0}' AND '{1}' 
			UNION
				SELECT 'check-payment' as category, payment_plan_checks.amount_recieved as amount, 'received' as export_status,  payment_plan_checks.date_cleared as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'payment' as transaction_type
	            FROM payment_plan_checks 	
	            WHERE 
                    payment_plan_checks.status != 'Pd Cash' AND payment_plan_checks.status != 'Void' AND (payment_plan_checks.status ='Cleared' || payment_plan_checks.status ='Paid' || payment_plan_checks.status ='Default' || payment_plan_checks.status ='Bounced')
			            AND
                    payment_plan_checks.date_cleared between '{2}' AND '{3}' 
			", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
			 startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            //UNION
            //    SELECT 'check-payment' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
            //    FROM transactions 	
            //    WHERE 
            //        transactions.check_type = 1
            //            AND
            //        transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND transactions.status ='Cleared'
            //            AND
            //        transactions.date_cleared between '{2}' AND '{3}' 


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
        private List<ExportEntity> amountAchReceived(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'ach-payment' as category, payment_plan_checks.amount_recieved as amount, 'received' as export_status,  payment_plan_checks.date_cleared as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'payment' as transaction_type
	            FROM payment_plan_checks 	
	            WHERE 
		            payment_plan_checks.status != 'Pd Cash' AND payment_plan_checks.status != 'Void' 
			            AND
                    payment_plan_checks.date_cleared between '{0}' AND '{1}' 
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
        private List<ExportEntity> amountCashReceived(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'received' as category, transactions.amount_recieved as amount, 'received' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status = 'Pd Cash'
			            AND
                    transactions.date_cleared between '{0}' AND '{1}' 
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
        private List<ExportEntity> amountCashPaid(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'cash-payment' as category, IF(payments.amount_paid IS NULL, transactions.amount_recieved, transactions.amount_recieved + payments.amount_paid ) as amount, 'paid' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions 
                    LEFT JOIN payments ON (transactions.id = payments.transaction_id	AND transactions.date_cleared = payments.date_paid)
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status = 'Pd Cash'
			            AND
                    transactions.date_cleared between '{0}' AND '{1}' 
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
        private List<ExportEntity> amountCashPaidPaymentPlan(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'cash-payment-pp' as category, payment_plan_checks.amount_recieved as amount, 'received' as export_status,  payment_plan_checks.date_paid as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'payment' as transaction_type
	            FROM payment_plan_checks 	
	            WHERE 
		            payment_plan_checks.status = 'Pd Cash' 
			            AND
		            payment_plan_checks.date_paid between '{0}' AND '{1}' 
            UNION
	            SELECT 'cash-payment-pp' as category, (transactions.amount_dispursed / 4) as amount, 'disbursed' as export_status,  payment_plan_checks.date_paid as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'payment' as transaction_type
	            FROM payment_plan_checks
		            left join transactions ON payment_plan_checks.transaction_id = transactions.id
	            WHERE 
		            payment_plan_checks.status = 'Pd Cash' 
			            AND
		            payment_plan_checks.date_paid between '{2}' AND '{3}'             
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

        private List<ExportEntity> amountCashIncomePaymentPlan(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'cash-pp-income' as category, payment_plan_checks.orignal_amount as amount, 'Pd Cash' as export_status,  payment_plan_checks.date_paid as export_date, payments.transaction_id as doc_num, payment_plan_checks.name as memo, 'null' as transaction_type
	            FROM payment_plan_checks join payments on payment_plan_checks.transaction_id = payments.transaction_id	
	            WHERE 
		            (payment_plan_checks.status = 'Pd Cash') 
			            AND 
                    payment_plan_checks.date_paid between '{0}' AND '{1}' 
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
        private List<ExportEntity> amountCheckDisbursed(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'check-payment' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
                    transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND (transactions.status ='Cleared' || transactions.status ='Paid' || transactions.status ='Default' || transactions.status ='Bounced')
			            AND
                    transactions.date_cleared between '{0}' AND '{1}' 
	        UNION   
				SELECT 'check-payment' as category, (transactions.amount_dispursed / 4) as amount, 'disbursed' as export_status,  payment_plan_checks.date_cleared as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'payment' as transaction_type
				FROM payment_plan_checks
					left join transactions ON payment_plan_checks.transaction_id = transactions.id
				WHERE 
					payment_plan_checks.status != 'Pd Cash' AND payment_plan_checks.status != 'Void' AND (payment_plan_checks.status ='Cleared' || payment_plan_checks.status ='Paid' || payment_plan_checks.status ='Default' || payment_plan_checks.status ='Bounced')
						AND
					payment_plan_checks.date_cleared between '{2}' AND '{3}'  
			", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            //UNION
            //    SELECT 'disbursed' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
            //    FROM transactions 	
            //    WHERE 
            //        transactions.check_type = 1
            //            AND
            //        transactions.status != 'Pd Cash' AND transactions.status != 'Void' AND transactions.status ='Cleared'
            //            AND
            //        transactions.date_cleared between '{2}' AND '{3}' 

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
        private List<ExportEntity> amountAchDisbursed(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'disbursed' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status = 'Pd Cash'
			            AND
                    transactions.date_cleared between '{2}' AND '{3}' 
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
        private List<ExportEntity> amountCashDisbursed(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
	            SELECT 'disbursed' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status,  transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'payment' as transaction_type
	            FROM transactions 	
	            WHERE 
		            transactions.check_type = 0
			            AND
		            transactions.status = 'Pd Cash'
			            AND
                    transactions.date_cleared between '{2}' AND '{3}' 
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

        
        private List<ExportEntity> bounced(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
            SELECT * FROM (
				SELECT 'bounced' as category, transactions.amount_recieved as amount, 'received'  as export_status, transactions.date_returned as export_date, transactions.id as doc_num, transactions.name as memo, 'bounced' as transaction_type 
				FROM transactions 
					LEFT JOIN payment_plan_checks ON payment_plan_checks.transaction_id = transactions.id AND payment_plan_checks.date_returned = transactions.date_returned
				WHERE 
					transactions.date_returned between '{0}' and '{1}'
						AND 
					payment_plan_checks.date_returned IS NULL
			UNION
				SELECT 'bounced-pp' as category, payment_plan_checks.amount_recieved as amount, 'received'  as export_status, transactions.date_returned as export_date, payment_plan_checks.id as doc_num, transactions.name as memo, 'bounced' as transaction_type 
				FROM transactions 
					RIGHT JOIN payment_plan_checks ON payment_plan_checks.transaction_id = transactions.id AND payment_plan_checks.date_returned = transactions.date_returned
				WHERE 
					transactions.date_returned between '{2}' and '{3}'
			) as A ORDER BY export_date", 
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
        private List<ExportEntity> bouncedDisbursed(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
            SELECT * FROM (
				SELECT 'bounced' as category, transactions.amount_dispursed as amount, 'disbursed'  as export_status, transactions.date_returned as export_date, transactions.id as doc_num, transactions.name as memo, 'bounced' as transaction_type 
				FROM transactions 
					LEFT JOIN payment_plan_checks ON payment_plan_checks.transaction_id = transactions.id AND payment_plan_checks.date_returned = transactions.date_returned
				WHERE 
					transactions.date_returned between '{0}' and '{1}'
						AND 
					payment_plan_checks.date_returned IS NULL
			UNION
				SELECT 'bounced-pp' as category, (transactions.amount_dispursed / 4) as amount, 'disbursed' as export_status, payment_plan_checks.date_returned as export_date, payment_plan_checks.id as doc_num, payment_plan_checks.name as memo, 'bounced' as transaction_type 
				FROM payment_plan_checks
					left join transactions ON payment_plan_checks.transaction_id = transactions.id
				WHERE 
					payment_plan_checks.status != 'Void' 
						AND
					payment_plan_checks.date_returned between '{2}' AND '{3}'  
            ) as A ORDER BY export_date
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
        private List<ExportEntity> defaulted(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
			SELECT 'default' as category, payment_plan_checks.amount_recieved as amount, 'received'  as export_status, transactions.date_returned as export_date, transactions.id as doc_num, transactions.name as memo, 'default' as transaction_type 
			FROM transactions 
				RIGHT JOIN payment_plan_checks ON payment_plan_checks.transaction_id = transactions.id 
			WHERE 
	
				transactions.date_returned between '{0}' AND '{1}'
				and
				payment_plan_checks.status = 'default'
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
                SELECT 'fee-income' as category, payments.amount_paid  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
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
                SELECT 'fee-income' as category, payments.amount_paid  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
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
        private List<ExportEntity> principalPayments(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'principal' as category, payments.balance  as amount, transactions.status  as export_status, payments.date_paid as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type
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
            UNION
	            SELECT 'debit-card'  as category, amount, 'cash' as export_status, date as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM cash_log 
	            WHERE date BETWEEN '{12}' AND '{13}' AND transaction_type = 'Debit Card Load' 
            UNION 
	            SELECT 'debit-card'  as category, amount, 'check' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'money-gram' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{14}' AND '{15}' AND transaction_type = 'Debit Card Load' 

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
	            SELECT 'cash-check' as category, transactions.amount_recieved as amount, 'recieved' as export_status, transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_cleared between '{0}' AND '{1}' 
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name !='Gold Gold'
            UNION   
	            SELECT 'cash-check' as category, transactions.amount_dispursed as amount, 'disbursed' as export_status, transactions.date_cleared as export_date, transactions.id as doc_num, transactions.name as memo, 'misc' as transaction_type 
	            FROM transactions 
	            WHERE 
		            transactions.check_type = 1 
			            AND 
                    transactions.date_cleared between '{2}' AND '{3}'  
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
                    transactions.date_due between '{4}' AND '{5}' 
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
                    transactions.date_due between '{6}' AND '{7}' 
                        AND
                    transactions.status != 'Void' 
                        AND
		            transactions.name = 'Gold Gold'
            ", 
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00"),
             startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")
             ); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, command))
            { 
                while(reader.Read())
                {
                    ExportEntity st = new ExportEntity();
                    st.catagory = reader["category"].ToString();
                    st.amount = decimal.Parse(reader["amount"].ToString());
                    st.export_date = reader["export_date"].ToString() != string.Empty ? DateTime.Parse(reader["export_date"].ToString()) : new DateTime();
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
                SELECT 'deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'loan' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{0}' AND '{1}' AND transaction_type = 'Deposit' AND description = 'CHECK DEPOSIT' 
            UNION
                SELECT 'deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'loan' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Deposit' AND description = 'ACH DEPOSIT' 
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
        private List<ExportEntity> bankCheckDeposits(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'check-deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'payment' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{0}' AND '{1}' AND transaction_type = 'Deposit' AND description = 'CHECK DEPOSIT' 
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
        private List<ExportEntity> bankAchDeposits(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'ach-deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'payment' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Deposit' AND description = 'ACH DEPOSIT' 
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
        private List<ExportEntity> bankCashDeposits(DateTime startDate, DateTime endDate)
        {
            List<ExportEntity> rtrn= new List<ExportEntity>();

            string command = string.Format(@"
                SELECT 'deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'loan' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{0}' AND '{1}' AND transaction_type = 'Deposit' AND description = 'CHECK DEPOSIT' 
            UNION
                SELECT 'deposit' as category, amount, 'deposit' as export_status, date_entered as export_date, transaction_number as doc_num, description as memo, 'loan' as transaction_type
	            FROM checkbook 
	            WHERE date_entered BETWEEN '{2}' AND '{3}' AND transaction_type = 'Deposit' AND description = 'ACH DEPOSIT' 
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
        /// <param name="retrnList"></param>
        /// <returns></returns>
        public List<ExportEntity> AdjustACHLastPayment(List<ExportEntity> retrnList)
        {

            var achPayments = retrnList.Where(w => w.catagory == "ach-payment");

            foreach (var ach in achPayments)
            {
                var fee = retrnList.Where(w => w.catagory == "fee-income" && w.doc_num == ach.doc_num).FirstOrDefault();       
                var principal = retrnList.Where(w => w.catagory == "principal" && w.doc_num == ach.doc_num).FirstOrDefault();       
            
                if( (ach.amount < fee.amount + principal.amount) && ach.export_status == "Paid NSF")
                {
                    var adjust = retrnList.Where(w => w.doc_num == ach.doc_num && w.catagory == "ach-payment" ).FirstOrDefault(); 
                    adjust.amount = fee.amount + principal.amount;
                }
            
            }
        
        
            return retrnList;
        }

    
    
    }
}
