using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Quickbooks
{
    public interface IQuickbooks
    {
        List<string> Issue(ExportEntity record, ExportEntity received = null);
        List<string> CombinedPayment(DateTime date, decimal achPayment, decimal cashPayment, decimal fee, decimal principal);
        List<string> ACHPayment(DateTime date, decimal payment, decimal fee, decimal principal, string docNum);
        List<string> CashPaidPaymentPlan(DateTime date, decimal payment, decimal fee, decimal principal, string docNum);
        List<string> CheckPayment(DateTime date, decimal payment, decimal fee, decimal deferredFee, decimal principal, decimal cash, string docNum);
        List<string> CashPayment(DateTime date, decimal payment, decimal fee, decimal principal);
        List<string> Bounce(ExportEntity bounce, ExportEntity fee, ExportEntity principal);
        List<string> MoneyGram(ExportEntity cash, ExportEntity check);
        List<string> Check(ExportEntity cash, ExportEntity check);
        List<string> Gold(ExportEntity cash, ExportEntity check);
        List<string> Default(ExportEntity record);
        List<string> DefaultPayment(ExportEntity record);
        List<string> TransferToCorprate(ExportEntity record);
        List<string> TransferFromCorprate(ExportEntity record);
        List<string> GetCash(ExportEntity record);
        List<string> MiscDeposits(ExportEntity record);
        List<string> ExpenseCASH(ExportEntity record);
        List<string> ExpenseCHECK(ExportEntity record);
    }
}
