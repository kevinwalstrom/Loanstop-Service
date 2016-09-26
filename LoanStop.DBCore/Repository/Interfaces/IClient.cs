using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public interface IClient
    {
        void Add(Entity.Client client);
        void Update(Entity.Client client);
        Entity.Client GetBySSNumber(string ssNumber);
    }
}
