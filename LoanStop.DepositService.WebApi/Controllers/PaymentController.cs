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
    public class PaymentController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }


        [HttpGet]
        [Route("api/paymentscreen/{state}/{store}/{transId}")]
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
                var exe = new PaymentScreen(connectionString);
                var result = exe.GetTransactionAndPayments(transId);
                response.Item = result.GetType().GetProperty("Item").GetValue(result, null);
            }

            return response;
        }

        [HttpGet]
        [Route("api/paymentscreen/CoPayoffAmount/{state}/{store}/{transId}/{theDate}")]
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
                var exe = new PaymentScreen(connectionString);
                var result = exe.CoPayoffAmount(transId, theDate);
                response.Item = result.GetType().GetProperty("Item").GetValue(result, null);
            }

            return response;
        }


        [HttpPost]
        [Route("api/paymentscreen/update/{state}/{store}")]
        public ResponseType UpdateTransaction(string state, string store, PaymentScreenModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new PaymentScreen(connectionString);
                var result = exe.UpdatePaymentTransaction(
                    model.ssNumber,
                    model.transId,
                    model.amount,
                    model.balance,
                    model.description,
                    model.datePaid,
                    model.paymentType,
                    model.otherFees,
                    model.bReceivedCash,
                    model.bChangeClientStatusToPaid
                );
                response.Item = 0;
            }

            return response;
        }

        [HttpGet]
        [Route("api/paymentscreen/SetTransactionStatus/{state}/{store}/{transId}/{status}")]
        public ResponseType SetTransactionStatus(string state, string store, long transId, string Status)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new PaymentScreen(connectionString);
                exe.SetTransactionStatus(transId, Status);
                response.Item = 0;
            }

            return response;
        }

        




    }

    public class PaymentScreenModel
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
        public bool bChangeClientStatusToPaid;
    }

}