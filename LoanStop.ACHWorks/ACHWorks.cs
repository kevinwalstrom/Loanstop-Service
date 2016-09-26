using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

using PlainElastic.Net;
using Newtonsoft.Json;
using LoanStop.Entities.logsene;
//using LoanStop.ACHWorks.Proxy;
using LoanStop.ACHWorks.Types;
using LoanStop.Entities.ACHWorks;
using LoanStopModel;
using LoanStop.Entities.CommonTypes;
using LoanStop.ACHWorks.com.achworks.securesoap;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.DBCore;

namespace LoanStop.ACHWorks
{
    public class ACHWorksClass
    {
        private DefaultType defaults;

        public ACHWorksClass(DefaultType _defaults)
        {
            this.defaults = _defaults;

        }
        
        /// <summary>
        /// 
        /// </summary>
        public void TSSConnectionCheck()
        {
            //var proxy = new IverClient();

            //var response = proxy.ConnectionCheck(this.defaults.ACHWorksLocId, this.defaults.ACHWorksCompanyId, this.defaults.ACHWorksCompanyKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public VerifyResponse TSSVerify(Client client, string amount)
        {

            var proxy = new Iverservice();

            string InpServiceCode = "1000";

            CustomerInfo InpCustomerInfo = new CustomerInfo()
                {
                    CustomerName = client.Firstname + " " + client.Lastname,
                    DateOfBirth = ((AuxClient)client.AuxClient).Dob,
                    SSN = client.SsNumber
                };

            CompanyInfo InpCompanyInfo = new CompanyInfo()
                {
                    Company = this.defaults.ACHWorksCompanyId,
                    CompanyKey = this.defaults.ACHWorksCompanyKey,
                    Email = "egraning@loanstop.com",
                    LocID = this.defaults.ACHWorksLocId
                };

            AcctInfo InpAcctInfo = new AcctInfo()
                {
                    AcctNo = client.BankAccount,
                    AcctType = "C",
                    Amount = amount,
                    RoutingNo = ((AuxClient)client.AuxClient).RoutingNumber
                };
            
            bool valid = false;
            string message = null;
            string code = null;
            try
            {
                var response = proxy.TSSVerify(InpServiceCode, InpCompanyInfo, InpCustomerInfo, InpAcctInfo);

                var start = response.CKVXML.IndexOf("<code>");

                var end = response.CKVXML.LastIndexOf("</code>");

                code = response.CKVXML.Substring(start + 6, end - start - 6);

                var messageStart = response.CKVXML.IndexOf("<description>");

                var messageEnd = response.CKVXML.LastIndexOf("</description>");

                message = response.CKVXML.Substring(messageStart + 13, messageEnd - messageStart - 13);

                valid = checkCode(code);

                var item = new CustomerInquirie
                {
                    Name = client.Firstname + " " + client.Lastname,
                    Store = defaults.StoreCode,
                    Code = code,
                    DateEntered = DateTime.Now,
                    Message = message
                };

                logAction(defaults.Store, "verify", item);

                tracking(item);

            }
            catch (Exception ex)
            {
                logException(defaults.Store, "verify", ex);
            }

            return new VerifyResponse() { Error = false, Status = valid, Code = code, VerifyMessage = message };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private bool checkCode(string code)
        {
            bool returValue = false;

            switch (code)
            {
                case "XP0":
                    break;
                case "XP1":
                    break;
                case "XP2":
                    break;
                case "R01":
                    break;
                case "XP7":
                    break;
                case "X30":
                    break;
                case "XP5":
                    break;
                case "AP5":
                    break;
                case "AP7": // LOW RISK - (Level B validation). Account is open and valid onContributor&#039;s file with a positive balance
                    returValue = true;
                    break;
                case "A71": // LOW RISK - (Level A validation). Account was open and valid the last time a transaction was seen for this account. No status about balance for thisaccount exists
                    returValue = true;
                    break;
                case "A72": // LOW RISK - (Level A validation). Account is open and valid and currently has sufficient funds for the amount submitted with the query. Account hasgood funding status.
                    returValue = true;
                    break;
                case "A73": // MEDIUM RISK - (Level B validation). Account has a high risk response but is open and valid with a positive balance
                    returValue = true;
                    break;
                case "XU1":
                    break;
                case "XU2":
                    break;
                case "XU3":
                    break;
                case "XB4":
                    break;
                case "XU5":
                    break;
                case "XU6":
                    break;
                case "A01":
                    returValue = true;
                    break;
                case "U80": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U81": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U82": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U85": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U88": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U90": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U91": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "U92": // (Level D validation). Transaction could not be verified because theVerification Service Provider Server is Busy or Unavailable. Cu
                    returValue = true;
                    break;
                case "V15": // HIGH RISK - (Level B validation). Account is open and has Hard NSF status.
                    returValue = false;
                    break;
                case "V40": // UNKNOWN RISK - (Level B validation). Transaction could not be verified because there is no negative information available from any participatingdatabases. Account is a non-DD
                    returValue = false;
                    break;
                case "V41": // HIGH RISK - (Level B validation). Account has negative status andAccount is presumed closed.
                    returValue = false;
                    break;
                case "V50": // UNKNOWN RISK - (Level B validation). Transaction could not be verifiedbecause there is no information available from any participatingdatabases. Account cannot be declined due to lack of information.Account has unknown status.
                    returValue = false;
                    break;
                case "XF1":
                    returValue = true;
                    break;
                case "XF3":
                    returValue = true;
                    break;
                case "XF4":
                    returValue = true;
                    break;
                case "XF5":
                    returValue = true;
                    break;
                case "XF7":
                    returValue = true;
                    break;
                case "X80":
                    returValue = true;
                    break;
                case "X81":
                    returValue = true;
                    break;
                case "X90":
                    returValue = true;
                    break;
                case "X91":
                    returValue = true;
                    break;
                case "X92":
                    returValue = true;
                    break;
                case "X93":
                    returValue = true;
                    break;
                case "XP8":
                    returValue = true;
                    break;
                case "X11":
                    returValue = false;
                    break;
                case "X12":
                    returValue = false;
                    break;
                case "X20":
                    returValue = false;
                    break;
                case "X21": // (Level D validation). Transaction could not be verified, not applicable.Customer account status cannot be determined.
                    returValue = true;
                    break;
                case "NA": // LOW RISK - (Level C validation). There are No Negative Reports in the experiential database containing &gt; 158 million accounts. Customeraccount has good status.
                    returValue = true;
                    break;
            }

            return returValue;
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        private void tracking(CustomerInquirie item)
        {
            var rep = new Repository.Tracking();

            item.Message = item.Message.Substring(0, 44);

            var result = rep.SaveCustomerInquirie(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="action"></param>
        /// <param name="model"></param>
        private void logAction(string store, string action, object model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}{1}", "i", action);
            string jsonData = JsonConvert.SerializeObject(model);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = store;
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch
            { }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="action"></param>
        /// <param name="model"></param>
        private void logException(string store, string action, Exception model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}-{1}", "ERROR", action);
            string jsonData = JsonConvert.SerializeObject(model.Message);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = store;
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch
            { }
        }

    }
}
