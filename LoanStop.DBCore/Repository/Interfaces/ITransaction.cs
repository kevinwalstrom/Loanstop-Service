using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public interface ITransaction
    {
        Entity.Transaction Add(Entity.Transaction transaction);
        void Update(Entity.Transaction transaction);
        Entity.Transaction GetById(long transactionId);
        ICollection<Entity.Transaction> GetByDate(DateTime queryDate);
        ICollection<Entity.Transaction> GetBySSNumber(string ssNumber);
    }
}
