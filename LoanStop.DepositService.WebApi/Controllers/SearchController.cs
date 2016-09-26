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
    public class SearchController : ApiController
    {
        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        [HttpGet]
        [Route("api/search/{state}/{store}/{value}")]
        public ResponseType Get(string state, string store, string value)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var storeDefaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var rep = new Repository.Search(connectionString);

            var resultsList = rep.SearchClients(value);

            response.Item = resultsList.Select(s => new {id = s.Id, SsNumber= s.SsNumber, name = s.Firstname});

            return response;
        }
    }
}
