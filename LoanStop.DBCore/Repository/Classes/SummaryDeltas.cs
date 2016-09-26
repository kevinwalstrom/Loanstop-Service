using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace LoanStop.DBCore.Repository
{
    public class SummaryDeltas
    {

        public string ConnectionString { get; set; }

        public SummaryDeltas(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Insert(DateTime theDate, decimal loans, decimal principalPaid, decimal principalBounced)
        {

            string command = string.Format(@"
                INSERT INTO summary_deltas (summary_date,loans,principal_paid,principal_bounced)
                VALUES ('{0}',{1},{2},{3})     

            ", theDate.ToString("yyyy-MM-dd"), loans.ToString(), principalPaid.ToString(), principalBounced.ToString());


           MySqlHelper.ExecuteNonQuery(ConnectionString, command);

        }
    }
}
