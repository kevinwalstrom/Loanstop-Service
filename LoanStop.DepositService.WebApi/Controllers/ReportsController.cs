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
using LoanStop.Services.WebApi.Quickbooks;
using LoanStop.Reports;
using LoanStop.DailyBalance;
using LoanStop.ACHFile;
//using LoanStop.VectraFile;

namespace LoanStop.Services.WebApi.Controllers
{
    public class ReportsController : ApiController
    {
        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }

        [HttpGet]
        [Route("api/report/export/{store}/{startDate}/{endDate}")]
        public ResponseType ExportStore(string store, DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            var storesList = Connections.Items.Where(s => s.DatabaseName == store.ToLower()).ToList();

            var execute = new ExportAll();
            
            response.Item = execute.ExportMultiple(storesList, startDate, endDate);


            return response;
        }

        [HttpGet]
        [Route("api/report/export/{store}/{lineItem}/{startDate}/{endDate}")]
        public ResponseType ExportStoreLineItem(string store, string lineItem, DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            var storeConnection = Connections.Items.Where(s => s.DatabaseName == store.ToLower()).FirstOrDefault();

            var execute = new Export(store, storeConnection.State, storeConnection.ConnectionString());
            
            response.Item = execute.ExportLineItem(store, lineItem, startDate, endDate);


            return response;
        }

        
        [HttpGet]
        [Route("api/report/export/state/{state}/{startDate}/{endDate}")]
        public ResponseType ExportByState(string state, DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            var storesList = Connections.Items.Where(s => s.State.ToLower() == state).ToList();

            var execute = new ExportAll();
            
            response.Item = execute.ExportMultiple(storesList, startDate, endDate);
            
            return response;
        }

        [HttpGet]
        [Route("api/report/export/all/{startDate}/{endDate}")]
        public ResponseType ExportAll(DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            var storesList = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming" ).ToList();
            
            var internet = storesList.Where( s => s.StoreName == "Internet").FirstOrDefault(); 
            storesList.Remove(internet);
            
            var execute = new ExportAll();
            
            response.Item = execute.ExportMultiple(storesList, startDate, endDate);

            return response;
        }

        [HttpGet]
        [Route("api/report/summary/{startDate}/{endDate}")]
        public ResponseType ExportByState(DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            var storesList = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming" ).ToList();

            var summary = new MonthlySummary();
            
            response.Item = summary.Execute(storesList, startDate, endDate);
            
            return response;
        }

        [HttpGet]
        [Route("api/report/export/file/{store}/{startDate}/{endDate}")]
        public ResponseType ExportFile(string store, DateTime startDate, DateTime endDate)
        {
            ResponseType response = new ResponseType();

            List<StoreConnectionType> storesList;

            if (store.ToLower() != "all") 
            { 
                storesList = Connections.Items.Where(s => s.DatabaseName == store.ToLower()).ToList();
            }
            else
            {
                storesList = Connections.Items.Where(w => w.State.ToLower() == "colorado" || w.State.ToLower() == "wyoming").ToList();
                var internet = Connections.Items.Where( s => s.StoreName == "Internet").FirstOrDefault(); 
                storesList.Remove(internet);
                //var closed = Connections.Items.Where( s => s.StoreName == "Closed Stores").FirstOrDefault(); 
                //storesList.Remove(closed);
            }

            List<string> rtrn = new List<string>();
            foreach (var item in storesList)
            { 
                Debug.WriteLine("Exporting file store : " + item.StoreName);

                var format = new MasterFormatter(item.DatabaseName, item.State, item.ConnectionString());
                if (item.StoreName == "Closed Stores")
                {
                    rtrn.AddRange(format.ExecuteClosed(startDate, endDate));
                }
                else if(item.State.ToLower() == "colorado")
                {
                    rtrn.AddRange(format.ExecuteCO(startDate, endDate));
                }
                else
                {
                    rtrn.AddRange(format.ExecuteWY(startDate, endDate));
                }
            }

            response.Item = rtrn;
            
            return response;
        }


        [HttpGet]
        [Route("api/report/dailybalance/{reportDate}")]
        public IHttpActionResult DailyBalance(DateTime reportDate)
        {
            ResponseType returnReport = new ResponseType(); 

            var storesList = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming" ).ToList();

            var dailyBalance = new Report();
            
            try
            { 
                var result = dailyBalance.RunReport(storesList, reportDate);
                returnReport.Item = result; 
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(returnReport);
        }
    

        [HttpGet]
        [Route("api/report/achfile/{depositDate}/{bankToDepositDate}")]
        public IHttpActionResult achfile(DateTime depositDate, DateTime bankToDepositDate)
        {
            ResponseType returnReport = new ResponseType(); 

            var storesList = Connections.Items.Where(s => s.State.ToLower() == "colorado").ToList();

            var ach = new ACH();
            
            try
            { 
                ach.Execute(storesList, depositDate, bankToDepositDate);
                returnReport.Item = ach.NACHAFile.file; 
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(returnReport);
        }
    
        //[HttpGet]
        //[Route("api/report/vectrafile/{reportDate}")]
        //public IHttpActionResult vectrafile(DateTime reportDate)
        //{
        //    //ResponseType returnReport = new ResponseType(); 

        //    //var storesList = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming" ).ToList();

        //    //var vectra = new Vectra();
            
        //    //try
        //    //{ 
        //    //    vectra.Execute(storesList, reportDate);
        //    //    returnReport.Item = vectra.File; 
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return InternalServerError(ex);
        //    //}

        //    //return Ok(returnReport);
        //}

        [HttpGet]
        [Route("api/report/wyoming/ScheduleA/{store}/{year}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult ScheduleA(string store, int year)
        {
            ResponseType returnReport = new ResponseType(); 
        
        
            var storeInfo = Connections.Items.Where( s => s.DatabaseName == store).FirstOrDefault();

            var report = new WyomingYearly(storeInfo.ConnectionString());
            
            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            { 
                returnReport.Item = report.YearlyReport(startDate, endDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(returnReport);
        
        }


        [HttpGet]
        [Route("api/report/Colorado/statesummary/{store}/{year}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult ColoradoStateSummary(string store, int year)
        {
            ResponseType returnReport = null;

            try
            {
                if (store.ToLower() == "all")
                {
                    returnReport = coloradoStateSummaryAll(year);
                }
                else
                {
                    returnReport = coloradoStateSummaryStore(store, year);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }

        [HttpGet]
        [Route("api/report/kristisummary/{startDate}/{endDate}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult kristisummary(DateTime startDate, DateTime endDate)
        {
            ResponseType returnReport = null;

            var stores = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming").ToList();
            stores.Remove(stores.Where(s => s.DatabaseName == "closed_stores").FirstOrDefault());


            var report = new KristiSummary(stores);

            try
            {

                returnReport = new ResponseType();
                returnReport.Item = report.Execute(startDate, endDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }


        #region "daily balance report
        [HttpGet]
        [Route("api/report/DailyBalanceSummary/{startDate}/{endDate}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult DailyBalanceSummary(DateTime startDate, DateTime endDate)
        {
            ResponseType returnReport = null;

            var stores = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming").ToList();
            stores.Remove(stores.Where(s => s.DatabaseName == "closed_stores").FirstOrDefault());

            var report = new Summary(stores);

            try
            {
                returnReport = new ResponseType();
                returnReport.Item = report.Execute(startDate, endDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }

        [HttpGet]
        [Route("api/report/DailyBalanceSummary/{store}/{startDate}/{endDate}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult DailyBalanceStoreSummary(string store, DateTime startDate, DateTime endDate)
        {
            ResponseType returnReport = null;

            var selectedStore = Connections.Items.Where(s => s.DatabaseName.ToLower() == store.ToLower()).ToList().FirstOrDefault();

            var report = new StoreSummary(selectedStore);

            try
            {

                returnReport = new ResponseType();
                returnReport.Item = report.Execute(startDate, endDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }

        [HttpGet]
        [Route("api/report/DailyBalanceSummary/detail/{store}/{category}/{startDate}/{endDate}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult DailyBalanceStoreDetail(string store, string category, DateTime startDate, DateTime endDate)
        {
            ResponseType returnReport = null;

            var selectedStores = Connections.Items.Where(s => s.DatabaseName.ToLower() == store.ToLower()).ToList();

            var report = new KristiSummary(selectedStores);

            try
            {

                returnReport = new ResponseType();
                returnReport.Item = report.Detail(category, startDate, endDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }
        #endregion 


        [HttpGet]
        [Route("api/report/dailysummary/{startDate}")]
        [EnableCors(origins: "http://localhost:8080, http://test.loanstop.com, http://localhost:49291, http://ls-server", headers: "*", methods: "*")]
        public IHttpActionResult dailysummary(DateTime startDate)
        {
            ResponseType returnReport = null;

            //var stores = Connections.Items.Where(s => s.State.ToLower() == "colorado" || s.State.ToLower() == "wyoming").ToList();

            var stores = Connections.Items.Where(s => s.StoreName.ToLower() == "arvada").ToList();

            //stores.Remove(stores.Where(s => s.DatabaseName == "closed_stores").FirstOrDefault());
            var report = new DailySummary(stores);


            try
            {

                returnReport = new ResponseType();
                returnReport.Item = report.Execute(startDate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }


            return Ok(returnReport);

        }


        protected ResponseType coloradoStateSummaryAll(int year)
        {

            ResponseType returnReport = new ResponseType();

            var stores = Connections.Items.Where(s => s.State.ToLower() == "colorado").ToList();
            stores.Remove(stores.Where(s => s.DatabaseName == "closed_stores").FirstOrDefault());

            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                List<ColoradoYearlyReport> list = new List<ColoradoYearlyReport>();
                foreach (var store in stores)
                {
                    var report = new ColoradoYearly(store.ConnectionString());
                    var storeReportItem = report.YearlyReport(startDate, endDate);
                    list.Add(storeReportItem);
                }

                var sumColoradoStores = new SumColoradoStores();
                returnReport.Item = sumColoradoStores.Execute(list);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnReport;
        }


        protected ResponseType coloradoStateSummaryStore(string store, int year)
        {

            ResponseType returnReport = new ResponseType();

            var storeInfo = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault();

            var report = new ColoradoYearly(storeInfo.ConnectionString());

            DateTime startDate = new DateTime(year, 1, 1);
            DateTime endDate = new DateTime(year, 12, 31);

            try
            {
                returnReport.Item = report.YearlyReport(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return returnReport;
        }

    }
}


