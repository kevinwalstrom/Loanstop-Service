using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LoanStop.DailyBalance
{
    class DALBase
    {
        public static string ConnectionStringKey = "master";
        public static bool EmptyStringIsNullValue = true;

        // GetDbMySqlCommand
        public static MySqlCommand GetDbMySqlCommand(string sqlQuery)
        {
            MySqlCommand command = new MySqlCommand();
            //command.Connection = GetDbConnection();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = sqlQuery;

            return command;
        }

    }
}
