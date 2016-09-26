using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoanStop.Entities.CommonTypes;

namespace LoanStop.Services.WebApi.Defaults
{
    public interface IDefaultsSingleton
    {
        List<DefaultType> Items { get; set; }
    }
}

