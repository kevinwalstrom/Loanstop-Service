using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStop.Services.WebApi.Models
{
    public class ResponseType
    {
        public bool ResponseError;
        public string Message;
        public string Status;
        public string Entity;
        public object Item;
    }
}