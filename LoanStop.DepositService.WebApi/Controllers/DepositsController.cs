using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

using Microsoft.Practices.Unity;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PlainElastic.Net;

using LoanStop.Entities.logsene;
using LoanStop.DepositService.DataContracts;
using LoanStop.DepositService.Core;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Models;

using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Controllers
{
    public class DepositsController : ApiController
    {

        //private string clverifyURI = ConfigurationManager.AppSettings["ClVerifyURI"];
        //private string clverifyAuthorizeString = ConfigurationManager.AppSettings["ClVerifyAuthroize"];

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        // GET api/deposits/state/store/date
        [HttpGet]
        [Route("api/alldeposits/{state}/{store}/{queryDate}")]
        public GetDepositsResponse Get(string state, string store, DateTime queryDate)
        {

            GetDepositsResponse returnResponse = null;

            try
            {
                Deposits deposits = new Deposits();

                var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();
                var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

                deposits.ConnectionString = connectionString;
                deposits.Store = store;

                if (deposits.ConnectionString != null)
                {
                    if (state.ToLower() == "colorado")
                        returnResponse = deposits.GetDepositsColorado(queryDate);
                    else if (state.ToLower() == "wyoming")
                        returnResponse = deposits.GetDepositsWyoming(queryDate);
                    else
                        returnResponse = null;
                }
            }
            catch (Exception ex)
            {
                var connection = new ElasticConnection();
                string command = "http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/teletrack-test1";
                string jsonData = JsonConvert.SerializeObject(ex.Message);
                LogSeneEntity logSeneEntity = new LogSeneEntity();
                logSeneEntity.facility = store;
                logSeneEntity.timestamp = DateTime.Now;
                logSeneEntity.message = jsonData;

                string post = JsonConvert.SerializeObject(logSeneEntity);

                try
                {
                    var logsene = connection.Post(command, post);
                }
                catch { }
            }


            return returnResponse;
        }

        // POST api/deposits
        public void Post([FromBody]string value)
        {

        }

        // PUT api/deposits/5
        [HttpPut]
        [Route("api/dodeposits/{state}/{store}/{queryDate}")]
        public DoDepositsResponse Put(string state, string store, DateTime queryDate, [FromBody]object value)
        {

            DoDepositsResponse returnResponse = null;

            DepositTransactionEntity depositsTransaction = JsonConvert.DeserializeObject<DepositTransactionEntity>(value.ToString());

            Deposits deposits = new Deposits();
            //deposits.clverifyURI = clverifyURI;
            //deposits.clverifyAuthorizeString = clverifyAuthorizeString;

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();
            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            deposits.ConnectionString = connectionString;
            deposits.Store = store;

            try
            {
                if (deposits.ConnectionString != null)
                {
                    if (state.ToLower() == "colorado")
                        returnResponse = deposits.DoDepositsColorado(depositsTransaction);
                    else if (state.ToLower() == "wyoming")
                        returnResponse = deposits.DoDepositsWyoming(depositsTransaction);
                    else
                        returnResponse = null;
                }
            }
            catch (Exception ex)
            {
                returnResponse.Status = ex.StackTrace;
            }

            return returnResponse;
        }

        // DELETE api/deposits/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("api/deposits/{state}/{store}/{transactionId}")]
        public ResponseType Transaction(string state, string store, long transactionId)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            Repository.Transaction transRepository = new Repository.Transaction(connectionString);

            var transaction = transRepository.GetById(transactionId);

            var deposits = new Deposits();

            var payoff = deposits.CoPayoffAmount(transaction, new DateTime(2016,05,17));

            response.Item = transaction;

            return response;
        }



    }
}
