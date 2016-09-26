using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{

    public class AuxClient : IAuxClient
    {

        public string ConnectionString {get; set;}

        public AuxClient(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public void Add(Entity.AuxClient auxClient)
        {
            Entity.AuxClient newItem = new Entity.AuxClient();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.CellPhone = auxClient.CellPhone;
                newItem.DateFromPaystub = auxClient.DateFromPaystub;
                newItem.Deposits = auxClient.Deposits;
                newItem.Dob = auxClient.Dob;
                newItem.Email = auxClient.Email;
                newItem.JointAccountId = auxClient.JointAccountId;
                newItem.Master = auxClient.Master;
                newItem.MonthlyFundsAvailable = auxClient.MonthlyFundsAvailable;
                newItem.NetFromPaystubs = auxClient.NetFromPaystubs;
                newItem.Occupation = auxClient.Occupation;
                newItem.PreferredContactMethod = auxClient.PreferredContactMethod;
                newItem.RoutingNumber = auxClient.RoutingNumber;
                newItem.SsNumber = auxClient.SsNumber;

                db.AuxClients.Add(newItem);

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        public void Update(Entity.AuxClient auxClient)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                Entity.AuxClient updateItem = db.AuxClients.Find(auxClient.Id);

                updateItem.CellPhone = auxClient.CellPhone;
                updateItem.DateFromPaystub = auxClient.DateFromPaystub;
                updateItem.Deposits = auxClient.Deposits;
                updateItem.Dob = auxClient.Dob;
                updateItem.Email = auxClient.Email;
                updateItem.JointAccountId = auxClient.JointAccountId;
                updateItem.Master = auxClient.Master;
                updateItem.MonthlyFundsAvailable = auxClient.MonthlyFundsAvailable;
                updateItem.NetFromPaystubs = auxClient.NetFromPaystubs;
                updateItem.Occupation = auxClient.Occupation;
                updateItem.PreferredContactMethod = auxClient.PreferredContactMethod;
                updateItem.RoutingNumber = auxClient.RoutingNumber;
                updateItem.SsNumber = auxClient.SsNumber;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public Entity.AuxClient GetBySSNumber(string ssNumber)
        {
            Entity.AuxClient rtrnAuxClient = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.AuxClients.Where(s => s.SsNumber == ssNumber);

                foreach (var aux in query)
                {
                    rtrnAuxClient = new Entity.AuxClient()
                    {
                        CellPhone = aux.CellPhone,
                        DateFromPaystub = aux.DateFromPaystub,
                        Deposits = aux.Deposits,
                        Dob = aux.Dob,
                        Email = aux.Email,
                        JointAccountId = aux.JointAccountId,
                        Master = aux.Master,
                        MonthlyFundsAvailable = aux.MonthlyFundsAvailable,
                        NetFromPaystubs = aux.NetFromPaystubs,
                        Occupation = aux.Occupation,
                        PreferredContactMethod = aux.PreferredContactMethod,
                        RoutingNumber = aux.RoutingNumber,
                        SsNumber = aux.SsNumber
                    };

                }
            }

            return rtrnAuxClient;
        }


    }

}
