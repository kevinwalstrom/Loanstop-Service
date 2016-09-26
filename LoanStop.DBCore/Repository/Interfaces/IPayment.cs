using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public interface IPayment
    {
        void Add(Entity.Payment payment);
    }
}
