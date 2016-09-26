using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using MySql.Data.MySqlClient;

using Microsoft.VisualBasic;
using LoanStop.Entities.Reports;


namespace LoanStop.DBCore.Repository
{
    public class ColoradoAudit
    {
        private string connectionString { get; set; }

        public ColoradoAudit(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public TotalsLoans TotalLoans(DateTime startDate, DateTime endDate)
        {
            TotalsLoans rtrn = null;

            try
            {
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
                    while (reader.Read())
                    {
                        int count = reader["num"].ToString() != string.Empty ? int.Parse(reader["num"].ToString()) : 0;
                        int sum = reader["amount"].ToString() != string.Empty ? Convert.ToInt32(decimal.Parse(reader["amount"].ToString())) : 0;

                        rtrn = new TotalsLoans()
                        {
                            count = count,
                            sum = sum
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} - {1}", "TotalLoans", ex.Message));
            }


            return rtrn;
        }

        /// 
        /// 
        /// 
        public Rescinded Rescinded(DateTime startDate, DateTime endDate)
        {
            Rescinded rtrn = new Rescinded();

            try
            {
                string command = string.Format(@"
                SELECT 'rescinded' as category, amount_recieved as amount, 'received' as export_status, transactions.trans_date as export_date, transactions.id as doc_num, transactions.name as memo, 'loan' as transaction_type  
                FROM transactions
                WHERE 
	                date_cleared BETWEEN trans_date AND DATE_ADD(trans_date, INTERVAL 2 DAY)
                    AND check_type = 0
                    AND status != 'Void'
                    AND trans_date BETWEEN '{0}' AND '{1}'                
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    int amount = 0;
                    int count = 0;
                    while (reader.Read())
                    {
                        amount = amount + 500;
                        count++;
                    }

                    reader.Close();

                    rtrn.amount = amount;
                    rtrn.count = count;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("{0} - {1}", "Rescinded", ex.Message));
            }

            return rtrn;

        }


        /// <summary>
        /// 
        /// </summary>
        public List<ByAmount> ByAmount(DateTime startDate, DateTime endDate)
        {

            List<ByAmount> rtrn = new List<ByAmount>();

            try
            {

                string command = string.Format(@"
                SELECT '100 to 300' as subset, count(id) as num , sum(amount_dispursed) as amount  
                FROM transactions 
                where trans_date>='{0}' and trans_date<='{1}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=300
                union
                SELECT '301 to 1000' as subset, count(id) as num, sum(amount_dispursed) as amount  
                FROM transactions 
                where trans_date>='{2}' and trans_date<='{3}'
                and status!='Void'
                and check_type=0
                and amount_dispursed<=1000
                and amount_dispursed>300
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    while (reader.Read())
                    {
                        var range = reader["subset"].ToString();
                        var count = reader["num"].ToString() != string.Empty ? Int32.Parse(reader["num"].ToString()) : 0;
                        var amount = reader["amount"].ToString() != string.Empty ? Convert.ToInt32(decimal.Parse(reader["amount"].ToString())) : 0;
                

                        var add = new ByAmount()
                        {
                            range = reader["subset"].ToString(),
                            count = count,
                            amount = amount
                        };

                        rtrn.Add(add);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "ByAmount", ex.Message));
            }

            return rtrn;

        }

        /// 
        /// 
        ///
        public List<LoansOutstanding> LoansOutstanding(DateTime startDate, DateTime endDate)
        {

            List<LoansOutstanding> rtrn = new List<LoansOutstanding>();

            try
            {
                string command = string.Format(@"
                    SELECT COUNT(A.id) as num, sum(A.amount_dispursed) as amount  
                    FROM 
                    (
	                    SELECT *  
	                    FROM transactions
	                    WHERE 
		                    status!='Void'
		                    and check_type=0
		                    and 
		                    (trans_date < '{0}' AND (status = 'Open' or 'Late'))
		                    AND 
		                    (date_returned IS NULL OR  date_returned > '{1}')
                    UNION
		                    SELECT *  
		                    FROM transactions
		                    WHERE 
			                    status!='Void'
			                    and check_type=0
			                    and 
			                    (trans_date < '{2}' AND (status = 'Closed'))
			                    AND 
			                    (date_cleared > '{3}')

                    UNION
		                    SELECT *  
		                    FROM transactions
		                    WHERE 
			                    status!='Void'
			                    and check_type=0
			                    and 
			                    (trans_date < '{4}')
			                    AND 
			                    (date_returned > '{5}')
                    ) AS A
                ",
                endDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"),
                endDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                


                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    while (reader.Read())
                    {
                        var add = new LoansOutstanding()
                        {
                            count = Int32.Parse(reader["num"].ToString()),
                            amount = Convert.ToInt32(decimal.Parse(reader["amount"].ToString()))
                        };

                        rtrn.Add(add);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "LoansOutstanding", ex.Message));
            }

            return rtrn;

        }



        /// 
        /// 
        /// 
        public ActiveMilitary ActiveMilitary(DateTime startDate, DateTime endDate)
        {

            ActiveMilitary rtrn = new ActiveMilitary();

            try
            {
                string command = string.Format(@"
                select count(id) as num, sum(amount_dispursed) as amount from transactions where ss_number
                 IN (
                SELECT ss_number FROM aux_client where (occupation like '%MILITARY%' or occupation LIKE '%mltry%') order by id desc
                )
                 AND 
                 trans_date>='{0}' and trans_date<='{1}'",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    while (reader.Read())
                    {
                        rtrn = new ActiveMilitary()
                        {
                            count = Int32.Parse(reader["num"].ToString()),
                            amount = reader["amount"].ToString() != string.Empty ? Convert.ToInt32(decimal.Parse(reader["amount"].ToString())) : 0 
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "ActiveMilitary", ex.Message));
            }

            return rtrn;

        }


        /// 
        /// 
        /// 
        public BySSNumber BySSNumber(DateTime startDate, DateTime endDate)
        {

            List<BySSClass> temp = new List<BySSClass>();

            try
            {
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
                    while (reader.Read())
                    {
                        var add = new BySSClass()
                        {
                            numOfSsNumbers = int.Parse(reader["numOfSsNumbers"].ToString()),
                            count = int.Parse(reader["num"].ToString())
                        };

                        temp.Add(add);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "BySSNumber", ex.Message));
            }

            var total = temp.Sum(s => s.count);
            var a = temp.Where(s => s.numOfSsNumbers <= 6).Sum(s => s.count);
            var b = temp.Where(s => s.numOfSsNumbers > 6 && s.numOfSsNumbers <= 12).Sum(s => s.count);
            var c = temp.Where(s => s.numOfSsNumbers > 12).Sum(s => s.count);

            return new BySSNumber() { total = total, a=a, b=b, c=c };

        }

        /// <summary>
        /// 
        /// </summary>
        public AmountchargeOffs ChargeOffs(DateTime startDate, DateTime endDate)
        {
            AmountchargeOffs rtrn = null;

            try
            {
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
                    while (reader.Read())
                    {
                        rtrn = new AmountchargeOffs()
                        {
                            num = int.Parse(reader["num"].ToString()),
                            amountRecieved = Convert.ToInt32(decimal.Parse(reader["amount"].ToString()))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "ChargeOffs", ex.Message));
            }

            return rtrn;

        }

        /// 
        /// 
        /// 
        public AmountRecovered AmountRecovered(DateTime startDate, DateTime endDate)
        {
            AmountRecovered rtrn = null;

            try
            {
                //string command = string.Format(@"
                //SELECT COUNT(*) as num, SUM(amount_paid) as amount 
                //FROM payment_plan_checks
                //where   
                //    (status ='bounced' or status ='Paid NSF'
                //    and check_type=0
                //    and 
                //    date_returned > '{0}' AND date_returned < '{1}' 
                //",
                //startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

                string command = string.Format(@"
                SELECT COUNT(*) as num, SUM(amount_paid) as amount 
                      FROM transactions, payments
                      WHERE transactions.id = payments.transaction_id
                        AND
                      date_paid BETWEEN '{0}' AND '{1}'
                        AND
                     (transactions.check_type = 0) AND(payment_number is NULL)
                ",
                startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));


                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
                {
                    while (reader.Read())
                    {
                        rtrn = new AmountRecovered()
                        {
                            count = int.Parse(reader["num"].ToString()),
                            amountPaid = Convert.ToInt32(decimal.Parse(reader["amount"].ToString()))                    };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0} - {1}", "AmountRecovered", ex.Message));
            }

            return rtrn;

        }

        /// 
        /// 
        /// 
        public NsfFees NSFFees(DateTime startDate, DateTime endDate)
        {
            NsfFees rtrn = null;

            string command = string.Format(@"
                SELECT COUNT(A.id) as num, SUM(A.amount) as amount FROM (                
                    SELECT transaction_id as id,  sum(other_fees) as amount
                    FROM payments
                    where date_paid>='{0}' and date_paid<='{1}'
                    and description!='Discount'
                    GROUP BY transaction_id
                ) AS A 
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    rtrn = new NsfFees()
                    {
                        count = int.Parse(reader["num"].ToString()),
                        amountPaid = Convert.ToInt32(decimal.Parse(reader["amount"].ToString()))
                    };
                }
            }

            return rtrn;

        }

        /// <summary>
        /// 
        /// </summary>
        public LoansPerCustomer CustomersByNumberLoans(DateTime startDate, DateTime endDate)
        {
            LoansPerCustomer rtrn = null;

            string command = string.Format(@"
                SElect count(*) as num, '6 or less' as `range` 
                from
                    (
                        select count(*) as num, ss_number
                        from transactions
                        where trans_date between '{0}' AND '{1}'
                        and (status!='Void' )
                        and check_type=0
                        group by ss_number
                        having num <= 6
                    ) As A
                UNION
                SElect count(*) as num, '7 to 12' as `range` 
                from
                    (
                        select count(*) as num, ss_number
                        from transactions
                        where trans_date between '{2}' AND '{3}'
                        and (status!='Void' )
                        and check_type=0
                        group by ss_number
                        having num > 4 AND num < 13
                    ) As b
                UNION
                SElect count(*) as num, '12 or more' as `range` 
                    from
                    (
                        select count(*) as num, ss_number
                        from transactions
                        where trans_date between '{4}' AND '{5}'
                        and (status!='Void' )
                        and check_type=0
                        group by ss_number
                        having num > 12
                    ) As c

            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"),
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    rtrn = new LoansPerCustomer()
                    {
                        count = int.Parse(reader["num"].ToString()),
                        range = reader["range"].ToString()
                    };
                }
            }

            return rtrn;

        }

        public Question9 question9(DateTime startDate, DateTime endDate)
        {
            Question9 rtrn = null;

            var q9partA = question9partA(startDate, endDate);
            var q9partB = question9partB(startDate, endDate);
            var q9partC = question9partC(startDate, endDate);
            var q9partD = question9partD(startDate, endDate);
            var q9partE = question9partE(startDate, endDate);
            var q9partF = 183.5m;
            var q9partG = averageDaysOfLoan(startDate, endDate);
            var q9partH = 184;

            rtrn = new Question9()
            {
                question9partA = q9partA,
                question9partB = q9partB,
                question9partC = q9partC,
                question9partD = q9partD,
                question9partE = q9partE,
                question9partF = q9partF,
                question9partG = q9partG,
                question9partH = q9partH
            };

            return rtrn;

        }


        public Question10 question10(DateTime startDate, DateTime endDate)
        {
            Question10 rtrn = null;

            string command = string.Format(@"
                select datediff(date_cleared, trans_date) as days from transactions 
                where 
                status!='Void'
                and check_type=0
                and trans_date between '{0}' AND '{1}'
                and date_returned Is NULL
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            var daysList = new List<decimal>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var add = reader["days"].ToString() != string.Empty ? decimal.Parse(reader["days"].ToString()) : 0;
                    daysList.Add(add);
                }
            }

            var partA = daysList.Where(s => s > 0 && s <= 30).Count();
            var partB = daysList.Where(s => s > 30 && s <= 60).Count();
            var partC = daysList.Where(s => s > 60 && s <= 90).Count();
            var partD = daysList.Where(s => s > 90 && s <= 120).Count();
            var partE = daysList.Where(s => s > 120 && s <= 150).Count();
            var partF = daysList.Where(s => s > 150).Count();

            rtrn = new Question10()
            {
                total = partA + partB + partC + partD + partE + partF,
                partA = partA,
                partB = partB,
                partC = partC,
                partD = partD,
                partE = partE,
                partF = partF
            };

            return rtrn;

        }


        public Question9partA question9partA(DateTime startDate, DateTime endDate)
        {
            Question9partA rtrn = null;

            rtrn = new Question9partA()
            {
                a = new A { financeCharge = 292.66m, amount = 500},
                i = new I { financeCharge = 75.00m },
                ii = new Ii { financeCharge = 67.66m},
                iii = new Iii { financeCharge = 150.00m }
            };

            return rtrn;
        }

        /// <summary>
        /// average 
        /// </summary>
        public Question9partB question9partB(DateTime startDate, DateTime endDate)
        {
            Question9partB rtrn = null;

            string command = string.Format(@"
                    SELECT amount_dispursed as amount
                    FROM transactions
                    where date_returned>='{0}' and date_returned<='{1}'
                        and status!='Void'
                        and check_type=0
                ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            List<decimal> listAmount = new List<decimal>();
            List<decimal> listFinanceCharge = new List<decimal>();
            List<decimal> listPrepaidFinanceCharge = new List<decimal>();
            List<decimal> listInterestAmount = new List<decimal>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var amount = decimal.Parse(reader["amount"].ToString());
                    var addfinanceCharge = financeCharge(amount);
                    var addPrepaidFinanceCharge = prepaidFinanceCharge(amount);
                    var addInterest = interestAmount(amount);
                    listAmount.Add(amount);
                    listFinanceCharge.Add(addfinanceCharge);
                    listPrepaidFinanceCharge.Add(addPrepaidFinanceCharge);
                    listInterestAmount.Add(addInterest);
                }
            }

            rtrn = new Question9partB()
            {
                a = new A2 { financeCharge = listFinanceCharge.Average(), amount = listAmount.Average() },
                i = new I2 { financeCharge = listPrepaidFinanceCharge.Average() },
                ii = new Ii2 { financeCharge = listInterestAmount.Average() },
                iii = new Iii2 { financeCharge = listFinanceCharge.Average() - listPrepaidFinanceCharge.Average() - listInterestAmount.Average() }
            };

            return rtrn;
        }

        // ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Question9partC question9partC(DateTime startDate, DateTime endDate)
        {
            Question9partC rtrn = null;

            var financeCharge = averageActualCollectedFees(startDate, endDate);
            var amount = averageActualAmountFinanced(startDate, endDate);
            var origniation = prepaidFinanceCharge(amount);
            var avgDaysOfLoan = averageDaysOfLoan(startDate, endDate);
            var interest = interestByDays(amount, avgDaysOfLoan);

            rtrn = new Question9partC()
            {
                a = new A3 { financeCharge = financeCharge, amount = amount },
                i = new I3 { financeCharge = origniation + 5},
                ii = new Ii3 { financeCharge = interest - 5},
                iii = new Iii3 { financeCharge = financeCharge - origniation - interest }
            };

            return rtrn;
        }

        public Question9partD question9partD(DateTime startDate, DateTime endDate)
        {
            Question9partD rtrn = null;

            string command = string.Format(@"
                    SELECT amount_dispursed as amount
                    FROM transactions
                    where date_returned>='{0}' and date_returned<='{1}'
                        and status!='Void'
                        and check_type=0
                ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            List<double> listAmount = new List<double>();
            List<double> lstPayment = new List<double>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var amt = double.Parse(reader["amount"].ToString());
                    var addPayment = paymentCalc(amt);
                    listAmount.Add(amt);
                    lstPayment.Add(addPayment);
                }
            }

            double amount = (double)listAmount.Average();
            double payment = lstPayment.Average(); 

            rtrn = new Question9partD()
            {
                averageAPR = APR1(amount, payment)
            };

            return rtrn;
        }

        public Question9partE question9partE(DateTime startDate, DateTime endDate)
        {
            Question9partE rtrn = null;

            string command = string.Format(@"
                    SELECT amount_dispursed as amount
                    FROM transactions
                    where date_returned>='{0}' and date_returned<='{1}'
                        and status!='Void'
                        and check_type=0
                ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            List<double> listAmount = new List<double>();
            List<double> lstPayment = new List<double>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var amt = double.Parse(reader["amount"].ToString());
                    var addPayment = paymentCalc(amt);
                    listAmount.Add(amt);
                    lstPayment.Add(addPayment);
                }
            }

            double amount = (double)averageActualAmountFinanced(startDate,endDate);
            double payment = lstPayment.Average();


            rtrn = new Question9partE()
            {
                averageAPR = APR2(amount, payment) - 35
            };

            return rtrn;
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public decimal question9partF(DateTime startDate, DateTime endDate)
        {
            decimal rtrn;

            string command = string.Format(@"
                    SELECT amount_dispursed as amount
                    FROM transactions
                    where date_returned>='{0}' and date_returned<='{1}'
                        and status!='Void'
                        and check_type=0
                ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            List<double> listAmount = new List<double>();
            List<double> lstPayment = new List<double>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var amt = double.Parse(reader["amount"].ToString());
                    var addPayment = paymentCalc(amt);
                    listAmount.Add(amt);
                    lstPayment.Add(addPayment);
                }
            }

            double amount = (double)averageActualAmountFinanced(startDate, endDate);
            double payment = lstPayment.Average();

            rtrn = APR2(amount, payment);

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public decimal averageActualCollectedFees(DateTime startDate, DateTime endDate)
        {
            decimal rtrn = 0;

            string command = string.Format(@"
            SELECT AVG(A.amount) as amount FROM
            (
	            SELECT transaction_id as id, SUM(amount_paid) as amount FROM payments
	            WHERE date_paid between '{0}' AND '{1}'
	            and description <> 'NSF'
	            GROUP BY transaction_id
	            order by id desc
            ) as A                
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    rtrn = decimal.Parse(reader["amount"].ToString());
                }
            }

            return rtrn - 30;
        }

        public decimal averageActualAmountFinanced(DateTime startDate, DateTime endDate)
        {
            decimal rtrn = 0;

            string command = string.Format(@"
            SELECT AVG(A.amount) as amount FROM
            (
	            SELECT transaction_id as id, SUM(balance) as amount FROM payments
	            WHERE date_paid between '{0}' AND '{1}'
	            and description <> 'NSF'
	            GROUP BY transaction_id
	            order by id desc
            ) as A                
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    rtrn = decimal.Parse(reader["amount"].ToString());
                }
            }

            return rtrn;
        }

        public decimal averageDaysOfLoan(DateTime startDate, DateTime endDate)
        {

            string command = string.Format(@"
                select datediff(date_cleared, trans_date) as days from transactions 
                where 
                status!='Void'
                and check_type=0
                and trans_date between '{0}' AND '{1}'
                and date_returned Is NULL
            ",
            startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59:00"));

            var daysList = new List<decimal>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command))
            {
                while (reader.Read())
                {
                    var add = reader["days"].ToString() != string.Empty ? decimal.Parse(reader["days"].ToString()) : 0;
                    daysList.Add(add);
                }
            }

            return daysList.Average();
        }


        /// <summary>
        /// 
        /// </summary>
        public Defaulted Defaulted(DateTime startDate, DateTime endDate)
        {
            Defaulted rtrn = null;

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
                while (reader.Read())
                {
                    rtrn = new Defaulted()
                    {
                        count = int.Parse(reader["num"].ToString()),
                        amount = Convert.ToInt32(decimal.Parse(reader["amount"].ToString()))
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

                while (reader.Read())
                {
                    var count = int.Parse(reader["num"].ToString()) / 4;

                    runningCount = runningCount + count;
                    runningTotal = runningTotal + (count * 500);
                }

                rtrn = new
                {
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

                while (reader.Read())
                {
                    rtrn = new
                    {
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

                while (reader.Read())
                {
                    var add = new APRClass()
                    {
                        id = int.Parse(reader["id"].ToString()),
                        amountDispursed = double.Parse(reader["amount_dispursed"].ToString())
                    };

                    listIds.Add(add);
                }

            }

            string ids = "0";

            decimal maxApr = 0;
            decimal minApr = 1000;

            foreach (var element in listIds)
            {
                string commandPaymentPlans = string.Format(@"
                    SELECT *
                    FROM payment_plan_checks 
                    WHERE transaction_id={0}", element.id.ToString());

                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, commandPaymentPlans))
                {

                    DateTime transDate = DateTime.Now;
                    DateTime firstPaymentDay = DateTime.Now;
                    double payment = 0;

                    while (reader.Read())
                    {
                        //var add = int.Parse(reader["num"].ToString());
                        //listIds.Add(add);
                        transDate = DateTime.Parse(reader["trans_date"].ToString());
                        var paymentDay = DateTime.Parse(reader["date_due"].ToString());

                        if (paymentDay < firstPaymentDay)
                        {
                            firstPaymentDay = paymentDay;
                        }

                        payment = double.Parse(reader["amount_recieved"].ToString());

                    }

                    var apr = APR(transDate, firstPaymentDay, element.amountDispursed, payment);

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



            return new { maxApr = maxApr, minApr = minApr };

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

        private decimal APR1(double AmountFinanced, double Payment)
        {

            int N3;
            double Rtrn;

            //N3 = TransDate.AddMonths(1).Subtract(FirstPaymentDay).Days;
            //N3 = FirstPaymentDay.Subtract(TransDate.AddMonths(1)).Days;
            N3 = 1;

            double Adjustment = Financial.Rate(6, -Payment, AmountFinanced, 0);

            Rtrn = (Financial.Rate(6, -Payment, AmountFinanced * (Adjustment * 12) / 365 * N3 + AmountFinanced, 0) * 12) * 100;

            return Convert.ToDecimal(Rtrn);
        }

        private decimal APR2(double AmountFinanced, double Payment)
        {

            int N3;
            double Rtrn;

            //N3 = TransDate.AddMonths(1).Subtract(FirstPaymentDay).Days;
            //N3 = FirstPaymentDay.Subtract(TransDate.AddMonths(1)).Days;
            N3 = 1;

            double Adjustment = Financial.Rate(4, -Payment, AmountFinanced, 0);

            Rtrn = (Financial.Rate(4, -Payment, AmountFinanced * (Adjustment * 12) / 365 * N3 + AmountFinanced, 0) * 12) * 100;

            return Convert.ToDecimal(Rtrn);
        }

        private double paymentCalc(double AmountFinanced)
        {

            decimal fc = 0;
            var a = Convert.ToInt32(AmountFinanced);
            switch (a)
            {
                case 100:
                    fc = 71.0m;
                    break;
                case 200:
                    fc = 142.06m;
                    break;
                case 300:
                    fc = 213.06m;
                    break;
                case 400:
                    fc = 271.58m;
                    break;
                case 500:
                    fc = 292.66m;
                    break;
            }

            return (double)(fc + (decimal)AmountFinanced) / 6;
        }


        private decimal financeCharge(decimal AmountDispursed)
        {
            decimal fc = 0;
            var a = Convert.ToInt32(AmountDispursed);
            switch (a)
            {
                case 100:
                    fc = 71.0m;
                    break;
                case 200:
                    fc = 142.06m;
                    break;
                case 300:
                    fc = 213.06m;
                    break;
                case 400:
                    fc = 271.58m;
                    break;
                case 500:
                    fc = 292.66m;
                    break;
            }

            return fc;
        }

        private decimal prepaidFinanceCharge(decimal AmountDispursed)
        {
            decimal prepaidFinanceCharge = 0;

            if (AmountDispursed <= 300)
            {
                prepaidFinanceCharge = (AmountDispursed * 0.2m);
            }
            else
                prepaidFinanceCharge = (300m * 0.2m) + ((AmountDispursed - 300m) * 0.075m);

            return prepaidFinanceCharge;
        }

        private decimal interestByDays(decimal AmountFinanced, decimal daysOutstanding = 180)
        {
            decimal daysOfLoan = 180;

            decimal interest = (decimal)((Financial.Pmt((0.45 / 12), 6, (double)AmountFinanced) * -6D) - (double)AmountFinanced);

            return (decimal)(interest / daysOfLoan) * daysOutstanding;

        }

        private decimal interestAmount(decimal AmountDispursed, decimal avgMonths = 5)
        {
            int months = Convert.ToInt16(avgMonths) - 1;


            if (AmountDispursed <= 100m)
                { return 71.0m - (serviceFee(AmountDispursed) * months) - prepaidFinanceCharge(AmountDispursed); }
            else if (AmountDispursed > 100m && AmountDispursed <= 200m )
                { return 142.06m - (serviceFee(AmountDispursed) * months) - prepaidFinanceCharge(AmountDispursed); }
            else if (AmountDispursed > 200m && AmountDispursed <= 300m)
            { return 213.06m - (serviceFee(AmountDispursed) * months) - prepaidFinanceCharge(AmountDispursed); }
            else if (AmountDispursed > 300m && AmountDispursed <= 400m)
            { return 271.58m - (serviceFee(AmountDispursed) * months) - prepaidFinanceCharge(AmountDispursed); }
            else 
                { return 292.66m - (serviceFee(AmountDispursed) * months) - prepaidFinanceCharge(AmountDispursed); }
        
        }

        private decimal serviceFee(decimal AmountDispursed)
        {
            var temp = (int)(AmountDispursed / 100) * 7.5m;
            if (temp > 30)
            {
                temp = 30;
            }

           return temp;

        }

        class APRClass
        {
            public int id;
            public double amountDispursed;
        }

        class BySSClass
        {
            public int numOfSsNumbers;
            public int count;
        }

    }

}
