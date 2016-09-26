using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Services.WebApi.Models;
using System.Data.Entity.Validation;
using Entity = LoanStopModel;

namespace LoanStop.Services.WebApi.Business.Client
{
    public class SaveClient
    {
     
        public string ConnectionString;
        
        public void SaveToAll(ClientModel model)
        { 
            Entity.Client clientItem = null;
            Entity.AuxClient auxItem = null;
            
            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                bool bNew = false;

                clientItem = db.Clients.Where(c => c.SsNumber == model.SsNumber).FirstOrDefault();

                if (clientItem == null)
                {
                    clientItem = new Entity.Client();
                    bNew = true;
                }

                try
                {
                    clientItem.AccountLimit = model.AccountLimit;
                    clientItem.Address = model.Address;
                    clientItem.AltPayday = model.AltPayday;
                    clientItem.ApprovedBy = model.ApprovedBy;
                    clientItem.BankAccount = model.BankAccount;
                    clientItem.BankName = model.BankName;
                    clientItem.CheckLimit = model.CheckLimit;
                    clientItem.City = model.City;
                    clientItem.DateOpened = bNew ? DateTime.Now.Date : clientItem.DateOpened;
                    clientItem.DriverLicense = string.IsNullOrEmpty(model.DriverLicense) ? string.Empty : model.DriverLicense; 
                    clientItem.Employer = string.IsNullOrEmpty(model.Employer) ? string.Empty : model.Employer;
                    clientItem.Firstname = model.Firstname;
                    clientItem.HasMc = "No";
                    clientItem.HomePhone = model.HomePhone;
                    //updateItem.Id = client.Id;
                    clientItem.Intl = "No";
                    clientItem.Lastname = model.Lastname;
                    clientItem.Mi = model.Mi;
                    clientItem.Payday = model.Payday;
                    clientItem.PaydayNotes = string.IsNullOrEmpty(model.PaydayNotes) ? string.Empty : model.PaydayNotes;
                    clientItem.PaydaySchedule = string.IsNullOrEmpty(model.PaydaySchedule) ? string.Empty : model.PaydaySchedule;
                    clientItem.ReferredBy = model.ReferredBy;
                    clientItem.SsNumber = model.SsNumber;
                    clientItem.State = model.State;
                    clientItem.Status = model.Status;
                    clientItem.Store = model.Store;
                    clientItem.UpdatedInfoDate = model.UpdatedInfoDate;
                    clientItem.WorkPhone = model.WorkPhone;
                    clientItem.Zip = model.Zip;
                    clientItem.TeletrackScore = model.TeletrackScore;
                    clientItem.InquireCode = model.InquireCode;

                    if (bNew == true)
                    { 
                        db.Clients.Add(clientItem);
                    }
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        ex.Message, ex.InnerException);

                }
            }
                

            using (var db = new Entity.LoanStopEntities(ConnectionString))
            {
                
                try
                {
                    bool bNew = false;
                    auxItem = db.AuxClients.Where(c => c.SsNumber == model.SsNumber).FirstOrDefault();

                    if (auxItem == null)
                    {
                       auxItem = new Entity.AuxClient();
                       bNew = true; 
                    }

                    auxItem.CellPhone = model.CellPhone;
                    auxItem.DateFromPaystub = model.DateFromPaystub;
                    auxItem.Deposits = model.Deposits;
                    auxItem.Dob = model.Dob.ToString("yyyy-MM-dd");
                    auxItem.Email = model.Email;
                    auxItem.JointAccountId = model.JointAccountId;
                    auxItem.Master = "False";
                    auxItem.MonthlyFundsAvailable = model.MonthlyFundsAvailable;
                    auxItem.NetFromPaystubs = model.NetFromPaystubs;
                    auxItem.Occupation = model.Occupation.Length > 45 ? model.Occupation.Substring(0,45) : model.Occupation ;
                    auxItem.PreferredContactMethod = model.PreferredContactMethod;
                    auxItem.RoutingNumber = model.RoutingNumber;
                    auxItem.SsNumber = model.SsNumber;

                    if (bNew == true)
                    { 
                        db.AuxClients.Add(auxItem);
                    }
                
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                }               
            }

            //save to master client table
            Entity.MasterClient masterItem = null;

            using (var masterDb = new Entity.MasterDb())
            {
                bool bNew = false;

                try { 
                    masterItem = masterDb.MasterClients.Where(c => c.SsNumber == model.SsNumber).FirstOrDefault();

                    if (clientItem == null)
                    {
                        clientItem = new Entity.Client();
                        bNew = true;
                    }

                    masterItem.AccountLimit = model.AccountLimit == null ? string.Empty : model.AccountLimit;
                    masterItem.Address = model.Address;
                    //masterItem.AltPayday = model.AltPayday;
                    masterItem.ApprovedBy = model.ApprovedBy;
                    masterItem.BankAccount = model.BankAccount;
                    masterItem.BankName = model.BankName;
                    masterItem.CheckLimit = model.CheckLimit;
                    masterItem.City = model.City;
                    //masterItem.DateOpened = model.DateOpened;
                    masterItem.DriverLicense = string.IsNullOrEmpty(model.DriverLicense) ? string.Empty : model.DriverLicense; 
                    masterItem.Employer = string.IsNullOrEmpty(model.Employer) ? string.Empty : model.Employer;
                    masterItem.Firstname = model.Firstname;
                    //masterItem.HasMc = model.HasMc;
                    masterItem.HomePhone = model.HomePhone;
                    //updateItem.Id = client.Id;
                    //masterItem.Intl = model.Intl;
                    masterItem.Lastname = model.Lastname;
                    masterItem.Mi = model.Mi;
                    //masterItem.Payday = model.Payday;
                    //masterItem.PaydayNotes = string.IsNullOrEmpty(model.PaydayNotes) ? string.Empty : model.PaydayNotes;
                    //masterItem.PaydaySchedule = model.PaydaySchedule;
                    masterItem.ReferredBy = model.ReferredBy;
                    masterItem.SsNumber = model.SsNumber;
                    masterItem.State = model.State;
                    masterItem.Status = model.Status;
                    masterItem.Store = model.Store;
                    masterItem.UpdatedInfoDate = model.UpdatedInfoDate;
                    masterItem.WorkPhone = model.WorkPhone;
                    masterItem.Zip = model.Zip;
                    //masterItem.TeletrackScore = model.TeletrackScore;
                    //masterItem.InquireCode = model.InquireCode;

                    if (bNew)
                    { 
                        masterDb.MasterClients.Add(masterItem);
                    }
            
                    masterDb.SaveChanges();
                }
                catch (Exception ex)
                {


                }
            
            }

        }
    }
}