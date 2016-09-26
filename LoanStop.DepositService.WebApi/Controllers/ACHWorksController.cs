using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;
using PlainElastic.Net;

using LoanStop.DBCore;
using LoanStopModel;

using LoanStop.ACHWorks;

using LoanStop.Entities;
using LoanStop.Entities.TeleTrack;
using LoanStop.Entities.logsene;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;

using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Controllers
{
    public class ACHWorksController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        private string ACCESS_KEY = ConfigurationManager.AppSettings["AccessKey"];

        [HttpGet]
        [Route("api/achworks/{store}/{value}/{amount}")]
        public ResponseType CheckClient(string store, string value, string amount)
        {

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var clientRepository = new Repository.Client(connectionString);

            var client = clientRepository.GetBySSNumber(value);

            var ach = new ACHWorksClass(defaults);

            var result = ach.TSSVerify(client, amount);

            if (result == null)
            {
                log(client, "ACH WORKS RESPONSE = NULL");
            }

            return new ResponseType() { Item = result };

        }


        private void log(Client client, string message)
        {

            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/teletrack");
            string jsonData = JsonConvert.SerializeObject(message);
            jsonData = jsonData + JsonConvert.SerializeObject(client);
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = "ACH Works";
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
}
