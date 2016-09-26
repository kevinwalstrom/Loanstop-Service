using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Reports;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.ACHFile
{
    public class ACH : CMySqlBase 
    {

        public string NavigateUrl {get; set;}
        public string FileName { get; set; }
        public string FileBasePath { get; set; }
        public CNACHAFile NACHAFile { get; set; }

        public void Execute(List<StoreConnectionType> storeList, DateTime depositDate, DateTime bankToDepositDate)
        {

            int NumberOfLines = 0;

            NACHAFile = new CNACHAFile(FileBasePath);

            FileName = NACHAFile.filename;

            NACHAFile.DepositDate = depositDate;
            NACHAFile.BankToDepositDate = bankToDepositDate;
            NACHAFile.PrintFileHeaderRecord();
            NumberOfLines = NumberOfLines + 1;

            decimal filedebit = 0;
            UInt32 fileHAsh = 0;

            int BatchCount = 0;
            UInt32 Hash = 0;
            decimal totalCredit = 0;
            decimal totalDebit = 0;

            List<object> TotalStoresACH = new List<object>();

            foreach (var storeConnection in storeList)
            {

                string StoreConnectionString = storeConnection.ConnectionString();

                    List<object> StoresACH = new List<object>();
                    int Entries = 0;
                    decimal StoreCredit = 0;
                    totalDebit = 0;

                    GetDeposits(depositDate, StoreConnectionString, StoresACH, TotalStoresACH, storeConnection.DatabaseName, storeConnection.StoreCode);

                    if (StoresACH.Count > 0)
                    {

                        if (storeConnection.AccountInfo != null && storeConnection.AccountInfo.Length > 2) 
                        { 
                            BatchCount = BatchCount + 1;
                            NACHAFile.PrintBatchHeaderRecord(storeConnection.AccountInfo, storeConnection.StoreCode, BatchCount);
                            NumberOfLines = NumberOfLines + 1;

                            foreach (CACHDebit ACHDebit in StoresACH)
                            {
                                NumberOfLines = NumberOfLines + 1;
                                Entries = Entries + 1;
                                NACHAFile.PrintDetailRecord(ACHDebit, Entries);
                                StoreCredit = StoreCredit + ACHDebit.Amount;
                                Hash = Hash + Convert.ToUInt32(ACHDebit.RoutingNumber.Substring(0, 8));
                                totalDebit = totalDebit + ACHDebit.Amount;
                            }
                            NACHAFile.PrintBatchTrailerRecord(TotalStoresACH.Count, Hash, totalDebit, BatchCount, storeConnection.StoreCode);
                            NumberOfLines = NumberOfLines + 1;
                        }
                    }

                    filedebit = filedebit + StoreCredit;
                    fileHAsh = fileHAsh + Hash;
                }
                

            fileHAsh = fileHAsh + Hash;

            NACHAFile.PrintFileTrailerRecord(BatchCount, TotalStoresACH.Count, fileHAsh, totalDebit, totalCredit, NumberOfLines);
            NumberOfLines = NumberOfLines + 1;

            NACHAFile.CloseFile(NumberOfLines);

            NavigateUrl = "~/ACH/" + NACHAFile.filename + ".txt"; 

        }

        private void GetDeposits(DateTime ToDate, string cn, List<object> StoresACH, List<object> TotalStoresACH, string store, string Storecode)
        {
        
            string sql;           

            sql = " " +
            " SELECT payment_plan_checks.id as id, transaction_id,name,payment_plan_checks.ss_number as ssNumber,bank_account AS AccountNumber,payment_plan_checks.status,amount_recieved, routing_number as RoutingNumber , '0' " +
            " FROM payment_plan_checks " +
            "     JOIN client ON client.ss_number =payment_plan_checks.ss_number " +
            "     JOIN aux_client ON aux_client.ss_number =payment_plan_checks.ss_number " +
            " where payment_plan_checks.date_cleared='" + ToDate.ToString("yyyy-MM-dd") + "' and payment_plan_checks.status='Cleared' AND payment_plan_checks.check_number='ACH' ";

            base.MySqlConn = cn;
            List<CACHDebit> QueryData = ReadData<CACHDebit>(sql, ParseACHQuery);

            foreach (CACHDebit d in QueryData){
                d.RoutingCheckDigit ="0";
                d.StoreCode = Storecode; 
                StoresACH.Add(d);
                TotalStoresACH.Add(d);
            }
        }

        private void ParseACHQuery(string FieldName, string FieldValue, CACHDebit FillData){

            switch (FieldName)
            {
                case "name":
                    FillData.Name = FieldValue ;
                    break;
                case "amount_recieved":
                    FillData.Amount = Convert.ToDecimal(FieldValue);
                    break;
                case "AccountNumber":
                    if (FieldValue.Length < 2) 
                        FillData.AccountNumber = "0";
                    else
                        FillData.AccountNumber = FieldValue ;
                    break;
                case "RoutingNumber":
                    if (FieldValue.Length < 2) 
                        FillData.RoutingNumber = "0";
                    else
                        FillData.RoutingNumber = FieldValue ;
                    break;
                case "ssNumber":
                    FillData.SSNumber = FieldValue ;
                    break;
                default:
                    break;
            }

        }


    }
}
