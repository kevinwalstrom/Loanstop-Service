using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data;
using MySql.Data.MySqlClient;

using Microsoft.VisualBasic;

using Entity = LoanStopModel;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.DBCore.Repository
{
    public class WyomingAudit
    {
        private string connectionString {get; set;}

        public WyomingAudit(string connectionString)
        {
            this.connectionString = connectionString;
        }
    
        /// 
        /// 
        /// 
        public List<object> PostDatedChecks(DateTime startDate, DateTime endDate)
        {
            List<object> rtrn= new List<object>();

            string command = string.Format(@"
                SELECT 'loan' as category, amount_recieved as amount, 'received' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
	            FROM transactions 	
	            WHERE 
		            transactions.status != 'Void' 
			            AND 
		            transactions.check_type = 0
			            AND
                    transactions.trans_date between '{0}' AND '{1}'
                ", 
                startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    var st = new{
                        catagory = reader["category"].ToString(),
                        amount = decimal.Parse(reader["amount"].ToString()),
                        export_date = DateTime.Parse(reader["export_date"].ToString()),
                        export_status = reader["export_status"].ToString(),
                        doc_num = reader["doc_num"].ToString(),
                        memo = reader["memo"].ToString(),
                        transactiontype = reader["transaction_type"].ToString()
                    };

                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }


        /// 
        /// 
        /// 
        public List<object> Borrowers(DateTime startDate, DateTime endDate)
        {
            List<object> rtrn= new List<object>();

            string command = string.Format(@"
                SELECT 'count', B.A,COUNT(B.A) from 
                (
                    SELECT ss_number as ss, count(ss_number) AS A
                    FROM transactions 
                    WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status!='Void') 
                        AND (check_type=0)
                    GROUP BY ss_number
                ) As B
                GROUP BY B.A                
                ", 
                startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    var st = new {
                        catagory = reader["category"].ToString(),
                        amount = decimal.Parse(reader["amount"].ToString()),
                        export_date = DateTime.Parse(reader["export_date"].ToString()),
                        export_status = reader["export_status"].ToString(),
                        doc_num = reader["doc_num"].ToString(),
                        memo = reader["memo"].ToString(),
                        transactiontype = reader["transaction_type"].ToString()
                    };

                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// 
        /// 
        /// 
        public List<object> Rescinded(DateTime startDate, DateTime endDate)
        {
            List<object> rtrn= new List<object>();

            string command = string.Format(@"
                SELECT 'rescinded' as category, amount_recieved as amount, 'received' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
                FROM transactions
                WHERE 
	                date_cleared BETWEEN trans_date AND DATE_ADD(trans_date, INTERVAL 2 DAY)
                    AND check_type = 0
                    AND status != 'Void'
                    AND trans_date BETWEEN '{0}' AND '{1}'                
            ", 
            startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    var st = new {
                        catagory = reader["category"].ToString(),
                        amount = decimal.Parse(reader["amount"].ToString()),
                        export_date = DateTime.Parse(reader["export_date"].ToString()),
                        export_status = reader["export_status"].ToString(),
                        doc_num = reader["doc_num"].ToString(),
                        memo = reader["memo"].ToString(),
                        transactiontype = reader["transaction_type"].ToString()
                    };

                    rtrn.Add(st);
                }
                reader.Close();
            }
            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        public object Totals(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT count(id) as num, sum(amount_dispursed) as amount 
                FROM transactions 
                where trans_date>='{0}' and trans_date<='{1}'
                and status!='Void'
                and check_type=0
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    rtrn = new {
                        count = int.Parse(reader["num"].ToString()),
                        sum = decimal.Parse(reader["amount"].ToString())
                    };
                }
            }

            return rtrn;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<object> ByAmount(DateTime startDate, DateTime endDate)
        {
        
            List<object> rtrn = new List<object>();
            
            string command = string.Format(@"
                SELECT '100 and less' as subset, count(id) as num 
                FROM transactions 
                where trans_date>='{0}' and trans_date<='{1}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=100
                union
                SELECT '101 to 200' as subset, count(id) as num
                FROM transactions 
                where trans_date>='{2}' and trans_date<='{3}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=200
                and amount_dispursed>100
                union
                SELECT '201 to 300' as subset, count(id) as num  
                FROM transactions 
                where trans_date>='{4}' and trans_date<='{5}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=300
                and amount_dispursed>200
                union
                SELECT '301 to 400' as subset, count(id) as num 
                FROM transactions 
                where trans_date>='{6}' and trans_date<='{7}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=400
                and amount_dispursed>300
                union
                SELECT '401 to 500' as subset, count(id) as num 
                FROM transactions 
                where trans_date>='{8}' and trans_date<='{9}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=500
                and amount_dispursed>400
                union
                SELECT '501 to 1000' as subset, count(id) as num 
                FROM transactions 
                where trans_date>='{10}' and trans_date<='{11}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=1000
                and amount_dispursed>500
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),	
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"), 	
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),	
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),	
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),	
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    var add = new {
                        range = reader["subset"].ToString(),
                        count = int.Parse(reader["num"].ToString())
                    };

                    rtrn.Add(add);
                }
            }

            return rtrn;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public List<object> BySSNumber(DateTime startDate, DateTime endDate)
        {
        
            List<object> rtrn = new List<object>();
            
            string command = string.Format(@"
                SELECT B.numOfssNumbers as numOfSsNumbers, COUNT(B.numOfSsNumbers) as num from 
                (
                    select ss_number as ss, count(ss_number) AS numOfSsNumbers
                    FROM transactions 
                    WHERE trans_date>='{0}' and trans_date<='{1}' 
                    AND (status!='void') 
                    AND (check_type=0)
                    GROUP BY ss_number
                ) As B
                GROUP BY B.numOfSsNumbers
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    var add = new {
                        numOfSsNumbers = int.Parse(reader["numOfSsNumbers"].ToString()),
                        count = int.Parse(reader["num"].ToString())
                    };

                    rtrn.Add(add);
                }
            }

            return rtrn;        
        
        }

        /// <summary>
        /// 
        /// </summary>
        public object PostDatedCheckLoanSummary(DateTime startDate, DateTime endDate)
        {
        
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT 
                    count(id) as num, 
                    sum(amount_recieved) as 'totalReceived', 
                    sum(amount_dispursed) as 'totalDispursed', 
                    (sum(amount_recieved)/count(id)) as 'avgRec', 
                    (sum(amount_dispursed)/count(id)) as 'avgLoan', 
                    (sum(datediff(date_due, trans_date))/count(id)) as 'avgLoanPeriod'
                FROM transactions 
                WHERE (trans_date BETWEEN '{0}' AND '{1}') 
                    AND (status!='void') 
                    AND (check_type=0)
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    rtrn = new {
                        num = int.Parse(reader["num"].ToString()),
                        totalReceived = decimal.Parse(reader["totalReceived"].ToString()),
                        totalDispursed = decimal.Parse(reader["totalDispursed"].ToString()),
                        avgRec = decimal.Parse(reader["avgRec"].ToString()),
                        avgLoan = decimal.Parse(reader["avgLoan"].ToString()),
                        avgLoanPeriod = decimal.Parse(reader["avgLoanPeriod"].ToString())
                    };
                }
            }

            return rtrn;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public object ChargeOffs(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT count(id) as num, sum(amount_recieved) as amount
                FROM transactions 
                where date_returned>='{0}' and date_returned<='{1}'
                and status!='Void'
                and check_type=0
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    rtrn = new {
                        num = int.Parse(reader["num"].ToString()),
                        amountRecieved = decimal.Parse(reader["amount"].ToString())
                    };
                }
            }

            return rtrn;
        
        }
        
        /// <summary>
        /// 
        /// </summary>
        public object AmountRecovered(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT COUNT(A.id) as num, SUM(A.amount) as amount FROM (                
                    SELECT transaction_id as id,  sum(amount_paid) as amount
                    FROM payments
                    where date_paid>='{0}' and date_paid<='{1}'
                    and description!='Discount'
                    and payment_number is null
                    GROUP BY transaction_id
                ) AS A 
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    rtrn = new {
                        count = int.Parse(reader["num"].ToString()),
                        amountPaid = decimal.Parse(reader["amount"].ToString())
                    };
                }
            }

            return rtrn;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public object Defaulted(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT count(*) as num, sum(amount_dispursed) as amount
                FROM transactions
                where date_returned>='{0}' and date_returned<='{1}'
                    and status!='Void'
                    and check_type=0
            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                while(reader.Read())
                {
                    rtrn = new {
                        count = int.Parse(reader["num"].ToString()),
                        amount = decimal.Parse(reader["amount"].ToString())
                    };
                }
            }

            return rtrn;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public object ExtendedPaymentPlan(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT ss_number, Count(*) as num
                FROM payment_plan_checks 
                WHERE trans_date between '{0}' and '{1}'
                GROUP BY ss_number            ", 
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 
                var runningCount = 0;
                var runningTotal = 0;

                while(reader.Read())
                {
                    var count = int.Parse(reader["num"].ToString())/4;
                    
                    runningCount = runningCount + count;
                    runningTotal = runningTotal +(count * 500);
                }

                rtrn = new {
                    count = runningCount,
                    amount = runningTotal
                };
            }

            return rtrn;
        
        }


        /// <summary>
        /// 
        /// </summary>
        public object DefaultPaymentPlan(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            string command = string.Format(@"
                SELECT count(*) as num, status FROM transactions 
                WHERE trans_date between '{0}' and '{1}'
                    AND status = 'Default PP'
                group by status",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 

                while(reader.Read())
                {
                    rtrn = new {
                        count = int.Parse(reader["num"].ToString())
                    };
                }

            }

            return rtrn;
        
        }

        /// <summary>
        /// 
        /// </summary>
        public object DefaultPaymentPlanAPR(DateTime startDate, DateTime endDate)
        {
            object rtrn = null;
            
            var listIds = new List<APRClass>();

            string command = string.Format(@"
                SELECT id, amount_dispursed
                FROM transactions 
                WHERE trans_date between '{0}' and '{1}'
                    AND status = 'Default PP'",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00")); 	

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            { 

                while(reader.Read())
                {
                    
                    
                    var add = new APRClass() {
                        id = int.Parse(reader["id"].ToString()),
                        amountDispursed = double.Parse(reader["amount_dispursed"].ToString())
                    };

                    listIds.Add(add);
                }

            }

            string ids= "0";

            decimal maxApr = 0;
            decimal minApr = 1000;

            foreach(var element in listIds){
                string commandPaymentPlans = string.Format(@"
                    SELECT *
                    FROM payment_plan_checks 
                    WHERE transaction_id={0}", element.id.ToString()); 	

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, commandPaymentPlans))
                { 

                    DateTime transDate = DateTime.Now;
                    DateTime firstPaymentDay = DateTime.Now;
                    double payment = 0;

                    while(reader.Read())
                    {
                        //var add = int.Parse(reader["num"].ToString());
                        //listIds.Add(add);
                        transDate = DateTime.Parse(reader["trans_date"].ToString());
                        var paymentDay= DateTime.Parse( reader["date_due"].ToString());

                        if (paymentDay < firstPaymentDay)
                        {
                            firstPaymentDay = paymentDay;
                        }
                        
                        payment = double.Parse( reader["amount_recieved"].ToString());

                    }

                    var apr = APR(transDate, firstPaymentDay, element.amountDispursed,  payment);

                    if (apr > maxApr)
                    {
                        maxApr = apr;
                    }
                    
                    if (apr < minApr)
                    {
                        minApr = apr;
                    }


                }

            }



            return new { maxApr = maxApr, minApr=minApr };
        
        }

        private decimal APR(DateTime TransDate, DateTime FirstPaymentDay, double AmountFinanced, double Payment)
        {
            
            int N3;
            double Rtrn;

            N3 = TransDate.AddMonths(1).Subtract(FirstPaymentDay).Days;
            //N3 = FirstPaymentDay.Subtract(TransDate.AddMonths(1)).Days;

            double Adjustment = Financial.Rate(6, -Payment, AmountFinanced, 0);

            Rtrn = (Financial.Rate(6, -Payment, AmountFinanced * (Adjustment * 12) / 365 * N3 + AmountFinanced, 0) * 12) * 100;

            return Convert.ToDecimal(Rtrn);
        }

    }


    class APRClass
    {
        public int id;
        public double amountDispursed;
    }

}
