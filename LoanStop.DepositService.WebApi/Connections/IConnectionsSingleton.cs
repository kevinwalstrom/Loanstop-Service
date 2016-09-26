using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LoanStop.Entities.CommonTypes;

namespace LoanStop.Services.WebApi.Connections
{
    public interface IConnectionsSingleton
    {
        List<StoreConnectionType> Items {get; set; }
    }

}