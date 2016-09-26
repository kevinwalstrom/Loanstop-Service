using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStop.Services.WebApi.Models
{
    public class ClientModel
    {
        public long Id { get; set; }
        public string SsNumber { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DriverLicense { get; set; }
        public string Employer { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string PaydaySchedule { get; set; }
        public string PaydayNotes { get; set; }
        public string HasMc { get; set; }
        public string Intl { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string AccountLimit { get; set; }
        public DateTime? AltPayday { get; set; }
        public string ApprovedBy { get; set; }
        public string CheckLimit { get; set; }
        public DateTime? DateOpened { get; set; }
        public string Mi { get; set; }
        public DateTime? Payday { get; set; }
        public string ReferredBy { get; set; }
        public string Store { get; set; }
        public DateTime? UpdatedInfoDate { get; set; }
        public int? TeletrackScore { get; set; }
        public string InquireCode { get; set; }
        
        //aux
        public string JointAccountId { get; set; }
        public string Master { get; set; }
        public DateTime Dob { get; set; }
        public string Occupation { get; set; }
        public string Deposits { get; set; }
        public string NetFromPaystubs { get; set; }
        public string RoutingNumber { get; set; }
        public string CellPhone { get; set; }
        public string MonthlyFundsAvailable { get; set; }
        public string PreferredContactMethod { get; set; }
        public string DateFromPaystub { get; set; }
        public string Email { get; set; }

    }
}