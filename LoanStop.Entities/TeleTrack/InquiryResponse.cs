using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.TeleTrack
{
    public class InquiryResponse
    {
        public bool Error {get; set;}
        public string TransactionCode {get; set;}
        public int TeleTrackScore { get; set; }
    }
}
