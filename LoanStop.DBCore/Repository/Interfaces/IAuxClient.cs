using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Entity = LoanStopModel;


namespace LoanStop.DBCore.Repository
{
    public interface IAuxClient
    {
        void Add(Entity.AuxClient auxClient);
        void Update(Entity.AuxClient auxClient);
        Entity.AuxClient GetBySSNumber(string ssNumber);
    }
}
