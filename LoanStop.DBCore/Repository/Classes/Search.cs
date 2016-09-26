using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Entity = LoanStopModel;

namespace LoanStop.DBCore.Repository
{
    public class Search
    {
    
        public string ConnectionString {get; set;}

        public Search(string connectionString)
        {
            ConnectionString = connectionString;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public List<SearchClientEntity> SearchClients(string searchValue)
        {
            var rtrn = new List<SearchClientEntity>();

            Entity.Transaction newItem = new Entity.Transaction();

            using (var db = new  Entity.LoanStopEntities(ConnectionString))
            {
                //var qry1 = db.Searches.Where(c => c.Name.StartsWith(searchValue)).Select(s => new Entity.Client(){Firstname = s.Name, SsNumber = s.SsNumber}).Take(150);
                var qry1 = db.Searches.Where(c => c.Name.StartsWith(searchValue)).Select(s => new SearchClientEntity(){Firstname = s.Name, SsNumber = s.SsNumber}).Take(150);

                rtrn = qry1.ToList<SearchClientEntity>();
            }
            
            return rtrn;

        }

        public class SearchClientEntity
        {
            public long Id;
            public string Firstname;
            public string SsNumber;
        }

    }
}
