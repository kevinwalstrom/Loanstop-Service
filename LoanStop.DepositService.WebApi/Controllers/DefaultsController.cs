using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Microsoft.Practices.Unity;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business;
using LoanStop.Services.WebApi.Business.Wyoming;
using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Controllers
{

    public class DefaultsController : ApiController
    {
        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        [HttpGet]
        [Route("api/defaults/{state}/{store}")]
        public ResponseType Get(string state, string store)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var storeDefaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            response.Item = storeDefaults;
            
            return response;
        }

    }
}
