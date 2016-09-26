using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LoanStop.ACHFile
{
    public class CNACHAFile
    {
    
        public List<string> file { get; set; }
        public string filename { get; set; }

        public DateTime DepositDate { get; set; }
        public DateTime BankToDepositDate { get; set; }
        public string OutputFile { get; set; }
        public string MYSQLCon  { get; set; }
        
        protected string ImmediateOrigin = " 841226664";
        //protected string ACHFilterTransmission;
        protected string CompanyName = "B&R Checkholders";
        protected string ComapanyInformation  = "                    ";
        protected string CompannyIdentification  = "          ";

        //public ACHDebitRecords As Collection;

        public CNACHAFile(string FileBasePath)
        {

            filename = "RPP0000-" + DateTime.Now.ToString("yyMMdd");
            file = new List<string>();
            //file = new System.IO.StreamWriter(FileBasePath + filename + ".txt");
        
        }

        public void CloseFile(int NumberOfLines){
      
            string temstring = "9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999" + "\r";

            while ((NumberOfLines % 10) != 0){
                file.Add(temstring.Substring(0, 94) + "\r");
                NumberOfLines = NumberOfLines + 1;
            }

            //file.Close();
        
        }

        public void PrintFileHeaderRecord(){

            string PrintRecord;

            PrintRecord = "1";                       // 1 character
            PrintRecord = PrintRecord + "01";                      // 2 character
            PrintRecord = PrintRecord + " 102003154";              // 10 character
            PrintRecord = PrintRecord + "1841226664";           // 10 character
            PrintRecord = PrintRecord + DepositDate.ToString("yyMMdd"); //"YYMMDD                 ' 6 character  
            PrintRecord = PrintRecord + DateTime.Now.ToString("hhmm"); //HHMM                ' 4 character
            PrintRecord = PrintRecord + "1"; //Number               // 1 character
            PrintRecord = PrintRecord + "094";                     // 3 character 
            PrintRecord = PrintRecord + "10";                      // 2 character  
            PrintRecord = PrintRecord + "1";                       // 1 character
            PrintRecord = PrintRecord + "VECTRA_BANK____________";               //23 characters 
            PrintRecord = PrintRecord + "LOANSTOP_______________";               // 23 characters with company name 
            PrintRecord = PrintRecord + "________";                // 8 characters

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + "_";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }
    
        public void PrintBatchHeaderRecord(string StoreName, string StoreCode, int BatchNumber){

            string PrintRecord;

            PrintRecord = "5";                      // 1 character
            PrintRecord = PrintRecord + "225";                     // 3 character
            PrintRecord = PrintRecord + StoreName;        // 16 character
            PrintRecord = PrintRecord + "____________________";    // 20 character
            PrintRecord = PrintRecord + StoreCode;              // 10 character  
            PrintRecord = PrintRecord + "PPD"; //HHMM                // 3 character
            PrintRecord = PrintRecord + "LOANPAYMNT"; //Number               // 10 character
            PrintRecord = PrintRecord + DepositDate.ToString("yyMMdd");                      // 6 character 
            PrintRecord = PrintRecord + BankToDepositDate.ToString("yyMMdd");                      /// 6 character  
            PrintRecord = PrintRecord + "___" ;                      // 3 character
            PrintRecord = PrintRecord + "1";               // 1 characters 
            PrintRecord = PrintRecord + "10200315";                     // 8 characters
            PrintRecord = PrintRecord + BatchNumber.ToString("0000000"); // 7 characters 

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + "_";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");        // 94 characters 
            }
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }
    

        public void PrintCreditBatchHeaderRecord(string AccountInfo, int BatchNumber){

            string PrintRecord;

            PrintRecord = "5";                       // 1 character
            PrintRecord = PrintRecord + "200";                     // 3 character
            PrintRecord = PrintRecord + "BR Checkholders ";        // 16 character
            PrintRecord = PrintRecord + "                    ";    // 20 character
            PrintRecord = PrintRecord + "1841226664";              // 10 character  
            PrintRecord = PrintRecord + "PPD"; //HHMM                ' 3 character
            PrintRecord = PrintRecord + "LOAN PMT  "; //Number               ' 10 character
            PrintRecord = PrintRecord + "      ";                     // 6 character 
            PrintRecord = PrintRecord + DepositDate.ToString("yyMMdd");                      // 6 character  
            PrintRecord = PrintRecord + "   ";                       // 3 character
            PrintRecord = PrintRecord + "1";               // 1 characters 
            PrintRecord = PrintRecord + "07007029";                     // 9 characters
            PrintRecord = PrintRecord + BatchNumber.ToString("0000000"); // 7 characters 

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + " ";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
        }

        public void PrintDetailRecord(CACHDebit A, int RecordCount){

            string PrintRecord;

            PrintRecord = "6";  // 1 character
            PrintRecord = PrintRecord + "27";      // 2 character
            PrintRecord = PrintRecord + A.RoutingNumber.Substring(0, 9);      // 8 character
            PrintRecord = PrintRecord + A.AccountNumber;      // 17 character
            PrintRecord = PrintRecord + A.Amount.ToString("00000000.00").Replace(".", "");      // 10 character
            PrintRecord = PrintRecord + A.StoreCode + "_" + A.SSNumber.Substring(7, 4);       // 15 character
            PrintRecord = PrintRecord + A.Name;      // 22 character
            PrintRecord = PrintRecord + "___";         // 2 character
            PrintRecord = PrintRecord + "0";                       // 1 character
            PrintRecord = PrintRecord + A.RoutingNumber.Substring(0, 8) + RecordCount.ToString("0000000");         // 15 characters 

            if  (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + " ";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }

        public void PrintCreditRecord(CACHDebit A, int RecordCount){

            string PrintRecord;

            PrintRecord = "6";   // 1 character
            PrintRecord = PrintRecord + "22";      // 2 character
            PrintRecord = PrintRecord + A.RoutingNumber.Substring(0, 8);      // 8 character
            PrintRecord = PrintRecord + A.RoutingNumber.Substring(8, 1);      // 1 character
            PrintRecord = PrintRecord + A.AccountNumber;      // 17 character
            PrintRecord = PrintRecord + A.Amount.ToString("00000000.00").Replace(".", "");      // 10 character
            PrintRecord = PrintRecord + "    " + A.SSNumber;      // 15 character
            PrintRecord = PrintRecord + A.Name;      // 22 character
            PrintRecord = PrintRecord + "   ";      // 2 character
            PrintRecord = PrintRecord + "0";                       // 1 character
            PrintRecord = PrintRecord + A.RoutingNumber.Substring(0, 8) + RecordCount.ToString("0000000");         // 15 characters 

            if (PrintRecord.Length != 94){ 
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + " ";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
        }


        public void PrintCreditBatchTrailerRecord(int RecordCount, int EntryHash, decimal TotalDebit, decimal TotalCredit, int BatchNumber, string AccountInfo){

            string PrintRecord;

            PrintRecord = "8";                       // 1 character
            PrintRecord = PrintRecord + "200";                      // 3 character
            PrintRecord = PrintRecord + RecordCount.ToString("000000");              // 6 character
            PrintRecord = PrintRecord + EntryHash.ToString("0000000000");              // 10 character
            PrintRecord = PrintRecord + TotalCredit.ToString("0000000000.00").Replace(".", ""); //"YYMMDD                 ' 12 character  
            PrintRecord = PrintRecord + TotalDebit.ToString("0000000000.00").Replace(".", ""); //HHMM                ' 12 character
            PrintRecord = PrintRecord + "1841226664"; //Number               ' 10 character
            PrintRecord = PrintRecord + "                         ";                     // 25 character 
            PrintRecord = PrintRecord + "07007029";                // 8 character  
            PrintRecord = PrintRecord + BatchNumber.ToString("0000000"); // 7 characters 

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + " ";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }

        public void PrintBatchTrailerRecord(int RecordCount, UInt32 EntryHash, decimal TotalDebit, int BatchNumber, string StoreCode){

            string PrintRecord;

            PrintRecord = "8";                       // 1 character
            PrintRecord = PrintRecord + "200";                      // 3 character
            PrintRecord = PrintRecord + RecordCount.ToString("000000") ;             // 6 character
            PrintRecord = PrintRecord + EntryHash.ToString("0000000000");              // 10 character
            PrintRecord = PrintRecord + TotalDebit.ToString("0000000000.00").Replace(".", ""); //"YYMMDD                 ' 12 character  
            PrintRecord = PrintRecord + "000000000000"; //HHMM                ' 12 character
            PrintRecord = PrintRecord + StoreCode; //Number               ' 10 character
            PrintRecord = PrintRecord + "___________________";                     // 19 character 
            PrintRecord = PrintRecord + "______";                     // 19 character 
            PrintRecord = PrintRecord + "10200315";                // 8 character  
            PrintRecord = PrintRecord + BatchNumber.ToString("0000000"); // 7 characters 

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + "_";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }    
            else
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }

        public void PrintFileTrailerRecord(int Batches, int Entries, UInt32 EntryHash, decimal TotalDebit, decimal TotalCredit, int numberoflines){

            string PrintRecord;
            int Blocks = 26;

            if (numberoflines % 10 > 5)
                Blocks = (numberoflines / 10);
            else
                Blocks = (numberoflines / 10);


            PrintRecord = "9";                       // 1 character
            PrintRecord = PrintRecord + Batches.ToString("000000");                      // 6 character
            PrintRecord = PrintRecord + Blocks.ToString("000000");          // 6 character
            PrintRecord = PrintRecord + Entries.ToString("00000000");              // 8 character
            PrintRecord = PrintRecord + EntryHash.ToString("0000000000");              // 10 character
            PrintRecord = PrintRecord + TotalDebit.ToString("0000000000.00").Replace(".", ""); //"YYMMDD                 ' 12 character  
            PrintRecord = PrintRecord + TotalCredit.ToString("0000000000.00").Replace(".", ""); //Number               ' 12 character
            PrintRecord = PrintRecord + "_______________________________________";                     // 39 character 

            if (PrintRecord.Length != 94){
                if (PrintRecord.Length < 94){
                    while (PrintRecord.Length < 94){
                        PrintRecord = PrintRecord + "_";
                    }
                }
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 
            }        
            else 
                file.Add(PrintRecord.Substring(0, 94) + "\r");         // 94 characters 

        }

    
    
    }
}
