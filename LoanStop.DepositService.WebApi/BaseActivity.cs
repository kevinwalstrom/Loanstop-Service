using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PlainElastic.Net;
using Newtonsoft.Json;
using LoanStop.Entities.logsene;

namespace LoanStop.Services.WebApi
{
    public class BaseActivity
    {
 
        protected string connectionString;
        protected string store;

        public BaseActivity(string store, string connectionString) 
        {
            this.connectionString = connectionString;
            this.store = store;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="action"></param>
        /// <param name="model"></param>
        protected void logAction(string store, string action, object model)
        {
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/{0}-{1}","INFORMATION", action);
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
            {}

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        /// <param name="action"></param>
        /// <param name="model"></param>
        protected void logException(string store, string action, Exception model)
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