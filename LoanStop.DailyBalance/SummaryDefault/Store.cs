using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.DailyBalance
{
    public class Store
    {

        public List<DO> SummaryDOs;
        public MySqlConnection StoreConnection;
        public string StoreName;

        public Store(StoreConnectionType storeInfo, DateTime ReportDate)
        {
            SummaryDOs = new List<DO>();

            StoreName = storeInfo.StoreName;  
            StoreConnection = new MySqlConnection(storeInfo.ConnectionString());
            StoreConnection.Open();

            foreach (SummaryColumns i in Enum.GetValues(typeof(SummaryColumns))) {

                Queries.CheckbookDatabaseName = storeInfo.DatabaseName; 
                Queries.DatabaseName =  storeInfo.DatabaseName;

                DO NewSummaryDO = Queries.SetObject(i);
                
                NewSummaryDO.MySqlConn = StoreConnection;

                NewSummaryDO.TheDate = ReportDate;

                NewSummaryDO.Execute();

                SummaryDOs.Add(NewSummaryDO);
            }

            StoreConnection.Close();

        }

        public string GetSummaryResult()
        {
            string NewLine = "";

            foreach (DO s in SummaryDOs)
            {
                NewLine = NewLine + " " + s.Total; 
            }

            return NewLine;
        }

    
    }
}
