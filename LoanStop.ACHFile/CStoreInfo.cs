using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanStop.ACHFile
{
    class CStoreInfo
    {
        public string IP { get; set; }
        public string Store { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //public string IP = MasterReader["ip"].ToString();
        //string store = MasterReader["store"].ToString();
        //string database_name = MasterReader["database_name"].ToString();
        //string port = MasterReader["port"].ToString();
        //string StoreName = MasterReader["Account_info"].ToString();
        //string StoreCode = MasterReader["store_code"].ToString();

        public static void ParseQuery(string FieldName, string FieldValue, CStoreInfo FillData)
        {

            switch (FieldName)
            {
                case "ip":
                    FillData.IP = FieldValue;
                    break;
                case "store":
                    FillData.Store = FieldValue;
                    break;
                case "database_name":
                    FillData.DatabaseName = FieldValue;
                    break;
                case "port":
                    FillData.Port = FieldValue;
                    break;
                case "account_info":
                    FillData.StoreName = FieldValue;
                    break;
                case "store_code":
                    FillData.StoreCode = FieldValue;
                    break;
                case "username":
                    FillData.Username = FieldValue;
                    break;
                case "password":
                    FillData.Password = FieldValue;
                    break;
                default:
                    break;
            }



        }
    
    }
}
