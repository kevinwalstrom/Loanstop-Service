using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{

    public class Client : IClient
    {
        public string ConnectionString {get; set;}

        public Client(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Add(Entity.Client client)
        {
            Entity.Client newItem = new Entity.Client();

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                newItem.AccountLimit = client.AccountLimit;
                newItem.Address = client.Address;
                newItem.AltPayday = client.AltPayday;
                newItem.ApprovedBy = client.ApprovedBy;
                newItem.BankAccount = client.BankAccount;
                newItem.BankName = client.BankName;
                newItem.CheckLimit = client.CheckLimit;
                newItem.City = client.City;
                newItem.DateOpened = client.DateOpened;
                newItem.DriverLicense = client.DriverLicense;
                newItem.Employer = client.Employer;
                newItem.Firstname = client.Firstname;
                newItem.HasMc = client.HasMc;
                newItem.HomePhone = client.HomePhone;
                newItem.Id = client.Id;
                newItem.Intl = client.Intl;
                newItem.Lastname = client.Lastname;
                newItem.Mi = client.Mi;
                newItem.Payday = client.Payday;
                newItem.PaydayNotes = client.PaydayNotes;
                newItem.PaydaySchedule = client.PaydaySchedule;
                newItem.ReferredBy = client.ReferredBy;
                newItem.SsNumber = client.SsNumber;
                newItem.State = client.State;
                newItem.Status = client.Status;
                newItem.Store = client.Store;
                newItem.UpdatedInfoDate = client.UpdatedInfoDate;
                newItem.WorkPhone = client.WorkPhone;
                newItem.Zip = client.Zip;
                newItem.TeletrackScore = client.TeletrackScore;
                newItem.InquireCode = client.InquireCode;

                db.Clients.Add(newItem);

                db.SaveChanges();
            }
        }

        public void Update(Entity.Client client)
        {
            Entity.Client updateItem = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                bool bNew = false;

                updateItem = db.Clients.Find(client.Id);
                
                //updateItem = db.Clients.Where(c => c.SsNumber == client.SsNumber).FirstOrDefault();

                if (updateItem == null)
                {
                    updateItem = new Entity.Client();
                    bNew = true;
                }

                updateItem.AccountLimit = client.AccountLimit;
                updateItem.Address = client.Address;
                updateItem.AltPayday = client.AltPayday;
                updateItem.ApprovedBy = client.ApprovedBy;
                updateItem.BankAccount = client.BankAccount;
                updateItem.BankName = client.BankName;
                updateItem.CheckLimit = client.CheckLimit;
                updateItem.City = client.City;
                updateItem.DateOpened = client.DateOpened;
                updateItem.DriverLicense = string.IsNullOrEmpty(client.DriverLicense) ? string.Empty : client.DriverLicense; 
                updateItem.Employer = string.IsNullOrEmpty(client.Employer) ? string.Empty : client.Employer;
                updateItem.Firstname = client.Firstname;
                updateItem.HasMc = client.HasMc;
                updateItem.HomePhone = client.HomePhone;
                //updateItem.Id = client.Id;
                updateItem.Intl = client.Intl;
                updateItem.Lastname = client.Lastname;
                updateItem.Mi = client.Mi;
                updateItem.Payday = client.Payday;
                updateItem.PaydayNotes = string.IsNullOrEmpty(client.PaydayNotes) ? string.Empty : client.PaydayNotes;
                updateItem.PaydaySchedule = client.PaydaySchedule;
                updateItem.ReferredBy = client.ReferredBy;
                updateItem.SsNumber = client.SsNumber;
                updateItem.State = client.State;
                updateItem.Status = client.Status;
                updateItem.Store = client.Store;
                updateItem.UpdatedInfoDate = client.UpdatedInfoDate;
                updateItem.WorkPhone = client.WorkPhone;
                updateItem.Zip = client.Zip;
                updateItem.TeletrackScore = client.TeletrackScore;
                updateItem.InquireCode = client.InquireCode;

                if (bNew)
                    db.Clients.Add(updateItem);
                
                db.SaveChanges();
            }
        }

        public Entity.Client GetBySSNumber(string ssNumber)
        {
            Entity.Client rtrnClient = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var query = db.Clients.Where(s => s.SsNumber == ssNumber);

                foreach (var client in query)
                {

                    rtrnClient = new Entity.Client()
                    {
                        AccountLimit = client.AccountLimit,
                        Address = client.Address,
                        AltPayday = client.AltPayday,
                        ApprovedBy = client.ApprovedBy,
                        BankAccount = client.BankAccount,
                        BankName = client.BankName,
                        CheckLimit = client.CheckLimit,
                        City = client.City,
                        DateOpened = client.DateOpened,
                        DriverLicense = client.DriverLicense,
                        Employer = client.Employer,
                        Firstname = client.Firstname,
                        HasMc = client.HasMc,
                        HomePhone = client.HomePhone,
                        Id = client.Id,
                        Intl = client.Intl,
                        Lastname = client.Lastname,
                        Mi = client.Mi,
                        Payday = client.Payday,
                        PaydayNotes = client.PaydayNotes,
                        PaydaySchedule = client.PaydaySchedule,
                        ReferredBy = client.ReferredBy,
                        SsNumber = client.SsNumber,
                        State = client.State,
                        Status = client.Status,
                        Store = client.Store,
                        UpdatedInfoDate = client.UpdatedInfoDate,
                        WorkPhone = client.WorkPhone,
                        Zip = client.Zip,
                        TeletrackScore = client.TeletrackScore,
                        InquireCode = client.InquireCode,

                    };

                    var auxRepository = new LoanStop.DBCore.Repository.AuxClient(ConnectionString);
                    rtrnClient.AuxClient = auxRepository.GetBySSNumber(rtrnClient.SsNumber); 
                    
                }
            
            }

            return rtrnClient;
        }

        public Entity.Client GetByClientId(long id)
        {
            Entity.Client rtrnClient = null;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                try
                {
                    var client = db.Clients.Find(id);

                    rtrnClient = new Entity.Client()
                    {
                        AccountLimit = client.AccountLimit,
                        Address = client.Address,
                        AltPayday = client.AltPayday,
                        ApprovedBy = client.ApprovedBy,
                        BankAccount = client.BankAccount,
                        BankName = client.BankName,
                        CheckLimit = client.CheckLimit,
                        City = client.City,
                        DateOpened = client.DateOpened,
                        DriverLicense = client.DriverLicense,
                        Employer = client.Employer,
                        Firstname = client.Firstname,
                        HasMc = client.HasMc,
                        HomePhone = client.HomePhone,
                        Id = client.Id,
                        Intl = client.Intl,
                        Lastname = client.Lastname,
                        Mi = client.Mi,
                        Payday = client.Payday,
                        PaydayNotes = client.PaydayNotes,
                        PaydaySchedule = client.PaydaySchedule,
                        ReferredBy = client.ReferredBy,
                        SsNumber = client.SsNumber,
                        State = client.State,
                        Status = client.Status,
                        Store = client.Store,
                        UpdatedInfoDate = client.UpdatedInfoDate,
                        WorkPhone = client.WorkPhone,
                        Zip = client.Zip,
                        TeletrackScore = client.TeletrackScore,
                        InquireCode = client.InquireCode,

                    };

                    var auxRepository = new LoanStop.DBCore.Repository.AuxClient(ConnectionString);
                    rtrnClient.AuxClient = auxRepository.GetBySSNumber(rtrnClient.SsNumber);
                }
                catch
                {

                }
            }

            return rtrnClient;
        }

        public List<string> GetNotes(string ssNumber)
        {
            List<string> rtrn = null;
   
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                try
                {
                    var client = db.CustomerNotes.Where(s => s.SsNumber == ssNumber).Select(s => s.Note);

                    rtrn = client.ToList(); 
                }
                catch
                {

                }
            }

            return rtrn;
        }

        public bool CheckSsNumber(string ssNumber)
        {
            bool rtrn = false;

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                var client = db.Clients.Where(s => s.SsNumber == ssNumber);
            
                if (client.Count() > 0)
                {
                    rtrn = true;
                }
            }

            return rtrn;
        }

        public void UpdateNotes(string ssNumber, string note)
        {
   
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                try
                {
                    var item = db.CustomerNotes.Where(s => s.SsNumber == ssNumber).FirstOrDefault();

                    if (item != null)
                    { 
                        item.Note = note;
                    }
                    else
                    {
                        item = new LoanStopModel.Notes();
                        item.SsNumber = ssNumber;
                        item.Note = note;
                        db.CustomerNotes.Add(item);
                    }
 
                    db.SaveChanges();
                
                }
                catch
                {

                }
            }

        }

        public void UpdateStatus(string ssNumber, string status)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                try
                {
                    var item = db.Clients.Where(s => s.SsNumber == ssNumber).FirstOrDefault();

                    item.Status = status;

                    db.SaveChanges();
                }
                catch
                {

                }
            }

        }

        public void UpdateContactInfo(string ssNumber, Dictionary<string, string> param)
        {
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {

                try
                {
                    var firstItem = db.Clients.Where(s => s.SsNumber == ssNumber).FirstOrDefault();

                    firstItem.Store = param["Store"];
                    firstItem.HomePhone = param["HomePhone"];
                    firstItem.WorkPhone = param["WorkPhone"];

                    var secondItem = db.AuxClients.Where(s => s.SsNumber == ssNumber).FirstOrDefault();

                    secondItem.CellPhone = param["CellPhone"];
                    secondItem.Email = param["Email"];
                    secondItem.PreferredContactMethod = param["PreferredContactMethod"];

                    db.SaveChanges();

                }
                catch
                {

                }
            }
        }

    }
}
