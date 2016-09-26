using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanStop.Entities.Export;

namespace LoanStop.DBCore.Repository.Interfaces
{
    public interface IStateQuries
    {
 
        List<ExportEntity> CallExport(DateTime startDate, DateTime endDate);
        List<ExportEntity> ExportLineItem(string lineItem, DateTime startDate, DateTime endDate);
    }
}
