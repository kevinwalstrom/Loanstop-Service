using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoanStopModel;
using LoanStop.TeleTrackClient.Proxy;
using LoanStop.TeleTrackClient.ReportingProxy;
using LoanStop.Entities.TeleTrack;
using LoanStop.Entities.logsene;
using PlainElastic.Net;
using Newtonsoft.Json;
using System.Configuration;

namespace LoanStop.TeleTrackClient
{
    public class ColoradoTeleTrack : ITeleTrack
    {

        public string UserName;
        public string SubscriberID;
        public string Password;
        public int Score;
        public string Facility;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public InquiryResponse Inquiry(Client client, AuxClient auxClient)
        {
            UserName = string.IsNullOrEmpty(UserName) ? "LS-DNVR" : UserName;
            Password = string.IsNullOrEmpty(Password) ? "88280328-e" : Password;
            SubscriberID = string.IsNullOrEmpty(SubscriberID) ? "50763" : SubscriberID;
            Score = Score > 0 ? Score : 480;

            TransactionRequestEntity requestEntity = new TransactionRequestEntity();

            requestEntity.TeletrackXMLVersion = "1.2.6";

            //subscriber
            Proxy.SubscriberEntity subscriberEntity = new Proxy.SubscriberEntity();
            subscriberEntity.UserName = UserName;
            subscriberEntity.SubscriberID = SubscriberID;
            requestEntity.Subscriber = subscriberEntity;

            //request details
            
            RequestEntity requestInquiry = new RequestEntity();
            requestInquiry.RequestType = RequestEntityRequestType.Inquiry;
            RequestEntity requestScore = new RequestEntity();
            requestScore.RequestType = RequestEntityRequestType.Score;
            requestEntity.RequestDetails = new RequestEntity[] { requestInquiry, requestScore };
           
                

            //customer information
            requestEntity.Applicant = new Proxy.ConsumerEntity();
            requestEntity.Applicant.Uin = new Proxy.UINEntity();
            requestEntity.Applicant.Uin.Type = "SSN";
            requestEntity.Applicant.Uin.Uin = client.SsNumber.Replace("-","");
            requestEntity.Applicant.Name = new Proxy.NameEntity();
            requestEntity.Applicant.Name.FirstName = client.Firstname;
            requestEntity.Applicant.Name.LastName = client.Lastname;
            requestEntity.Applicant.Name.MiddleName = client.Mi;
            requestEntity.Applicant.Address = new Proxy.AddressEntity();
            requestEntity.Applicant.Address.SimpleAddress = new Proxy.SimpleAddressEntity();
            requestEntity.Applicant.Address.SimpleAddress.Line1 = client.Address;
            requestEntity.Applicant.Address.SimpleAddress.City = client.City;
            requestEntity.Applicant.Address.SimpleAddress.State = client.State;
            requestEntity.Applicant.Address.SimpleAddress.PostalCode = client.Zip;
            requestEntity.Applicant.BirthDate = DateTime.Parse(auxClient.Dob);
            requestEntity.Applicant.DriversLicense = new DriversLicenseEntity();
            requestEntity.Applicant.DriversLicense.Number = client.DriverLicense;
            requestEntity.Applicant.DriversLicense.State = client.DriverLicense;
            var cellPhone = new Proxy.PhoneEntity();
            cellPhone.Number = auxClient.CellPhone;
            cellPhone.Type = Proxy.PhoneTypeCodeType.Mobile;
            var workPhone = new Proxy.PhoneEntity();
            workPhone.Number = client.WorkPhone;
            workPhone.Type = Proxy.PhoneTypeCodeType.Work;
            var homePhone = new Proxy.PhoneEntity();
            homePhone.Number = client.HomePhone;
            homePhone.Type = Proxy.PhoneTypeCodeType.Home;
            requestEntity.Applicant.Phones = new Proxy.PhoneEntity[] { cellPhone, workPhone, homePhone };

            //employmennt information
            EmployerEntity employerEntity = new EmployerEntity();
            employerEntity.EmployerName = client.Employer;
            employerEntity.Occupation = auxClient != null ? auxClient.Occupation : "unknown";
            IncomeEntity incomeEntity = new IncomeEntity();
            incomeEntity.Amount = auxClient != null ? decimal.Parse(auxClient.NetFromPaystubs) : 1000m;
            incomeEntity.PaymentFrequency = Proxy.PaymentFrequencyType.Biweekly;
            employerEntity.Salary = incomeEntity;

            requestEntity.Applicant.Employer = employerEntity;

            InquirySoapClient teleTrack = new InquirySoapClient();


            teleTrack.ClientCredentials.UserName.UserName = UserName;
            teleTrack.ClientCredentials.UserName.Password = Password;

            string env = string.Empty;
#if DEBUG
            env = "-DEBUG";
#endif

            var verify = ConfigurationManager.AppSettings["Verify.Customer"];
            Proxy.TransactionResponseEntity response = null;
            if (verify == "TELETRACK")
            {
                try
                {
                    response = teleTrack.GetData(requestEntity);
                }
                catch (Exception ex)
                {
                   
                    var connection = new ElasticConnection();
                    string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/teletrack", env);
                    string jsonData = JsonConvert.SerializeObject(ex.Message);
                    jsonData = jsonData + JsonConvert.SerializeObject(requestEntity);
                    LogSeneEntity logSeneEntity = new LogSeneEntity();
                    logSeneEntity.facility = Facility;
                    logSeneEntity.timestamp = DateTime.Now;
                    logSeneEntity.message = jsonData;

                    string post = JsonConvert.SerializeObject(logSeneEntity);

                    try
                    {
                        var logsene = connection.Post(command, post);
                    }
                    catch { }

                }
            }

            if (response != null)
            {
                
                var connection = new ElasticConnection();
                string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/teletrack",env);
                string jsonData = JsonConvert.SerializeObject(response);
                LogSeneEntity logSeneEntity = new LogSeneEntity();
                logSeneEntity.facility = Facility;
                logSeneEntity.timestamp = DateTime.Now;
                logSeneEntity.message = jsonData;

                string post = JsonConvert.SerializeObject(logSeneEntity);

                try
                {
                    var logsene = connection.Post(command, post);
                }
                catch { }
            }

            var inquiryResponse = new InquiryResponse();
            if (response!= null && response.TransactionErrors == null)
            {
                inquiryResponse.Error = false;
                inquiryResponse.TransactionCode = response.ConsumerCreditReport.TransactionCode;
                inquiryResponse.TeleTrackScore = response.ConsumerCreditReport.Scores.TeletrackScore.Score;
            }
            else
            {
                inquiryResponse.TransactionCode = "TESTA";
                inquiryResponse.TeleTrackScore = 520;
                inquiryResponse.Error = false;
            }
            
            return inquiryResponse;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ReportResponse IssueLoan(Client client, Transaction transaction, PaymentPlanCheck ppc, string store)
        {
            UserName = string.IsNullOrEmpty(UserName) ? "LS-DNVR" : UserName;
            Password = string.IsNullOrEmpty(Password) ? "88280328-e" : Password;
            SubscriberID = string.IsNullOrEmpty(SubscriberID) ? "50763" : SubscriberID;
 
            var reportEntity = new ReportingProxy.TransactionSetEntity();

            //subscriber
            ReportingProxy.SubscriberEntity subscriberEntity = new ReportingProxy.SubscriberEntity();
            subscriberEntity.UserName = UserName;
            subscriberEntity.SubscriberID = SubscriberID;
            reportEntity.Subscriber = subscriberEntity;

            var transactionReport = new TransactionReportEntity();
            //customer information
            var consumer = new ReportingProxy.ConsumerEntity();
            consumer.Name = new ReportingProxy.NameEntity();
            consumer.Name.FirstName = client.Firstname;
            consumer.Name.LastName = client.Lastname;
            consumer.Name.MiddleName = client.Mi;
            consumer.Address = new ReportingProxy.AddressEntity();
            consumer.Address.SimpleAddress = new ReportingProxy.SimpleAddressEntity();
            consumer.Address.SimpleAddress.Line1 = client.Address;
            consumer.Address.SimpleAddress.City = client.City;
            consumer.Address.SimpleAddress.State = client.State;
            consumer.Address.SimpleAddress.PostalCode = client.Zip;
            consumer.BirthDate = DateTime.Parse(((AuxClient)client.AuxClient).Dob);
            var cellPhone = new ReportingProxy.PhoneEntity();
            cellPhone.Number = ((AuxClient)client.AuxClient).CellPhone;
            cellPhone.Type = ReportingProxy.PhoneTypeCodeType.Mobile;
            consumer.Phone = cellPhone;
            transactionReport.Consumer = consumer;

            var loanEntity = new LoanEntity();

            loanEntity.LoanType = ReportingProxy.LoanTypeType.Installment;
            loanEntity.OpenDatetime = (DateTime)transaction.TransDate;
            loanEntity.OriginalBalance = transaction.AmountDispursed;
            loanEntity.OriginalBalanceSpecified = true;
            loanEntity.DueDate = (DateTime)ppc.DateDue;
            loanEntity.CurrentBalance = transaction.AmountRecieved;
            loanEntity.PaymentFrequency = ReportingProxy.PaymentFrequencyType.Monthly;
            loanEntity.AccountStatus = ReportingProxy.AccountStatusType.Open;
            loanEntity.TransactionCode = client.InquireCode;
            loanEntity.AccountNumber = transaction.Id.ToString();
            loanEntity.PaymentAmount = ppc.AmountRecieved;
            loanEntity.PaymentAmountSpecified = true;
            loanEntity.TransactionDatetime = DateTime.Now;
            loanEntity.DelinquencyStatus = "Current";
            loanEntity.ECOAIndicator = "I";
            loanEntity.DateReported = DateTime.Now;

            transactionReport.Loan = loanEntity;

            reportEntity.TransactionReport = new TransactionReportEntity[] { transactionReport };

            report(reportEntity);

            var response = new ReportResponse();
            response.Error = false;

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="transaction"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public ReportResponse BouncedInstallment(Client client, Transaction transaction, PaymentPlanCheck ppc, string store)
        {

            UserName = string.IsNullOrEmpty(UserName) ? "LS-DNVR" : UserName;
            Password = string.IsNullOrEmpty(Password) ? "88280328-e" : Password;
            SubscriberID = string.IsNullOrEmpty(SubscriberID) ? "50763" : SubscriberID;
 
            var reportEntity = new ReportingProxy.TransactionSetEntity();

            //subscriber
            ReportingProxy.SubscriberEntity subscriberEntity = new ReportingProxy.SubscriberEntity();
            subscriberEntity.UserName = UserName;
            subscriberEntity.SubscriberID = SubscriberID;
            reportEntity.Subscriber = subscriberEntity;


            var transactionReport = new TransactionReportEntity();
            //customer information
            var consumer = new ReportingProxy.ConsumerEntity();
            consumer.Name = new ReportingProxy.NameEntity();
            consumer.Name.FirstName = client.Firstname;
            consumer.Name.LastName = client.Lastname;
            consumer.Name.MiddleName = client.Mi;
            consumer.Address = new ReportingProxy.AddressEntity();
            consumer.Address.SimpleAddress = new ReportingProxy.SimpleAddressEntity();
            consumer.Address.SimpleAddress.Line1 = client.Address;
            consumer.Address.SimpleAddress.City = client.City;
            consumer.Address.SimpleAddress.State = client.State;
            consumer.Address.SimpleAddress.PostalCode = client.Zip;
            consumer.BirthDate = DateTime.Parse(((AuxClient)client.AuxClient).Dob);
            var cellPhone = new ReportingProxy.PhoneEntity();
            cellPhone.Number = ((AuxClient)client.AuxClient).CellPhone;
            cellPhone.Type = ReportingProxy.PhoneTypeCodeType.Mobile;
            consumer.Phone = cellPhone;
            transactionReport.Consumer = consumer;

            var loanEntity = new LoanEntity();

            loanEntity.LoanType = ReportingProxy.LoanTypeType.Installment;
            loanEntity.OpenDatetime = (DateTime)transaction.TransDate;
            loanEntity.OriginalBalance = transaction.AmountDispursed;
            loanEntity.OriginalBalanceSpecified = true;
            loanEntity.DueDate = (DateTime)ppc.DateDue;
            loanEntity.CurrentBalance = transaction.AmountRecieved;
            loanEntity.PaymentFrequency = ReportingProxy.PaymentFrequencyType.Monthly;
            loanEntity.AccountStatus = ReportingProxy.AccountStatusType.NSF;
            loanEntity.TransactionCode = client.InquireCode;
            loanEntity.AccountNumber = transaction.Id.ToString();
            loanEntity.PaymentAmount = ppc.AmountRecieved;
            loanEntity.PaymentAmountSpecified = true;
            loanEntity.TransactionDatetime = DateTime.Now;
            loanEntity.DelinquencyStatus = "Current";
            loanEntity.DateOfFirstDelinquency = (DateTime)ppc.DateReturned;
            loanEntity.ECOAIndicator = "I";
            loanEntity.DateReported = DateTime.Now;

            transactionReport.Loan = loanEntity;

            reportEntity.TransactionReport = new TransactionReportEntity[] { transactionReport };

            report(reportEntity);

            var response = new ReportResponse();
            response.Error = false;

            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ReportResponse PaidInFull(Client client, Transaction transaction, string store)
        {

            var reportEntity = new ReportingProxy.TransactionSetEntity();

            //subscriber
            ReportingProxy.SubscriberEntity subscriberEntity = new ReportingProxy.SubscriberEntity();
            subscriberEntity.UserName = UserName;
            subscriberEntity.SubscriberID = SubscriberID;
            reportEntity.Subscriber = subscriberEntity;


            var transactionReport = new TransactionReportEntity();
            //customer information
            var consumer = new ReportingProxy.ConsumerEntity();
            consumer.Name = new ReportingProxy.NameEntity();
            consumer.Name.FirstName = client.Firstname;
            consumer.Name.LastName = client.Lastname;
            consumer.Name.MiddleName = client.Mi;
            consumer.Address = new ReportingProxy.AddressEntity();
            consumer.Address.SimpleAddress = new ReportingProxy.SimpleAddressEntity();
            consumer.Address.SimpleAddress.Line1 = client.Address;
            consumer.Address.SimpleAddress.City = client.City;
            consumer.Address.SimpleAddress.State = client.State;
            consumer.Address.SimpleAddress.PostalCode = client.Zip;
            consumer.BirthDate = DateTime.Parse(((AuxClient)client.AuxClient).Dob);
            var cellPhone = new ReportingProxy.PhoneEntity();
            cellPhone.Number = ((AuxClient)client.AuxClient).CellPhone;
            cellPhone.Type = ReportingProxy.PhoneTypeCodeType.Mobile;
            consumer.Phone = cellPhone;
            transactionReport.Consumer = consumer;

            var loanEntity = new LoanEntity();

            loanEntity.LoanType = ReportingProxy.LoanTypeType.Installment;
            loanEntity.OpenDatetime = (DateTime)transaction.TransDate;
            loanEntity.OriginalBalance = transaction.AmountDispursed;
            loanEntity.DueDate = (DateTime)transaction.DateDue;
            loanEntity.CurrentBalance = transaction.AmountRecieved;
            loanEntity.PaymentFrequency = ReportingProxy.PaymentFrequencyType.Monthly;
            loanEntity.AccountStatus = ReportingProxy.AccountStatusType.Paid;
            loanEntity.TransactionCode = store;
            loanEntity.AccountNumber = client.SsNumber;
            loanEntity.TransactionDatetime = DateTime.Now;
            loanEntity.DelinquencyStatus = "Current";
            loanEntity.ECOAIndicator = "I";
            loanEntity.DateReported = DateTime.Now;

            transactionReport.Loan = loanEntity;

            reportEntity.TransactionReport = new TransactionReportEntity[] { transactionReport };

            report(reportEntity);

            var response = new ReportResponse();
            response.Error = false;

            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public ReportResponse ChargeOff(Client client, Transaction transaction, string store)
        {
            var reportEntity = new ReportingProxy.TransactionSetEntity();

            //subscriber
            ReportingProxy.SubscriberEntity subscriberEntity = new ReportingProxy.SubscriberEntity();
            subscriberEntity.UserName = UserName;
            subscriberEntity.SubscriberID = SubscriberID;
            reportEntity.Subscriber = subscriberEntity;


            var transactionReport = new TransactionReportEntity();
            //customer information
            var consumer = new ReportingProxy.ConsumerEntity();
            consumer.Name = new ReportingProxy.NameEntity();
            consumer.Name.FirstName = client.Firstname;
            consumer.Name.LastName = client.Lastname;
            consumer.Name.MiddleName = client.Mi;
            consumer.Address = new ReportingProxy.AddressEntity();
            consumer.Address.SimpleAddress = new ReportingProxy.SimpleAddressEntity();
            consumer.Address.SimpleAddress.Line1 = client.Address;
            consumer.Address.SimpleAddress.City = client.City;
            consumer.Address.SimpleAddress.State = client.State;
            consumer.Address.SimpleAddress.PostalCode = client.Zip;
            consumer.BirthDate = DateTime.Parse(((AuxClient)client.AuxClient).Dob);
            var cellPhone = new ReportingProxy.PhoneEntity();
            cellPhone.Number = ((AuxClient)client.AuxClient).CellPhone;
            cellPhone.Type = ReportingProxy.PhoneTypeCodeType.Mobile;
            consumer.Phone = cellPhone;
            transactionReport.Consumer = consumer;

            var loanEntity = new LoanEntity();

            loanEntity.LoanType = ReportingProxy.LoanTypeType.Installment;
            loanEntity.OpenDatetime = (DateTime)transaction.TransDate;
            loanEntity.OriginalBalance = transaction.AmountDispursed;
            loanEntity.DueDate = (DateTime)transaction.DateDue;
            loanEntity.CurrentBalance = transaction.AmountRecieved;
            loanEntity.PaymentFrequency = ReportingProxy.PaymentFrequencyType.Monthly;
            loanEntity.AccountStatus = ReportingProxy.AccountStatusType.Chargeoff;
            loanEntity.TransactionCode = client.InquireCode; 
            loanEntity.AccountNumber = client.SsNumber;
            loanEntity.TransactionDatetime = DateTime.Now;
            loanEntity.DelinquencyStatus = "Current";
            loanEntity.ECOAIndicator = "I";
            loanEntity.DateReported = DateTime.Now;

            transactionReport.Loan = loanEntity;

            reportEntity.TransactionReport = new TransactionReportEntity[] { transactionReport };

            report(reportEntity);

            var response = new ReportResponse();
            response.Error = false;

            return response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportEntity"></param>
        private void report(TransactionSetEntity reportEntity)
        {
            TransactionResponseSetEntity response;

            var verify = ConfigurationManager.AppSettings["Verify.Customer"];

            var teleTrack = new ReportingSoapClient();

            if (verify == "TELETRACK")
            {
                teleTrack.ClientCredentials.UserName.UserName = UserName;
                teleTrack.ClientCredentials.UserName.Password = Password;

                response = teleTrack.ReportData(reportEntity);
            }
            else
            {

            }

        }

        private decimal PaymentAmount(decimal loanamount)
        {
                if (loanamount == 100)
                {
                    return (decimal)(171.02 / 6);
                }
                if (loanamount == 200)
                {
                    return (decimal)(342.04  / 6);
                }
                if (loanamount == 300)
                {
                    return (decimal)(513.06 / 6);
                }
                if (loanamount == 400)
                {
                    return (decimal)(671.58 / 6);
                }
                if (loanamount == 500)
                {
                    return (decimal)(792.66 / 6);
                }

                return 0;
            }
        
    }
}
