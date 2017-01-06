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
    public class DailyBalanceDetail
    {
        private string connectionString { get; set; }

        public DailyBalanceDetail(string connectionString)
        {
            this.connectionString = connectionString;
        }
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

        public List<object> CashedChecksCashLogDetail(DateTime startDate, DateTime endDate)
        {
            List<object> returnValue = new List<object>();

            string command = string.Format(
            @"
            SELECT transaction_number as id, payable_to as name, amount
            FROM cash_log 
            WHERE 
                `date` between '{0}' AND '{1}'
                and category = 'Cash Check'
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

        public List<object> CashedChecksTransactionsDetail(DateTime startDate, DateTime endDate)
        {
            List<object> returnValue = new List<object>();

            string command = string.Format(
                @"SELECT id, name, amount_dispursed, status
                      FROM transactions 
                      WHERE trans_date BETWEEN '{0}' AND '{1}'
                        AND (status <> 'Void') AND (check_type=1)
                    ", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd 23:59"));

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
                        amount = decimal.Parse(reader["amount_dispursed"].ToString()),
                        status = reader["status"].ToString()
                    };

                    returnValue.Add(obj);
                }
                reader.Close();
            }

            return returnValue;
        }

    }
}
