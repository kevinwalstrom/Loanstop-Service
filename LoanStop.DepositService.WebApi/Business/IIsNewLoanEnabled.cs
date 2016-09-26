using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model = LoanStopModel;
using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.CommonTypes;

namespace LoanStop.Services.WebApi.Business
{
    interface IIsNewLoanEnabled
    {
        bool Execute(Model.Client client, DateTime theDate, List<Model.Transaction> transactionList);
    }
}
