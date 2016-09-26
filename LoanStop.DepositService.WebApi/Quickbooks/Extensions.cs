using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanStop.Services.WebApi.Quickbooks
{
  
    
    public static class Extensions
    {
        private static string tabDelimiter = "\t";
        private static string commaDelimiter = ",";

        public static string ToQuickbooks(this QbEntity item)
        {
            var delimiter = tabDelimiter;
            string value = item.Trns + delimiter +  
                           item.TrnsType + delimiter + 
                           item.Date.ToString("MM/dd/yy") + delimiter +
                           item.Accnt + delimiter +
                           item.Name + delimiter +
                           item.Amount.ToString("N2") + delimiter +
                           item.Docnum + delimiter +
                           item.Memo + delimiter +
                           item.Class + delimiter +
                           item.Clear;
            return value;
        }
    }
}