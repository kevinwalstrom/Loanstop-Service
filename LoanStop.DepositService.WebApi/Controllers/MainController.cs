using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.Practices.Unity;
using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Entities.CommonTypes;
using LoanStop.Entities.Transaction;
using LoanStop.Entities.logsene;
using LoanStopModel;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business;
using Repository = LoanStop.DBCore.Repository;
using Wyoming = LoanStop.Services.WebApi.Business.Wyoming;
using Colorado = LoanStop.Services.WebApi.Business.Colorado;

namespace LoanStop.Services.WebApi.Controllers
{
    public class MainController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }


        [HttpGet]
        [Route("api/main/{state}/{store}/{value}")]
        public ResponseType Get(string state, string store, string value)
        {
           
            Client client = null;
            bool? paymentPlanEnabled = null;
            long id;

            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var clientRepository = new Repository.Client(connectionString);

            if (long.TryParse(value, out id))
            {
                client = clientRepository.GetByClientId(id);
            }
            else
            {
                client = clientRepository.GetBySSNumber(value);
            }

            var transRepository = new Repository.Transaction(connectionString);
            var transactionList = transRepository.GetBySSNumber(client.SsNumber);

            var notes = clientRepository.GetNotes(client.SsNumber);

            var consecutive = Consecutive.DetermineConsecutive(transactionList);
            var aging = Aging.DetermineAging(transactionList);
            var daysLeftForAgingStatus = Aging.DaysLeftForAgingStatus(transactionList);
            
            var isInfoUpdate = client.Status == "0" ? true : false;

            IIsNewLoanEnabled objNewLoanEnabled = null;

            if (state.ToUpper() == "COLORADO")
            {
                objNewLoanEnabled = new Colorado.IsNewLoanEnabled(connectionString, defaults);
            }
            else if (state.ToUpper() == "WYOMING")
            {
                objNewLoanEnabled = new Wyoming.IsNewLoanEnabled(connectionString, defaults);
            }

            bool isNewLoanEnabled = false;

            try 
            { 
                isNewLoanEnabled = objNewLoanEnabled.Execute(client, DateTime.Now, transactionList.ToList());
            }
            catch (Exception exLoan)
            {
                var message = exLoan.Message;
            }

            decimal sumAmountDispursed = transRepository.CustomerAmountDisbursed(client.SsNumber);

            if (state.ToUpper() == "WYOMING")
            {
                var wyoming = new Wyoming.PaymentPlan();
                wyoming.ConnectionString = connectionString;
                var result = wyoming.IsPaymentPlanEnabled(transactionList);
                paymentPlanEnabled = result;
            }
            else
            {
                paymentPlanEnabled = true;
            }

            response.Item = new {
                PaymentPlanEnabled = paymentPlanEnabled,
                Aging = aging,
                Consecutive = consecutive,
                IsInfoUpdate = isInfoUpdate, 
                IsNewLoanEnabled = isNewLoanEnabled, 
                SumAmountDispursed = sumAmountDispursed, 
                AgingDaysLeft = daysLeftForAgingStatus,
                Client = client,
                Transactions = transactionList,
                Notes = notes
            };
            
            return response;

        }

        [HttpPost]
        [Route("api/main/{state}/{store}/UpdateNote")]
        public ResponseType UpdateNote(string state, string store, UpdateNoteModel value)
        {
           
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var clientRepository = new Repository.Client(connectionString);
            
            clientRepository.UpdateNotes(value.SsNumber, value.Note);

            response.Item = new {};
            
            return response;

        }

        [HttpPost]
        [Route("api/main/postlog")]
        public void PostLog(object model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}", "test");
            string jsonData = JsonConvert.SerializeObject(model);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = "TEST";
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = "TEST";

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }

        }


    }
}
