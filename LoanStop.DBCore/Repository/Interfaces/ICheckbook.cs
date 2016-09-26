using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public interface ICheckbook
    {
        void Add(Entity.Checkbook checkbook);
    }
}
