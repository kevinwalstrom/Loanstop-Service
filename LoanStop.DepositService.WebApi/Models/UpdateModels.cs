using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStop.Services.WebApi.Models
{
    public class UpdateNoteModel
    {
        public string SsNumber { get; set;}
        public string Note { get; set;}
    }

    public class UpdateStatusModel
    {
        public string SsNumber { get; set;}
        public string Status { get; set;}
    }

    public class UpdateClientContactInfoModel
    {
        public string SsNumber { get; set;}
        public string Store { get; set;}
        public string HomePhone { get; set;}
        public string WorkPhone { get; set;}
        public string WorkPhoneExtension { get; set;}
        public string CellPhone { get; set;}
        public string Email { get; set;}
        public string PreferredContactMethod { get; set;}
    }


}