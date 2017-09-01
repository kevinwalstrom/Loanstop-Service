using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Entity = LoanStopModel;
using LoanStop.Entities.Export;
using LoanStop.DBCore.Repository.Interfaces;

namespace LoanStop.DBCore.Collections
{
    public class Reports
    {

        private string connectionString { get; set; }

        public Reports(string connectionString)
        {

            this.connectionString = connectionString;
        }

        public List<object> AccountsToFollowUp()
        {
            var rtrnList = new List<object>();

            using (var db = new Entity.LoanStopEntities(connectionString))
            {

                string command2 = string.Format(
                    @"
                    SELECT * FROM 
                    ( 
                       SELECT a.name, MAX(date_entered) as TheDate FROM 
                       ( 
                           SELECT collect.id as id, transactions.name as name, transactions.status 
                           FROM collect  
                              JOIN transactions ON transactions.id = collect.transaction_id 
                           WHERE  
                               transactions.status='Late' 
                                AND 
                                ((collect.payment_agreement < CURRENT_DATE) Or (payment_agreement Is NULL)) 
                       ) as a 
                            LEFT OUTER JOIN collection_notes ON a.id = collection_notes.collection_id 
                            group by name 
                    ) as b 
                    WHERE  
                        ((CURRENT_DATE > DATE_ADD(TheDate, INTERVAL 2 DAY)) OR TheDate IS NULL)");

                command2 = command2.Replace("\r\n", "");
                command2 = command2.Replace("\n", "");


                using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, command2))
                {
                    while (reader.Read())
                    {
                        string name = reader["name"].ToString() ;
                        string theDate = reader["TheDate"].ToString();

                        var rtrn = new 
                        {
                            Name = name,
                            TheDate = theDate
                        };

                        rtrnList.Add(rtrn);

                    }
                }




            }

            return rtrnList;

        }

    }
}
