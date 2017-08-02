using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

using Microsoft.Practices.Unity;

using LoanStop.Entities.Transaction;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business.Wyoming;
using LoanStop.Transactions;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Services.WebApi.Business;


namespace LoanStop.Services.WebApi.Controllers
{
    public class DefaultScreenController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }


        [HttpGet]
        [Route("api/defaultscreen/{state}/{store}/{transId}")]
        [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
        public ResponseType DefaultScreen(string state, string store, long transId)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new DefaultScreen(connectionString);
                var result = exe.GetTransactionAndPayments(transId);
                response.Item = result.GetType().GetProperty("Item").GetValue(result, null);
            }

            return response;
        }

        [HttpGet]
        [Route("api/defaultscreen/CoPayoffAmount/{state}/{store}/{transId}/{theDate}")]
        [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
        public ResponseType CoPayoffAmount(string state, string store, long transId, DateTime theDate)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new DefaultScreen(connectionString);
                var result = exe.CoPayoffAmount(transId, theDate);
                response.Item = result.GetType().GetProperty("Item").GetValue(result, null);
            }

            return response;
        }


        [HttpPost]
        [Route("api/defaultscreen/update/{state}/{store}")]
        public ResponseType UpdateTransaction(string state, string store, DefaultScreenModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new DefaultScreen(connectionString);
                var result = exe.UpdateDefaultedTransaction(
                    model.ssNumber,
                    model.transId,
                    model.amount,
                    model.balance,
                    model.description,
                    model.datePaid,
                    model.paymentType,
                    model.otherFees,
                    model.bReceivedCash,
                    model.bChangeClientStatusToPdDefaut
                );
                response.Item = 0;
            }

            return response;
        }




    }

    public class DefaultScreenModel
    {
        public string ssNumber;
        public long transId;
        public decimal amount;
        public decimal balance;
        public string description;
        public DateTime datePaid;
        public string paymentType;
        public decimal otherFees;
        public bool bReceivedCash;
        public bool bChangeClientStatusToPdDefaut;
    }

}
