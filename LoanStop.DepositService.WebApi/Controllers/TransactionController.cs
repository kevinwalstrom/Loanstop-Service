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

namespace LoanStop.Services.WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:49292, http://test.loanstop.com", headers: "*", methods: "*")]
    public class TransactionController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }


        [HttpGet]
        [Route("api/transaction/{state}/{store}/{transactionId}")]
        public ResponseType Transaction(string state, string store, long transactionId)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var transRepository = new Repository.Transaction(connectionString);

            var transaction = transRepository.GetById(transactionId);

            response.Item = transaction;
            
            return response;
        }

        [HttpGet]
        [Route("api/paymenttable/{state}/{store}/{transactionId}")]
        public ResponseType PaymentTable(string state, string store, long transactionId)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName.ToLower() == store.ToLower()).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var exe = new Transaction(store, connectionString, defaults);

            var result = exe.PaymentTable(transactionId);

            response.Item = result;

            return response;
        }

        [HttpPost]
        [Route("api/moneygram/{state}/{store}")]
        public ResponseType MoneyGram(string state, string store, MoneyGramModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName.ToLower() == store.ToLower()).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var exe = new Transaction(store, connectionString, defaults);

            var result = exe.MoneyGram(model);

            response.Item = result;

            return response;
        }

        [HttpGet]
        [Route("api/GetPaymentPlanRecords/{state}/{store}/{transactionId}")]
        public ResponseType GetPaymentPlanRecords(string state, string store, long transactionId)
        {

            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.GetPaymentPlanRecords(transactionId);
                response.ResponseError = false;
                response.Item = result;
            }
            else if (state.ToUpper() == "COLORADO")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.GetPaymentPlanRecords(transactionId);
                response.ResponseError = false;
                response.Item = result;
            }

            return response;
        }
        
        [HttpGet]
        [Route("api/transaction/{state}/{store}/{ssNumber}/PaymentPlanEnabled")]
        public ResponseType PaymentPlanEnabled(string state, string store, string ssNumber)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();
            
            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {
                var wyoming = new PaymentPlan();
                wyoming.ConnectionString = connectionString;
                var result = wyoming.IsPaymentPlanEnabled(ssNumber);
                response.Item = result;
            }

            if (state.ToUpper() == "COLORADO")
            {


            }

            return response;
        }

        [HttpPost]
        [Route("api/transaction/{state}/{store}/bounce")]
        public ResponseType Bounce(string state, string store, BounceModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.Bounce(model);
                response.ResponseError = result.Error;
                response.Item = result.Ppcs;
            }

            return response;
        }

        [HttpGet]
        [Route("api/transaction/{state}/{store}/SetPpcStatus/{ppcid}/{status}")]
        public ResponseType SetPpcStatus(string state, string store, long ppcid, string status)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var ppcRepository = new Repository.PaymentPlanCheck(connectionString);

                var item = ppcRepository.GetById(ppcid);

                item.Status = status;

                var r = ppcRepository.Update(item);

                response.ResponseError = false;
            }

            return response;
        }

        [HttpPost]
        [Route("api/transaction/{state}/{store}/CashCheck")]
        public ResponseType CashCheck(string state, string store, CashCheckModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var exe = new Transaction(store, connectionString, defaults);
            var result = exe.CashCheck(model);
            response.ResponseError = result.Error;
            response.Item = result.Ppcs;

            return response;
        }

		[HttpGet]
        [Route("api/transaction/CashCheckTransactions/{accountNumber}")]
        public IHttpActionResult CashCheckTransactions(string accountNumber)
        {
            var response = new CheckCashingRecordsResponse();

            var exe = new LoanStop.Transactions.MasterDb();

			try
			{
				response = exe.CheckGetCheckCashingRecords(accountNumber);
			}
            catch (Exception ex)
			{
				return InternalServerError(ex);
			}			
			
			return Ok(response);

        }

		
		[HttpPost]
        [Route("api/transaction/{state}/{store}/CreateNewInstallmentLoan")]
        public ResponseType CreateNewInstallmentLoan(string state, string store, NewInstallmentLoanModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {

            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.NewInstallmentLoan(model);
                
                response.ResponseError = result.Error;
                response.Item = result;
            }

            return response;
        }

        [HttpPost]
        [Route("api/transaction/{state}/{store}/AdjustHoldDates")]
        public ResponseType AdjustHoldDates(string state, string store, AdjustHoldDatesModel model)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.AdjustHoldDates(store, model);
                
                response.ResponseError = result.Error;
                response.Item = result;
            }

            if (state.ToUpper() == "COLORADO")
            {
                var exe = new Transaction(store, connectionString, defaults);
                var result = exe.AdjustHoldDates(store, model);
                
                response.ResponseError = result.Error;
                response.Item = result;
            }

            return response;
        }

        [HttpGet]
        [Route("api/transaction/adjust/fetch/{store}/{id}")]
        public ResponseType FetchSingleTransaction(string store, int id)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var modeule  = new Adjust(store.ToLower(), connectionItem.ConnectionString(), defaults);

            if (connectionItem.State.ToLower() == "colorado")
            {
                response.Item = modeule.FetchColorado(id);
            }
            else
            {
                //response.Item = modeule.FetchWyoming(id);
            }
            
            return response;
        }

        [HttpGet]
        [Route("api/transaction/{store}/SetClientStatusForPaidBounce/{ssNumber}/{transId}")]
        public ResponseType SetClientStatusForPaidBounce(string store, string ssNumber, int transId)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.SetClientStatusForPaidBounce(ssNumber, transId);

            return response;

        }

        [HttpGet]
        [Route("api/transaction/{store}/SetTransactionStatus/{ssNumber}/{transId}")]
        public ResponseType SetTransactionStatus(string store, string ssNumber, int transId)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.SetTransactionStatus(ssNumber, transId);

            return response;

        }

	    [HttpPost]
        [Route("api/transaction/{store}/MinumimPayment")]
        public ResponseType MinumimPayment(string store, PaymentModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.MinumimPayment(model);

            return response;

        }

		[HttpPost]
        [Route("api/transaction/{store}/PartialPayment")]
        public ResponseType PartialPayment(string store, PaymentModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.PartialPayment(model);

            return response;

        }

		[HttpPost]
        [Route("api/transaction/{store}/PaidCash")]
        public ResponseType PaidCash(string store, PaymentModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();
            
            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.PaidCash(model);

            return response;

        }

        [HttpPost]
        [Route("api/transaction/{store}/PaidPayoffAmount")]
        public IHttpActionResult PaidPayoffAmount(string store, PaymentModel model)
        {
            var response = new ResponseType();
			response.ResponseError = false;

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            if (!trans.PaidPayoffAmount(model))
			{
				return InternalServerError();
			}

            return Ok();

        }

        [HttpPost]
        [Route("api/transaction/{store}/OverPayment")]
        public ResponseType OverPayment(string store, PaymentModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.OverPayment(model);

            return response;

        }

        [HttpPost]
        [Route("api/transaction/{store}/voidppc")]
        public ResponseType voidppc(string store, PaymentModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            trans.OverPayment(model);

            return response;

        }

        [HttpPost]
        [Route("api/transaction/createpaydayloan/{state}/{store}")]
        public ResponseType CreatePaydayLoan(string state, string store, NewPaydayLoanModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

            response.Item = trans.CreatePaydayLoan(model);

            return response;

        }

        [HttpPost]
        [Route("api/transaction/createpaymentplan/{state}/{store}")]
        public ResponseType CreatePaymentPlan(string state, string store, NewPaymentPlanModel model)
        {
            var response = new ResponseType();

            var connectionItem = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var defaults = Defaults.Items.Where(s => s.Store.ToLower() == store.ToLower()).FirstOrDefault();

            if (state.ToUpper() == "WYOMING")
            {
                var trans = new Transaction(store.ToLower(), connectionItem.ConnectionString(), defaults);

                trans.CreatePaymentPlan(model);

            }

            if (state.ToUpper() == "COLORADO")
            {

            }


            return response;

        }


    }
}
