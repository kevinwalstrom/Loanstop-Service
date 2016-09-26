using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Entity = LoanStopModel;

namespace LoanStop.Services.WebApi.Business
{
    public static class Consecutive
    {
    
        public static long DetermineConsecutive(ICollection<Entity.Transaction> trans)
        {
            long count = 0;

            var query1 = trans.Where( t => t.DateCleared.HasValue);

            if (query1.Count() > 0)
            {
                var query2 = query1.Where(t => Convert.ToDateTime(t.DateCleared) > DateTime.Now.AddDays(-4) && (t.Status == "Pd Cash" || t.Status == "Cleared"));

                foreach(var t in query2.ToList())
                {
                    if (t.Consecutive.HasValue)
                        count = t.Consecutive.Value;
                }
            }
            
            return count;
        }
        
    }
}