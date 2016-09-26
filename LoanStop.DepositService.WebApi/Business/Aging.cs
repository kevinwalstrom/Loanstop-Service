using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Entity = LoanStopModel;


namespace LoanStop.Services.WebApi.Business
{
    public static class Aging
    {
        public static bool DetermineAging(ICollection<Entity.Transaction> trans)
        {
            bool count = false;

            var query1 = trans.Where(t => t.DateCleared > DateTime.Now.AddDays(-14) && t.CheckType == 0 && t.TransDate > DateTime.Now.AddDays(-61));

            if (query1.Count() > 0)
            {
                count = true;
            }

            return count;
        }

        public static long DaysLeftForAgingStatus(ICollection<Entity.Transaction> trans)
        {
            long days = 0;

            var query1 = trans.Where(t => t.DateCleared == null && t.CheckType == 0 && t.Status == "Open" && t.TransDate > DateTime.Now.AddDays(-61));

            if (query1.Count() > 0)
            {
                var agingTrans = query1.FirstOrDefault();

                var difference = agingTrans.TransDate.Value.AddDays(62).Subtract(DateTime.Now);

                days = (long)difference.TotalDays;
            }

            return days;
        }
    
    }
}