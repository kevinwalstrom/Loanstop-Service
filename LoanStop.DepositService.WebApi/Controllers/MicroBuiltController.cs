using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using MySql.Data.MySqlClient;

using LoanStop.Services.WebApi.Models;
using Newtonsoft.Json;
//using LoanStop.MicroBuilt.Core;
using Repository = LoanStop.DBCore.Repository;

//namespace LoanStop.Services.WebApi.Controllers
//{
//    public class MicroBuiltController : ApiController
//    {
//        // GET api/microbuilt
//        public IEnumerable<string> Get()
//        {
//            return new string[] { "value1", "value2" };
//        }

//        // GET api/microbuilt/5
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/microbuilt
//        public void Post([FromBody]object value)
//        {
        
//        }

//        // PUT api/microbuilt/5
//        public HttpResponseMessage Put(int id, [FromBody]object value)
//        {
//            MicroBuiltRequestModel request = JsonConvert.DeserializeObject<MicroBuiltRequestModel>(value.ToString());

//            var microbuilt = new CLVerify();

            
//            var connectionString = GetDatabaseConnectionString(request.Store);

//            microbuilt.ConnectionString = connectionString;
//            Repository.Client clientRepository = new Repository.Client(connectionString);

//            LoanStopModel.Client client = clientRepository.GetBySSNumber(request.SSNumber);

//            if (request.RequestType == "Payment")
//                microbuilt.Payment(request.Store, client, request.TransactionId, request.Balance);
//            if (request.RequestType == "Closed")
//                microbuilt.Closed(request.Store, client, request.TransactionId, request.Balance);

//            return Request.CreateResponse(HttpStatusCode.OK);
//        }

//        // DELETE api/microbuilt/5
//        public void Delete(int id)
//        {
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="store"></param>
//        /// <returns></returns>
//        private string GetDatabaseConnectionString(string store)
//        {

//            string database = null;
//            string host = null;
//            string userId = null;
//            string password = null;

//            string sql = "SELECT id, store, ip, database_name, username, password, port, checking_database_name FROM master.mysql_ips WHERE store = '" + store + "'";

//            string MASTERCONNECTIONSTRING = ConfigurationManager.ConnectionStrings["master"].ConnectionString;

//            using (MySqlConnection con = new MySqlConnection(MASTERCONNECTIONSTRING))
//            {
//                con.Open();
//                MySqlDataReader reader = MySqlHelper.ExecuteReader(con, sql);

//                while (reader.Read())
//                {
//                    database = reader["database_name"].ToString();
//                    host = reader["ip"].ToString();
//                    userId = reader["username"].ToString();
//                    password = reader["password"].ToString();
//                }
//                reader.Close();
//            }

//            string connectionString = string.Format("Database={0};Data Source={1};User ID={2};Password={2};port=3306", database, host, userId, password);

//            return connectionString;
//        }

    
//    }
//}
