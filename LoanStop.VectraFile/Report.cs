using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.VectraFile
{
    public class Vectra : CMySqlBase
    {

        public string NavigateUrl { get; set; }
        public string FileName { get; set; }
        public string FileBasePath { get; set; }
        public List<string> File { get; set; }

        public void Execute(List<StoreConnectionType> storeList, DateTime toDate)
        {
            //base.MySqlConn = MYSQLConMaster;
            //string MasterSQL = "SELECT store, ip, checking_database_name, account_info, store_code, account_number FROM mysql_ips WHERE Location='Colorado' ORDER BY store";
            //List<CStoreInfo> QueryData = ReadData<CStoreInfo>(MasterSQL, CStoreInfo.ParseQuery);

            List<CCheck> TotalStoresChecks = new List<CCheck>();

            foreach (StoreConnectionType Store in storeList)
            {

                //string StoreConnectionString = "Server=" + Store.IP + ";Database=" + Store.CheckingDatabaseName + ";port=" + Store.Port + ";Uid=program;Pwd=1payday1; Connection Timeout=10;";
                string StoreConnectionString = Store.ConnectionString();

                if (Store.DatabaseName != "test")
                {

                    List<CCheck> StoreChecks= GetChecks(toDate, StoreConnectionString, Store.DatabaseName);

                    if (StoreChecks.Count > 0)
                    {
                        foreach (CCheck d in StoreChecks)
                        {
                            CCheck NewCheck = new CCheck();
                            NewCheck.AccountNumber = Store.AccountInfo;
                            NewCheck.CheckNumber = d.CheckNumber;
                            NewCheck.VoidCode = " ";
                            NewCheck.CheckDate = d.CheckDate;
                            NewCheck.Amount = d.Amount;
                            NewCheck.PayableTo = d.PayableTo;
                            TotalStoresChecks.Add(NewCheck);
                        }
                    }
                }
            } //end stores loop


            FileName = "RPP0000-" + DateTime.Now.ToString("yyMMdd");
            //System.IO.StreamWriter file = new System.IO.StreamWriter(FileBasePath + filename + ".txt");
            //System.IO.StreamWriter file = new System.IO.StreamWriter(FileBasePath + FileName + ".csv");
            File = new List<string>();

            foreach (CCheck d in TotalStoresChecks)
            {
                string tempstring = d.AccountNumber + "," + d.CheckNumber + "," + d.VoidCode + "," + d.CheckDate + "," + d.Amount + "," + d.PayableTo + " \r\n"; 
                File.Add(tempstring + "\r");
            }
            
            //file.Close();

        }

        private List<CCheck> GetChecks(DateTime ToDate, string cn, string CheckBookDatabaseName)
        {
        
            string sql = " " +
            " SELECT check_number, date_entered, amount, payable_to " +
            " FROM " + CheckBookDatabaseName + ".checkbook " +
            " WHERE date_entered='" + ToDate.ToString("yyyy-MM-dd") + " 00:00:00' AND " + 
            "  transaction_type IN ('CORP', 'Expense')";

            base.MySqlConn = cn;

            List<CCheck> StoresChecks = ReadData<CCheck>(sql, ParseQuery);

            return StoresChecks; 
        
        }


        public static void ParseQuery(string FieldName, string FieldValue, CCheck FillData)
        {
            switch (FieldName)
            {
                case "check_number":
                    FillData.CheckNumber = FieldValue;
                    break;
                case "date_entered":
                    FillData.CheckDate = FieldValue;
                    break;
                case "amount":
                    if (FieldValue.Length < 2)
                        FillData.Amount = "0";
                    else
                        FillData.Amount = FieldValue;
                    break;
                case "payable_to":
                    FillData.PayableTo = FieldValue.Replace(","," ");
                    break;
                default:
                    break;
            }


        }

    
    }

    
    public class CCheck
    {
        public string AccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public string VoidCode { get; set; }
        public string CheckDate { get; set; }
        public string Amount { get; set; }
        public string PayableTo { get; set; }
    }



    public class CStoreInfo
    {
        public string IP { get; set; }
        public string Store { get; set; }
        public string DatabaseName { get; set; }
        public string CheckingDatabaseName { get; set; }
        public string Port { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string AccountNumber { get; set; }

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
                case "checking_database_name":
                    FillData.CheckingDatabaseName = FieldValue;
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
                case "account_number":
                    FillData.AccountNumber = FieldValue;
                    break;
                default:
                    break;
            }



        }

    }
}
