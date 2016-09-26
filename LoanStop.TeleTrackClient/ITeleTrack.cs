using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoanStopModel;
using LoanStop.Entities.TeleTrack;

namespace LoanStop.TeleTrackClient
{
    interface ITeleTrack
    {
         InquiryResponse Inquiry(Client client, AuxClient auxClient);
         ReportResponse IssueLoan(Client client, Transaction transaction, PaymentPlanCheck ppc, string store);
         ReportResponse PaidInFull(Client client, Transaction transaction, string store);
         ReportResponse BouncedInstallment(Client client, Transaction transaction, PaymentPlanCheck ppc, string store);
         ReportResponse ChargeOff(Client client, Transaction transaction, string store);
    }
}

