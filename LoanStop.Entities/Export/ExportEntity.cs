using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Export
{
    public class ExportEntity 
    {
        public string catagory { get; set;}
        public decimal amount { get; set;}
        public string export_status { get; set;}
        public DateTime export_date { get; set;}
        public string doc_num { get; set;}
        public string memo { get; set;}
        public string transactiontype { get; set;}

    }

    public class QuickBooksStoreExport 
    {
        public string Store;
        public string State;
        public List<ExportEntity> Results;
    }

}
