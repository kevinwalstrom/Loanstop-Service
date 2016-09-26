using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.DailyBalance
{
    public class Report
    {

        public List<Store> Stores;
        public string ReportDate;

        public List<DailySummaryEntity> RunReport(List<StoreConnectionType> storeList, DateTime reportDate)
        {

            var report = new List<DailySummaryEntity>();
			
            foreach (var storeConnection in storeList) {
                if (storeConnection.StoreName != "Internet") { 
                    Store currentStore = new Store(storeConnection, reportDate);
                    
                    var result = new DailySummaryEntity();
                    result.StoreName = currentStore.StoreName;
                    result.Undeposit = currentStore.SummaryDOs[0].Total;
                    result.UndepositCo = currentStore.SummaryDOs[1].Total;
                    result.Payments = currentStore.SummaryDOs[2].Total;
                    result.Receivables = result.Undeposit + result.UndepositCo - result.Payments;
                    result.Bounced = currentStore.SummaryDOs[3].Total;
                    result.Paid = currentStore.SummaryDOs[4].Total;
                    
                    var amountDispursed = currentStore.SummaryDOs[5].Total;
                    var amountRecieved = currentStore.SummaryDOs[6].Total;
                    var moneyOrderFees = currentStore.SummaryDOs[7].Total;

                    result.NetFees = amountRecieved - amountDispursed - result.Bounced + result.Paid + moneyOrderFees;
                    result.Cash = currentStore.SummaryDOs[8].Total;
                    result.Checking = currentStore.SummaryDOs[9].Total;
                    result.Transfers = currentStore.SummaryDOs[10].Total;
                    result.Expenses = currentStore.SummaryDOs[11].Total;

                    
                    report.Add(result);
                }
            }

            return report;
            
        }
    
    }

    public class StoreResultType 
    {
        public decimal[] Columns;

        public StoreResultType(SummaryColumns s)
        {
            foreach (SummaryColumns i in Enum.GetValues(typeof(SummaryColumns))) {
                Columns[(int)i] = new decimal(0);
            }
        }

    }

    //public class StoreConnectionType
    //{
    //    public string StoreName { get; set; }
    //    public string StoreIP { get; set; }
    //    public string DatabaseName { get; set; }
    //    public string DatabaseNameFormated { get; set; }
    //    public string CheckbookDatabaseName { get; set; }
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public string Port { get; set; }

    //    public string ConnectionString()
    //    {
    //        return "Database=" + DatabaseName + ";Data Source=" + StoreIP + ";User ID=" + Username + ";Password=" + Password + ";port=" + Port + "";
    //    }
    //}
}
