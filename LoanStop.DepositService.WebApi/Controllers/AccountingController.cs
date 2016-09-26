using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;

using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business;
using LoanStop.Services.WebApi.Business.Accounting;
using LoanStop.Entities.logsene;
using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Controllers
{
    public class AccountingController : ApiController
    {
        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        // GET: api/Accounting/Transactions
        [HttpGet]
        [Route("api/Accounting/{state}/{store}/balances")]
        public ResponseType GetBalances(string state, string store)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();
            
            var account = new Balances(connectionString);

            decimal cash = 0;
            try
            {
                cash = account.CashBalance(DateTime.Now);
            }
            catch (Exception cashEx)
            {
                logException(store, "balances-CASH", cashEx);
            }

            decimal bank = 0;
            try
            {
                bank = account.BankBalance(DateTime.Now);
            }
            catch (Exception bankEx)
            {
                logException(store, "balances-BANK", bankEx);
            }

            return new ResponseType() { Item = new { Cash = cash, Bank = bank } };
        }

        // GET: api/Accounting/{state}/{store}/CashData/{startDate}/{endDate}
        //
        [HttpGet]
        [Route("api/Accounting/{state}/{store}/CashData/{startDate}/{endDate}")]
        public ResponseType CashData(string state, string store, DateTime startDate, DateTime endDate)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var cash = new Repository.CashLog(connectionString);

            var rtrn = cash.TableData(startDate, endDate, false);

            return new ResponseType() { Item = rtrn };
        }

        // GET: api/Accounting/{state}/{store}/CheckbookData/{startDate}/{endDate}
        //
        [HttpGet]
        [Route("api/Accounting/{state}/{store}/CheckbookData/{startDate}/{endDate}")]
        public ResponseType CheckbookData(string state, string store, DateTime startDate, DateTime endDate)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var checkbook = new Repository.Checkbook(connectionString);

            var rtrn = checkbook.TableData(startDate, endDate, false);

            return new ResponseType() { Item = rtrn };
        }


        [HttpPost]
        [Route("api/Accounting/{state}/{store}/Checkbook")]
        public ResponseType PostCheckbook(string state, string store, DateTime startDate, DateTime endDate)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var checkbook = new Repository.Checkbook(connectionString);

            var rtrn = checkbook.TableData(startDate, endDate, false);

            return new ResponseType() { Item = rtrn };
        }

        [HttpPost]
        [Route("api/Accounting/{state}/{store}/Cash")]
        public ResponseType PostCash(string state, string store, DateTime startDate, DateTime endDate)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var checkbook = new Repository.Checkbook(connectionString);

            var rtrn = checkbook.TableData(startDate, endDate, false);

            return new ResponseType() { Item = rtrn };
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
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}-{1}", "INFORMATION", action);
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
