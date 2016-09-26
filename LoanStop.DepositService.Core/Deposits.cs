using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

using LoanStop.DBCore;
using LoanStopModel;
using Repository = LoanStop.DBCore.Repository;

using LoanStop.DepositService.DataContracts;
//using LoanStop.MicroBuilt.Core;

    
namespace LoanStop.DepositService.Core
{
    public class Deposits
    {
        public string ConnectionString { get; set; }
        public string Store { get; set; }
        public string clverifyURI { get; set; }
        public string clverifyAuthorizeString { get; set; }

        public GetDepositsResponse GetDepositsColorado(DateTime queryDate)
        {

            GetDepositsResponse returnList = new GetDepositsResponse();

            List<DepositEntity> depositsList = new List<DepositEntity>();
            List<DepositEntity> achDeposits = new List<DepositEntity>();
            List<DepositEntity> checkDeposits = new List<DepositEntity>();
            List<DepositEntity> closedAccounts = new List<DepositEntity>();

            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                con.Open();

                //MySqlDataReader reader = MySqlHelper.ExecuteReader(ConnectionString, sql);

                string sql = string.Format("CALL deposits('{0}')", queryDate.ToString("yyyy-MM-dd"));
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, sql);
                
                Repository.Transaction transRepository = new Repository.Transaction(ConnectionString);
                Repository.Client clientRepository = new Repository.Client(ConnectionString);

                while (reader.Read())
                {

                    var newItem = new DepositEntity();

                    newItem.Name = reader["Name"].ToString();
                    Debug.WriteLine("Customer Name : " + newItem.Name);
                    newItem.CheckNumber = reader["check_number"].ToString();
                    newItem.DepositAmount = decimal.Parse(reader["amount_recieved"].ToString());
                    newItem.Deposit = false;
                    newItem.PaymentPlanCheckId = long.Parse(reader["id"].ToString());
                    newItem.TransactionId = long.Parse(reader["transaction_id"].ToString());
                    newItem.CheckType = reader[8].ToString();
                    newItem.Status = reader["status"].ToString();
                    newItem.AmountRecieved = decimal.Parse(reader["amount_recieved"].ToString());

                    DateTime dateDue;
                    if (DateTime.TryParse(reader["date_due"].ToString(), out dateDue))
                        newItem.DateDue = dateDue;
                    else
                        newItem.DateDue = null;

                    if (!string.IsNullOrEmpty(reader[10].ToString()))
                        newItem.RoutingNumber = reader[10].ToString();

                    newItem.Transaction = transRepository.GetById(newItem.TransactionId);
                    newItem.Client = clientRepository.GetBySSNumber(newItem.Transaction.SsNumber);

                    if (newItem.CheckType == "0")
                    {
                        if (newItem.Name.Substring(0, 1).ToUpper() == "K")
                        {
                            var name = newItem.Name;
                        }

                        if (CoPayoffAmount(newItem.Transaction, queryDate) < newItem.DepositAmount)
                            newItem.DepositAmount = CoPayoffAmount(newItem.Transaction, queryDate);
                        
                    }
                    depositsList.Add(newItem);
                }
                reader.Close();
            }//end using con

            foreach (var d in depositsList)
            {
                if (d.IsValidLoanForDeposit) achDeposits.Add(d);

                if (d.IsValidCheckCashForDeposit) achDeposits.Add(d);

                if (d.IsClosedAccount) closedAccounts.Add(d);
            }

            returnList.ACHDeposits = achDeposits;
            returnList.ClosedAccounts = closedAccounts;

            return returnList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GetDepositsResponse GetDepositsWyoming(DateTime queryDate)
        {
            GetDepositsResponse returnList = new GetDepositsResponse();

            //string CONNECTIONSTRING = "Database=southdenver;Data Source=us-cdbr-azure-west-b.cleardb.com;User Id=b91e321d074c50;Password=5cfa9c6c; Pooling=True; MinimumPoolSize=3;maximumpoolsize=14;";
            List<DepositEntity> depositsList = new List<DepositEntity>();
            List<DepositEntity> achDeposits = new List<DepositEntity>();
            List<DepositEntity> checkDeposits = new List<DepositEntity>();
            List<DepositEntity> closedAccounts = new List<DepositEntity>();

            using (MySqlConnection con = new MySqlConnection(ConnectionString))
            {
                con.Open();

                string sql = string.Format("CALL deposits('{0}')", queryDate.ToString("yyyy-MM-dd"));
                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, sql);

                Repository.Transaction transRepository = new Repository.Transaction(ConnectionString);
                Repository.Client clientRepository = new Repository.Client(ConnectionString);

                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(reader["date_due"].ToString()))
                    {
                        if (reader["status"].ToString() == "Pick Up" || reader["status"].ToString() == "Deposit" || reader["status"].ToString() == "Pick Up-NC" || reader["status"].ToString() == "Partial")
                        {
                            if (reader["status"].ToString() != "Voided")
                            {
                                if (reader["status"].ToString() != "Void")
                                {
                                    var newItem = new DepositEntity();

                                    newItem.Name = reader["Name"].ToString();
                                    Debug.WriteLine("Customer Name : " + newItem.Name);
                                    newItem.CheckNumber = reader["check_number"].ToString();
                                    newItem.DepositAmount = decimal.Parse(reader["amount_recieved"].ToString());
                                    newItem.Deposit = false;
                                    newItem.PaymentPlanCheckId = long.Parse(reader["id"].ToString());
                                    newItem.TransactionId = long.Parse(reader["transaction_id"].ToString());
                                    newItem.CheckType = reader[8].ToString();
                                    newItem.Status = reader["status"].ToString();
                                    newItem.AmountRecieved = decimal.Parse(reader["amount_recieved"].ToString());

                                    DateTime dateDue;
                                    if (DateTime.TryParse(reader["date_due"].ToString(), out dateDue))
                                        newItem.DateDue = dateDue;
                                    else
                                        newItem.DateDue = null;

                                    if (!string.IsNullOrEmpty(reader[10].ToString()))
                                        newItem.RoutingNumber = reader[10].ToString();

                                    newItem.Transaction = transRepository.GetById(newItem.TransactionId);

                                    if (newItem.CheckType == "0")
                                    {
                                        if (WyPayoffAmount(newItem.Transaction, queryDate) < newItem.DepositAmount)
                                            newItem.DepositAmount = WyPayoffAmount(newItem.Transaction, queryDate);
                                    }
                                    depositsList.Add(newItem);

                                }
                            }
                        }
                    }
                }
                reader.Close();
            }

            returnList.ACHDeposits = depositsList;
            //returnList.ClosedAccounts = closedAccounts;

            return returnList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deposits"></param>
        /// <param name="depositsTransaction"></param>
        /// <returns></returns>
        public DoDepositsResponse DoDepositsColorado(DepositTransactionEntity depositsTransaction)
        {
            List<DepositEntity> CLVerifyPayment = new List<DepositEntity>();
            List<DepositEntity> CLVerifyClosed = new List<DepositEntity>();
            List<string[]> ReceiptCollection = new List<string[]>();

            decimal total = 0;
            decimal achTotal = 0;

            //repositories used in this procedure
            Repository.Transaction transRepository = new Repository.Transaction(ConnectionString);
            Repository.Client clientRepository = new Repository.Client(ConnectionString);
            Repository.PaymentPlanCheck ppcRepository = new Repository.PaymentPlanCheck(ConnectionString);
            Repository.Payment paymentRepository = new Repository.Payment(ConnectionString);
            Repository.CashLog cashLogRepository = new Repository.CashLog(ConnectionString);
            Repository.Checkbook checkbookRepository = new Repository.Checkbook(ConnectionString);

            //var svcCl = new CLVerify();
            //svcCl.ConnectionString = ConnectionString;
            //svcCl.clverifyURI = clverifyURI;
            //svcCl.clverifyAuthorizeString = clverifyAuthorizeString;

            var svcTeleTrack = new ColoradoTeleTrack();
            
            //object ACHDebitCollection;
            //object DepositsCollection = null;

            foreach (DepositEntity item in depositsTransaction.Deposits)
            {
                if (item.Deposit == true)
                {
                    decimal depositAmount = 0;
                    decimal principle = 0;
                    decimal fee = 0;
                    decimal startingBalance = 0;

                    LoanStopModel.Transaction transaction = transRepository.GetById(item.Transaction.Id);
                    LoanStopModel.Client client = clientRepository.GetBySSNumber(item.Transaction.SsNumber);
                    item.Client = client;

                    Debug.WriteLine("Customer Name : " + client.Firstname + "  "+ client.Lastname);

                    depositAmount = item.DepositAmount;
                    startingBalance = transaction.AmountRecieved;

                    string[] newItem = new string[] { transaction.Name, depositAmount.ToString("C2") };

                    ReceiptCollection.Add(newItem);

                    total = total + depositAmount;

                    if (item.CheckType == "1")
                    {
                        transaction.DateCleared = DateTime.Now.Date;
                        transaction.Status = "Cleared";
                        transRepository.Update(transaction);
                    }
                    else
                    {
                        if (item.DepositAmount < item.AmountRecieved)
                        {
                            principle = transaction.AmountRecieved;
                            fee = decimal.Round(depositAmount - principle, 2);
                        }
                        else
                        {
                            fee = transaction.FinanceCharge / 6;
                            principle = decimal.Round(depositAmount - fee, 2);
                        }

                        if (item.Status == "Partial")
                        {
                            principle = depositAmount;
                            fee = 0;
                        }

                        transaction.AmountRecieved = decimal.Round(transaction.AmountRecieved - principle, 2);
                        transRepository.Update(transaction);

                        //CLVerifyPayment.Add(item);
                        //svcCl.Payment(Store, client, transaction.Id, transaction.AmountRecieved); 
                        //CLVerifyPayment(client, transaction);
                        //svcTeleTrack.PaidInFull(Store, client, transaction.Id, transaction.AmountRecieved);


                        LoanStopModel.PaymentPlanCheck ppc = ppcRepository.GetById(item.PaymentPlanCheckId);

                        if (item.Status == "Partial")
                        {
                            ppc.AmountPaid = decimal.Round(ppc.AmountPaid + depositAmount, 2);
                            ppc.AmountRecieved = depositAmount;
                            ppc.DatePaid = DateTime.Now.Date;
                            ppc.DateCleared = DateTime.Now.Date;
                            ppc.Status = "Cleared";
                            ppcRepository.Update(ppc);
                            // update memory copy when updating database
                            //transaction.PaymentPlanChecks.Update(ppc)
                        }
                        else
                        {
                            ppc.AmountPaid = depositAmount;
                            ppc.AmountRecieved = depositAmount;
                            ppc.DatePaid = DateTime.Now.Date;
                            ppc.DateCleared = DateTime.Now.Date;
                            ppc.Status = "Cleared";
                            ppcRepository.Update(ppc);
                            // update memory copy when updating database
                            //transaction.PaymentPlanChecks.Update(ppc)
                        }

                        if ((startingBalance - principle) < 0.05m)
                        {
                            transaction.Status = "Closed";
                            transaction.DateCleared = DateTime.Now.Date;
                            transRepository.Update(transaction);

                            foreach (PaymentPlanCheck setVoid in transaction.PaymentPlanChecks)
                            {
                                //'set the rest of the scheduled payments to void 
                                if (setVoid.Status == "Pick Up" || setVoid.Status == "Deposit")
                                {
                                    if (ppc.Id != setVoid.Id)
                                    {
                                        setVoid.Status = "Void";
                                        ppcRepository.Update(setVoid);
                                    }
                                }
                            }

                            // this is to add to collection so that closed account leter gets printed.
                            CLVerifyClosed.Add(item);
                            
                            //svcCl.Closed(Store, client, transaction.Id, transaction.AmountRecieved);
                            //CLVerifyClosed.Add(item);
                        }

                        LoanStopModel.Payment payment = new LoanStopModel.Payment();

                        payment.DateDue = null;
                        payment.TransactionId = (int)transaction.Id;
                        payment.SsNumber = transaction.SsNumber;
                        payment.Name = transaction.Name;

                        payment.Balance = principle.ToString("N");
                        payment.Description = "Deposit Payment";
                        payment.DatePaid = DateTime.Now.Date;
                        payment.AmountPaid = fee.ToString("N");
                        payment.AmountDue = depositAmount.ToString("N");
                        payment.PaymentNumber = 0;
                        payment.PaymentType = "Fee Income";

                        paymentRepository.Add(payment);
                    }
                }
            }// end foreach

            if (depositsTransaction.CheckAmount > 0)
            {
                Checkbook checkbook = new Checkbook();

                checkbook.CheckNumber = 0;
                checkbook.TransactionType = "Deposit";
                checkbook.Amount = depositsTransaction.CheckAmount;
                checkbook.Category = "Deposit";
                checkbook.Type = "credit";
                checkbook.Description = "CHECK DEPOSIT";
                checkbook.Employee = " ";
                checkbook.PayableTo = "Deposit";
                checkbook.SsNumber = " ";
                checkbook.TransactionNumber = null;
                checkbook.DateEntered = DateTime.Now.Date;
                checkbook.DateTime = DateTime.Now;

                checkbookRepository.Add(checkbook);
            }

            if (depositsTransaction.ACHAmount > 0)
            {
                Checkbook checkbook = new Checkbook();

                checkbook.CheckNumber = 0;
                checkbook.TransactionType = "Deposit";
                checkbook.Amount = depositsTransaction.ACHAmount;
                checkbook.Category = "Deposit";
                checkbook.Type = "credit";
                checkbook.Description = "ACH DEPOSIT";
                checkbook.Employee = " ";
                checkbook.PayableTo = "Deposit";
                checkbook.SsNumber = " ";
                checkbook.TransactionNumber = null;
                checkbook.DateEntered = DateTime.Now.Date;
                checkbook.DateTime = DateTime.Now;

                checkbookRepository.Add(checkbook);
                achTotal = depositsTransaction.ACHAmount;
            }

            if (depositsTransaction.Cash > 0)
            {
                if (depositsTransaction.Cash > 0)
                {

                    Checkbook checkbook = new Checkbook();

                    checkbook.CheckNumber = 0;
                    checkbook.TransactionType = "Deposit";
                    checkbook.Amount = depositsTransaction.Cash;
                    checkbook.Category = "Deposit";
                    checkbook.Type = "credit";
                    checkbook.Description = "CASH";
                    checkbook.Employee = " ";
                    checkbook.PayableTo = "Deposit";
                    checkbook.SsNumber = " ";
                    checkbook.TransactionNumber = null;
                    checkbook.DateEntered = DateTime.Now.Date;
                    checkbook.DateTime = DateTime.Now;

                    checkbookRepository.Add(checkbook);

                    CashLog cashlog = new CashLog();

                    cashlog.Category = "Revenue";
                    cashlog.TransactionType = "Misc. Deposit";
                    cashlog.Description = "DEPOSIT CASH";
                    cashlog.Amount = -depositsTransaction.Cash;
                    cashlog.Type = "debit";
                    cashlog.PayableTo = " ";
                    cashlog.SsNumber = " ";
                    cashlog.TransactionNumber = 0;
                    cashlog.Date = DateTime.Now.Date;

                    cashLogRepository.Add(cashlog);
                }
            }

            var response = new DoDepositsResponse();

            response.Closed = CLVerifyClosed;
            response.Receipt = ReceiptCollection;
            response.Total = total.ToString("C2");
            response.ACHAmount = achTotal.ToString("C2");

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depositsTransaction"></param>
        /// <returns></returns>
        public DoDepositsResponse DoDepositsWyoming(DepositTransactionEntity depositsTransaction)
        {
            List<DepositEntity> CLVerifyPayment = new List<DepositEntity>();
            List<DepositEntity> CLVerifyClosed = new List<DepositEntity>();
            List<string[]> ReceiptCollection = new List<string[]>();

            decimal total = 0;
            decimal achTotal = 0;

            Repository.Transaction transRepository = new Repository.Transaction(ConnectionString);
            Repository.Client clientRepository = new Repository.Client(ConnectionString);
            Repository.PaymentPlanCheck ppcRepository = new Repository.PaymentPlanCheck(ConnectionString);
            Repository.Payment paymentRepository = new Repository.Payment(ConnectionString);
            Repository.CashLog cashLogRepository = new Repository.CashLog(ConnectionString);
            Repository.Checkbook checkbookRepository = new Repository.Checkbook(ConnectionString);

            //var svc = new CLVerify();
            //svc.ConnectionString = ConnectionString;

            foreach (var deposit in depositsTransaction.Deposits)
            {
                decimal depositAmount = 0;
                decimal principle = 0;
                decimal fee = 0;

                decimal startingBalance;

                LoanStopModel.Transaction transaction = transRepository.GetById(deposit.Transaction.Id);
                LoanStopModel.Client client = deposit.Client;

                depositAmount = deposit.DepositAmount;    
                startingBalance = transaction.AmountRecieved;

                string[] newItem = new string[] { transaction.Name, depositAmount.ToString("C2") };
                ReceiptCollection.Add(newItem);

                total = total + depositAmount;

                if (deposit.CheckType == "1")
                {
                    transaction.DateCleared = DateTime.Now.Date;
                    transaction.Status = "Cleared";
                    transRepository.Update(transaction);
                }
                else
                {

                    LoanStopModel.PaymentPlanCheck ppc = ppcRepository.GetById(deposit.PaymentPlanCheckId);
                    ppc.AmountPaid = decimal.Round(depositAmount, 2);
                    ppc.DatePaid = DateTime.Now.Date;
                    ppc.DateCleared = DateTime.Now.Date;
                    ppc.Status = "Cleared";
                    ppcRepository.Update(ppc);

                    //TODO
                    if ((startingBalance - (transaction.SumAmountPaidPaymentPlaychecks + decimal.Round(depositAmount, 2))) < 0.05m)
                    {
                        transaction.DateCleared = DateTime.Now.Date;
                        transaction.Status = "Closed";
                        transRepository.Update(transaction);
                    }

                    LoanStopModel.Payment payment = new LoanStopModel.Payment();

                    payment.DateDue = null;
                    payment.TransactionId = (int)transaction.Id;
                    payment.SsNumber = transaction.SsNumber;
                    payment.Name = transaction.Name;

                    payment.Balance = depositAmount.ToString("N");
                    payment.Description = "DEPOSITS SERVICE";
                    payment.DatePaid = DateTime.Now.Date;
                    payment.AmountPaid = fee.ToString("N");
                    payment.AmountDue = depositAmount.ToString("N");
                    payment.PaymentNumber = 0;
                    payment.PaymentType = "Fee Income";

                    paymentRepository.Add(payment);
                }
            }

            if (depositsTransaction.CheckAmount > 0) 
            {
                Checkbook checkbook = new Checkbook();

                checkbook.CheckNumber = 0;
                checkbook.TransactionType = "Deposit";
                checkbook.Amount = depositsTransaction.CheckAmount;
                checkbook.Category = "Deposit";
                checkbook.Type = "credit";
                checkbook.Description = "CHECK DEPOSIT";
                checkbook.Employee = " ";
                checkbook.PayableTo = "Deposit";
                checkbook.SsNumber = " ";
                checkbook.TransactionNumber = null;
                checkbook.DateEntered = DateTime.Now.Date;
                checkbook.DateTime = DateTime.Now;

                checkbookRepository.Add(checkbook);
            }

            if (depositsTransaction.ACHAmount > 0) 
            {
                Checkbook checkbook = new Checkbook();

                checkbook.CheckNumber = 0;
                checkbook.TransactionType = "Deposit";
                checkbook.Amount = depositsTransaction.ACHAmount;
                checkbook.Category = "Deposit";
                checkbook.Type = "credit";
                checkbook.Description = "ACH DEPOSIT";
                checkbook.Employee = " ";
                checkbook.PayableTo = "Deposit";
                checkbook.SsNumber = " ";
                checkbook.TransactionNumber = null;
                checkbook.DateEntered = DateTime.Now.Date;
                checkbook.DateTime = DateTime.Now;

                checkbookRepository.Add(checkbook);
                achTotal = depositsTransaction.ACHAmount;
            }

            if (depositsTransaction.Cash > 0) 
            {

                Checkbook checkbook = new Checkbook();

                checkbook.CheckNumber = 0;
                checkbook.TransactionType = "Deposit";
                checkbook.Amount = depositsTransaction.Cash;
                checkbook.Category = "Deposit";
                checkbook.Type = "credit";
                checkbook.Description = "CASH";
                checkbook.Employee = " ";
                checkbook.PayableTo = "Deposit";
                checkbook.SsNumber = " ";
                checkbook.TransactionNumber = null;
                checkbook.DateEntered = DateTime.Now.Date;
                checkbook.DateTime = DateTime.Now;

                checkbookRepository.Add(checkbook);
    
                CashLog cashlog = new CashLog();

                cashlog.Category = "Revenue";
                cashlog.TransactionType = "Misc. Deposit";
                cashlog.Description = "DEPOSIT CASH";
                cashlog.Amount = -depositsTransaction.Cash;
                cashlog.Type = "debit";
                cashlog.PayableTo = " ";
                cashlog.SsNumber = " ";
                cashlog.TransactionNumber = null;
                cashlog.Date = DateTime.Now.Date;

                cashLogRepository.Add(cashlog);
            }

            var response = new DoDepositsResponse();

            response.Closed = CLVerifyClosed;
            response.Receipt = ReceiptCollection;
            response.Total = total.ToString("C2");
            response.ACHAmount = achTotal.ToString("C2");

            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private long GetDifferenceInMonths(DateTime startDate, DateTime endDate)
        {
            long rtrnNumb = 0;
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                switch (currentDate.Month)
                {
                    case 1:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 2:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 3:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 4:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 5:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 6:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 7:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 8:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 9:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 10:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 11:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                    case 12:
                        currentDate = currentDate.AddMonths(1);
                        if (currentDate <= endDate) rtrnNumb = rtrnNumb + 1;
                        break;
                }
            }

            return rtrnNumb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TheDate"></param>
        /// <returns></returns>
        public decimal CoPayoffAmount(Transaction transaction, DateTime TheDate)
        {
            decimal Balance = 0;
            decimal TotalPayments = 0;
            decimal OrignationFee;
            decimal TotalFee = 0;
            decimal TotalOrignalPayments = 0;

            DateTime transDate = (DateTime)transaction.TransDate;
            TimeSpan ts = transDate - TheDate;

            if (ts.Days >= -1)
                Balance = transaction.AmountDispursed;
            else
            {
                if (transaction.AmountDispursed <= 300)
                    OrignationFee = (transaction.AmountDispursed * 0.2m);
                else
                    OrignationFee = (300m * 0.2m) + ((transaction.AmountDispursed - 300m) * 0.075m);

                foreach (var row in transaction.PaymentPlanChecks)
                {
                    TotalOrignalPayments = TotalOrignalPayments + decimal.Parse(row.OrignalAmount) + row.OtherFee; ;

                    TotalPayments = TotalPayments + row.AmountPaid;

                    TotalFee = TotalFee + row.OtherFee;
                }

                decimal ServiceFee = Math.Truncate(transaction.AmountDispursed / 100m) * 7.5m;
                decimal AmountFinanced = transaction.AmountDispursed;

                if (ServiceFee > 30) ServiceFee = 30;

                double Interest = (Financial.Pmt((0.45 / 12), 6, (double)AmountFinanced) * -6D) - (double)AmountFinanced;

                DateTime StartingDate = ((DateTime)transaction.TransDate).AddDays(30);

                long DaysOutstanding = (long)Math.Truncate(((TimeSpan)((DateTime)transaction.TransDate - TheDate)).TotalDays);
                long DaysOfLoan = (long)Math.Truncate(((TimeSpan)((DateTime)transaction.TransDate - transaction.FinalPayoffPaymentDay)).TotalDays);

                long differenceInMonths = GetDifferenceInMonths(StartingDate, TheDate);

                decimal ServiceFeeEarned = differenceInMonths * ServiceFee;

                OrignationFee = (decimal)(OrignationFee / DaysOfLoan) * DaysOutstanding;

                if (AmountFinanced <= 300m)
                {
                    if (((DateTime)transaction.TransDate - TheDate).TotalDays <= 31)
                        Balance = AmountFinanced + OrignationFee + ((decimal)(Interest / DaysOfLoan) * DaysOutstanding + ServiceFeeEarned) - TotalPayments + TotalFee;
                    else
                        Balance = AmountFinanced + OrignationFee + ((decimal)(Interest / DaysOfLoan) * DaysOutstanding + ServiceFeeEarned) - TotalPayments + TotalFee;
                }
                else
                    Balance = AmountFinanced + OrignationFee + ((decimal)(Interest / DaysOfLoan) * DaysOutstanding + ServiceFeeEarned) - TotalPayments + TotalFee;

                if (Balance > TotalOrignalPayments - TotalPayments)
                    Balance = TotalOrignalPayments - TotalPayments;

            }

            if (transaction.Status == "Closed") Balance = 0;

            return Math.Round(Balance,2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="theDate"></param>
        /// <returns></returns>
        private decimal WyPayoffAmount(Transaction transaction, DateTime theDate)
        {
            decimal interest = 0;
            decimal balance = 0;
            decimal totalPayments = 0;
            decimal orignationFee = 0;
            decimal totalFee = 0;
            decimal totalOrignalPayments = 0;

            if (DateTime.Compare((DateTime)transaction.TransDate, theDate) <= 1)
                balance = transaction.AmountDispursed;
            else
            {
                if (transaction.AmountDispursed <= 300)
                    orignationFee = transaction.AmountDispursed * 0.2m;
                else 
                    orignationFee = (300m * 0.2m) + ((transaction.AmountDispursed - 300) * 0.075m);

                foreach(var checkItem in transaction.PaymentPlanChecks)
                {
                    if (!string.IsNullOrEmpty(checkItem.OrignalAmount))
                        totalOrignalPayments = totalOrignalPayments + decimal.Parse(checkItem.OrignalAmount);

                    if (checkItem.AmountPaid > 0m)
                        totalPayments = totalPayments + checkItem.AmountPaid;

                    if (checkItem.OtherFee > 0m)
                        totalFee = totalFee + checkItem.OtherFee;
                }

                //int daysOutstanding;
                //int daysOfLoan;
                //decimal serviceFeeEarned;
                //decimal serviceFee = 30;
                decimal differenceINMonths;

                decimal amountFinanced = amountFinanced = transaction.AmountDispursed;
                DateTime startingDate = ((DateTime)transaction.TransDate).AddDays(30);

                differenceINMonths = GetDifferenceInMonths(startingDate, theDate);

                //orignationFee = (orignationFee / daysOfLoan) * daysOutstanding;

                if(amountFinanced <=300)
                {
                    if (((DateTime)transaction.TransDate - theDate).TotalDays <= 31)
                        balance = amountFinanced + orignationFee - totalPayments + totalFee;
                    else
                        balance = amountFinanced + orignationFee - totalPayments + totalFee;
                }
                else
                    balance = amountFinanced + orignationFee - totalPayments + totalFee;

                if (balance > totalOrignalPayments - totalPayments)
                    balance = totalOrignalPayments - totalPayments;

            }

            if (transaction.Status == "Closed") balance = 0;

            return balance;
        }



    }
}