using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LoanStop.ACHFile
{
   public class CMySqlBase
    {

        protected string MySqlConn;

        public List<T> ReadData<T>(string sql, Action<string, string, T> ParseData) where T : new()
        {

            List<T> ReturnList = new List<T>();

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlConn, sql))
            {

                while (reader.Read())
                {
                    T FillData = new T();

                    try
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            string FieldName = reader.GetName(i).ToString();
                            string FieldValue = reader.GetValue(i).ToString();
                            ParseData(FieldName, FieldValue, FillData);
                        }

                        ReturnList.Add(FillData);

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);   
                    }
                }
            }

            return ReturnList;
        }

    
    }
}
