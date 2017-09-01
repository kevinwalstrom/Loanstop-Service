using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics;

using Microsoft.Practices.Unity;
using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Entities.CommonTypes;
using LoanStop.Entities.Reports;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Reports;
using LoanStop.DBCore.Collections;

namespace LoanStop.Services.WebApi.Controllers
{
    public class CollectionsController : ApiController
    {
        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        [HttpGet]
        [Route("api/collections/AccountsToFollowUp/{state}/{store}")]
        public IHttpActionResult AccountsToFollowUp(string state, string store)
        {
            ResponseType returnReport = null;

            var currentStore = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var report = new LoanStop.DBCore.Collections.Reports(currentStore.ConnectionString());

            try
            {
                returnReport = new ResponseType();
                returnReport.Item = report.AccountsToFollowUp();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }



    }
}


