using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.CommonTypes
{
    public class StoreConnectionType
    {
        public string StoreName { get; set; }
        public string StoreIP { get; set; }
        public string DatabaseName { get; set; }
        public string CheckbookDatabaseName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }
        public string VoiceServoID { get; set; }
        public string TextServoID { get; set; }
        public string VoicePhoneNumber { get; set; }
        public string TextPhoneNumber { get; set; }
        public string TextMessage { get; set; }
        public string Email { get; set; }
        public string State { get; set; }
        public string AccountInfo { get; set; }
        public string StoreCode { get; set; }

        public string ConnectionString()
        {
            return "Database=" + DatabaseName + ";Data Source=" + StoreIP + ";User ID=" + Username + ";Password=" + Password + ";port=" + Port + ";Convert Zero Datetime=True;Allow Zero Datetime=True;default command timeout=180;Connection Timeout=180;Pooling=True";
        }
    }
}
