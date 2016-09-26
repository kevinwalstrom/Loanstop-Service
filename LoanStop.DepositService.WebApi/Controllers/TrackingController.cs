using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.Practices.Unity;
using LoanStop.Entities;
using LoanStop.Entities.logsene;
using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business.Wyoming;

using Repository = LoanStop.DBCore.Repository;
using LoanStop.DBCore;

namespace LoanStop.Services.WebApi.Controllers
{
    public class TrackingController : ApiController
    {
        // GET: api/Tracking
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/tracking/CheckCashing/{id}")]
        public ResponseType GetCheckCashing(long id)
        {
            var response = new ResponseType();

            var rep = new Repository.Tracking();

            var result = rep.GetCheckCashing(id);

            return new ResponseType() { Item = result };

        }

        [HttpGet]
        [Route("api/tracking/CustomerInquirie/{id}")]
        public ResponseType GetGetCustomerInquirie(long id)
        {
            var response = new ResponseType();

            var rep = new Repository.Tracking();

            var result = rep.GetCustomerInquirie(id);

            return new ResponseType() { Item = result };

        }

        [HttpPost]
        [Route("api/tracking/checkcashing")]
        public ResponseType PostCheckCashing(CheckCashing value)
        {

            var response = new ResponseType();

            var rep = new Repository.Tracking();

            var result = rep.SaveCheckCashing(value);

            return new ResponseType() { Item = result };
        
        }

        [HttpPost]
        [Route("api/tracking/CustomerInquirie")]
        public ResponseType Post(CustomerInquirie value)
        {

            var response = new ResponseType();

            var rep = new Repository.Tracking();

            var result = rep.SaveCustomerInquirie(value);

            return new ResponseType() { Item = result };

        }

        [HttpGet]
        [Route("api/tracking/ClientCheckCashing/{accountNumber}/{date}")]
        public ResponseType GetCheckCashing(string accountNumber, DateTime date)
        {
            var response = new ResponseType();

            var rep = new Repository.Tracking();

            var result = new 
                {
                    Checks = rep.ClientCheckCashing(accountNumber),
                    TotalCashed = rep.TotalCashed(accountNumber),
                    TotalCashedLast5Days = rep.TotalCashedLast5Days(accountNumber, date)
                }; 

            return new ResponseType() { Item = result };

        }

    
        [HttpPost]
        [Route("api/tracking/logerror")]
        public ResponseType logerror(LogModel model)
        {

            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/teletrack");
            string jsonData = JsonConvert.SerializeObject(model);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = string.Format("{0}-desktop",model.Store);
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch { }

            return new ResponseType() { Item = null };

        }

    
    }
}
