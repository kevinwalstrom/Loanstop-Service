using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.ACHWorks
{
    public class VerifyResponse
    {
        public bool Error { get; set; }
        public bool Status { get; set; }
        public string Code { get; set; }
        public string VerifyMessage { get; set; }
    }
}
