using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;
using LoanStop.DBCore;
using LoanStopModel;
using LoanStop.TeleTrackClient;
using LoanStop.Entities;
using LoanStop.Entities.TeleTrack;

using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using LoanStop.Services.WebApi.Models;
using LoanStop.Services.WebApi.Business.Client;

using Repository = LoanStop.DBCore.Repository;

namespace LoanStop.Services.WebApi.Controllers
{
    public class ClientController : ApiController
    {

        [Dependency]
        public IDefaultsSingleton Defaults { get; set; }

        [Dependency]
        public IConnectionsSingleton Connections { get; set; }
        
        private string ACCESS_KEY = ConfigurationManager.AppSettings["AccessKey"];
        
        // GET api/client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/client/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/client
        public void Post([FromBody]RequestType request)
        {

            var connectionString = Connections.Items.Where(s => s.StoreName == request.Store).FirstOrDefault().ConnectionString();

            if (Request.Headers.Contains("access-key"))
            {
                    var client = JsonConvert.DeserializeObject<Client>(request.Entity);

                    var clientRepository = new Repository.Client(connectionString);

                    clientRepository.Update(client);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verifyId"></param>
        /// <param name="request"></param>
        public ResponseType Post(string verifyId, [FromBody]RequestType request)
        {
            AuxClient auxClient = null;

            ResponseType response = new ResponseType();

            if (Request.Headers.Contains("access-key"))
            {
                    var connectionString = Connections.Items.Where(s => s.DatabaseName == request.Store).FirstOrDefault().ConnectionString();
                    var defaults = Defaults.Items.Where(s => s.Store == request.Store).FirstOrDefault();

                    var inquiryResponse = new InquiryResponse();

                    var client = JsonConvert.DeserializeObject<Client>(request.Entity);

                    if (client.AuxClient != null)
                    {
                        auxClient = JsonConvert.DeserializeObject<AuxClient>(client.AuxClient.ToString());
                    }

                    var teletrack = new ColoradoTeleTrack();
                    var teletrackResonse = new InquiryResponse() { Error = true };
                     
                    if (auxClient != null)
                    {
                        teletrack.Facility = request.Store;
                        teletrack.UserName = defaults.TeleTrackUserName;
                        teletrack.SubscriberID = defaults.TeleTrackSubscriberID;
                        teletrack.Password = defaults.TeleTrackPassword;
                        teletrackResonse = teletrack.Inquiry(client, auxClient);
                    }

                    if (!teletrackResonse.Error)
                    {
                        // clverify tracking table
                        var rep = new Repository.Tracking();

                        var clientInquirie = new CustomerInquirie()
                        {
                            Store = defaults.StoreCode,
                            Name = client.Firstname + " " + client.Lastname,  
                            Code = teletrackResonse.TeleTrackScore.ToString(),
                            Message = "denied"
                        };


                        if (teletrackResonse.TeleTrackScore > defaults.TeleTrackScore || teletrackResonse.TeleTrackScore == 0)
                        {
                            var clientRepository = new Repository.Client(connectionString);

                            client.TeletrackScore = teletrackResonse.TeleTrackScore;
                            client.InquireCode = teletrackResonse.TransactionCode;
                            client.Status = "0";
                            clientRepository.Update(client);
                            clientInquirie.Message = "approved";
                        }

                        // save to clverify tracking table
                        clientInquirie.DateEntered = DateTime.Now;
                        var result = rep.SaveCustomerInquirie(clientInquirie);

                    }

                    response.Entity = JsonConvert.SerializeObject(teletrackResonse); ;
                    response.Status = "Success";
                    response.ResponseError = false;
                }
                else
                {
                    response.Status = "Not Authorized";
                    response.ResponseError = true;
                }

            return response;
        }

        [HttpPost]
        [Route("api/client/{state}/{store}/Save")]
        public ResponseType Save(string state, string store, ClientModel model)
        {
            ResponseType response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var repository = new SaveClient();
            
            repository.ConnectionString = connectionString;

            repository.SaveToAll(model);

            return response;
        }

        [HttpGet]
        [Route("api/client/{store}/{ssnumber}")]
        public ResponseType Get(string store, string ssnumber)
        {
            ResponseType response = new ResponseType();

            if (Request.Headers.Contains("access-key"))
            {
                    var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

                    Repository.Client clientRepository = new Repository.Client(connectionString);

                    var client = clientRepository.GetBySSNumber(ssnumber);

                    response.Item = client;

                    response.Status = "Success";
                    response.ResponseError = false;
            }
            else
            {
                response.Status = "No Access Key Sent";
                response.ResponseError = true;
            }

            return response;
        
        }


        [HttpGet]
        [Route("api/client/{state}/{store}/CheckSsNumber/{ssNumber}")]
        public ResponseType Get(string state, string store, string ssNumber)
        {
            ResponseType response = new ResponseType();

            if (Request.Headers.Contains("access-key"))
            {
                    bool isClient = false;
                    string text = null;

                    var rtrn = new {IsClient = false, Store = "none"};

                    var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

                    Repository.Client clientRepository = new Repository.Client(connectionString);
                    Repository.Tracking trackingRepository = new Repository.Tracking();

                    var masterClient = trackingRepository.CheckSsNumber(ssNumber);
                    
                    if (masterClient["IsClient"] == "true")
                    {
                        isClient = true;
                        store = masterClient["Store"];
                        text = string.Format("This SS Number already exists at another store. {0}", masterClient["Store"]);
                    }

                    var storeClient = clientRepository.CheckSsNumber(ssNumber);

                    if (storeClient)
                    {
                        isClient = true;
                        text = string.Format("Client exists at your store {0}", store);
                    }

                    response.Status = "Success";
                    response.ResponseError = false;
                    response.Item = new { IsClient = isClient, Store = store, Text = text };
            }
            else
            {
                response.Status = "No Access Key Sent";
                response.ResponseError = true;
            }

            return response;
        
        }

        [HttpPost]
        [Route("api/client/{state}/{store}/ContactInfo")]
        public void UpdateContactInfo(string state, string store, UpdateClientContactInfoModel value)
        {

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var clientRepository = new Repository.Client(connectionString);

            var param = new Dictionary<string, string>();

            param.Add("Store", value.Store);
            param.Add("HomePhone", value.HomePhone);
            param.Add("WorkPhone", value.WorkPhone);
            param.Add("CellPhone", value.CellPhone);
            param.Add("Email", value.Email);
            param.Add("PreferredContactMethod", value.PreferredContactMethod);

            clientRepository.UpdateContactInfo(value.SsNumber, param);
        }
    
        [HttpPost]
        [Route("api/client/{state}/{store}/status")]
        public ResponseType UpdateStatus(string state, string store, UpdateStatusModel value)
        {
            var response = new ResponseType();

            var connectionString = Connections.Items.Where(s => s.DatabaseName == store).FirstOrDefault().ConnectionString();

            var defaults = Defaults.Items.Where(s => s.Store == store).FirstOrDefault();

            var clientRepository = new Repository.Client(connectionString);
            
            clientRepository.UpdateStatus(value.SsNumber, value.Status);

            response.Item = new {};

            return response;
        }

    }
}
