using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LoanStop.DailyBalance
{

        public class DO : SummaryBase
        {
            public MySqlConnection MySqlConn;
            private string _total;
            private decimal dTotal;
            public decimal Total
            {
                get
                {
                    if (decimal.TryParse(_total, out dTotal))
                    {
                        //do whatever...
                    }
                    return dTotal;
                }
            }

            public DO(string sql)
            {
                _sql = sql;
            }

            public void Execute()
            {
                MySqlCommand cmd = DALBase.GetDbMySqlCommand(_sql);
                cmd.Connection = MySqlConn;
                cmd.Parameters.AddWithValue("@TheDate", TheDate.ToString("yyyy-MM-dd"));
                _total = cmd.ExecuteScalar().ToString();
            }

        };

        public class SummarySqlDO : DO
        {

            public SummarySqlDO(string sql)
                : base(sql)
            {
                _sql = sql;
            }

        };

        public class SummaryMathDO : DO
        {

            public SummaryMathDO(string sql)
                : base(sql)
            {
                _sql = sql;
            }

        }
}
