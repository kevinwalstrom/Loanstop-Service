using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.Export;

namespace LoanStop.Services.WebApi.Quickbooks
{
    public abstract class BaseQbType : IQuickbooks
    {
        protected string cashAccount;
        protected string bankAccount;
        protected string bankAccount2;
        protected string classNumber;
        protected string storeName;
        protected string bankName;
        protected string bankName2;

        public BaseQbType(string store)
        {
            switch (store)
            {
                case "aurora":
                    bankAccount = "1030";
                    bankAccount2 = "1030";
                    cashAccount = "1035";
                    classNumber = "03 Aurora";
                    storeName = "Aurora";
                    bankName = "Aurora";
                    bankName2 = "Aurora";
                    break;
                case "southdenver":
                    bankAccount = "1140";
                    bankAccount2 = "1140";
                    cashAccount = "1145";
                    classNumber = "14 South Denver";
                    storeName = "South Denver";
                    bankName = "South Denver";
                    bankName2 = "South Denver";
                    break;
                case "thornton":
                    bankAccount = "1150";
                    bankAccount2 = "1150";
                    cashAccount = "1155";
                    classNumber = "15 Thornton";
                    storeName = "Thornton";
                    bankName = "Thornton";
                    bankName2 = "Thornton";
                    break;
                case "arvada":
                    bankAccount = "1020";
                    bankAccount2 = "1020";
                    cashAccount = "1025";
                    classNumber = "02 Arvada";
                    storeName = "Arvada";
                    bankName = "Arvada";
                    bankName2 = "Arvada";
                    break;
                case "pueblo":
                    bankAccount = "1120";
                    bankAccount2 = "1120";
                    cashAccount = "1125";
                    classNumber = "12 Pueblo";
                    storeName = "Pueblo";
                    bankName = "Pueblo";
                    bankName2 = "Pueblo";
                    break;
                case "fort_collins":
                    bankAccount = "1062";
                    bankAccount2 = "1060";
                    cashAccount = "1065";
                    classNumber = "06 Fort Collins";
                    storeName = "Fort Collins";
                    bankName = "Fort Collins Vectra";
                    bankName2 = "Fort Collins-US Bank";
                    break;
                case "grand_junction":
                    bankAccount = "1080";
                    bankAccount2 = "1080";
                    cashAccount = "1085";
                    classNumber = "08 Grand Junction";
                    storeName = "Grand Junction";
                    bankName = "Grand Junction";
                    bankName2 = "Grand Junction";
                    break;
                case "greeley":
                    bankAccount = "1092";
                    bankAccount2 = "1090";
                    cashAccount = "1095";
                    classNumber = "09 Greeley";
                    storeName = "Greeley";
                    bankName = "Greeley Vectra Bank";
                    bankName2 = "Greeley-US Bank";
                    break;
                case "littleton":
                    bankAccount = "1110";
                    bankAccount2 = "1110";
                    cashAccount = "1115";
                    classNumber = "11 Littleton";
                    storeName = "Littleton";
                    bankName = "Littleton";
                    bankName2 = "Littleton";
                    break;
                case "casper":
                    bankAccount = "1041";
                    bankAccount2 = "1042";
                    cashAccount = "1045";
                    classNumber = "04 Casper";
                    storeName = "Casper";
                    bankName = "Casper - Hilltop";
                    bankName2 = "Casper Corporate - Hilltop";
                    break;
                case "cheyenne":
                    bankAccount = "1051";
                    bankAccount2 = "1050";
                    cashAccount = "1055";
                    classNumber = "05 Cheyenne";
                    storeName = "Cheyenne";
                    bankName = "Cheyenne - Vectra";
                    bankName2 = "Cheyenne";
                    break;
                case "laramie":
                    bankAccount = "1101";
                    bankAccount2 = "1100";
                    cashAccount = "1105";
                    classNumber = "10 Laramie";
                    storeName = "Laramie";
                    bankName = "Laramie - Vectra";
                    bankName2 = "Laramie";
                    break;
                case "gillette":
                    bankAccount = "1070";
                    bankAccount2 = "1070";
                    cashAccount = "1075";
                    classNumber = "07 Gillette";
                    storeName = "Gillette";
                    bankName = "Gillette";
                    bankName2 = "Gillette";
                    break;
                case "rocksprings":
                    bankAccount = "1131";
                    bankAccount2 = "1130";
                    cashAccount = "1135";
                    classNumber = "13 Rock Springs";
                    storeName = "Rock Springs";
                    bankName = "Rock Springs - Vectra";
                    bankName2 = "Rock Springs";
                    break;
                case "closed_stores":
                    bankAccount = "1011";
                    bankAccount2 = "1011";
                    cashAccount = "1145";
                    classNumber = "99 Closed Stores";
                    storeName = "South Denver";
                    bankName = "Corporate-Vectra Bank";
                    bankName2 = "Corporate-Vectra Bank";
                    break;
            }

        }

        public abstract List<string> Issue(ExportEntity record, ExportEntity received = null);
        public abstract List<string> CombinedPayment(DateTime date, decimal achPayment, decimal cashPayment, decimal fee, decimal principal);
        public abstract List<string> ACHPayment(DateTime date, decimal payment, decimal fee, decimal principal, string docNum = null);
        public abstract List<string> CashPaidPaymentPlan(DateTime date, decimal payment, decimal fee, decimal principal,  string docNum = null);
        public abstract List<string> CheckPayment(DateTime date, decimal payment, decimal fee, decimal deferredFee, decimal principal, decimal cash, string docNum = null);
        public abstract List<string> CashPayment(DateTime date, decimal payment, decimal fee, decimal principal);
        public abstract List<string> Bounce(ExportEntity bounce, ExportEntity fee, ExportEntity principal);
        public abstract List<string> MoneyGram(ExportEntity cash, ExportEntity check);
        public abstract List<string> Check(ExportEntity cash, ExportEntity check);
        public abstract List<string> Gold(ExportEntity cash, ExportEntity check);
        public abstract List<string> Default(ExportEntity record);
        public abstract List<string> DefaultPayment(ExportEntity record);
        public abstract List<string> TransferToCorprate(ExportEntity record);
        public abstract List<string> TransferFromCorprate(ExportEntity record);
        public abstract List<string> GetCash(ExportEntity record);
        public abstract List<string> MiscDeposits(ExportEntity record);
        public abstract List<string> ExpenseCASH(ExportEntity record);
        public abstract List<string> ExpenseCHECK(ExportEntity record);
    
    }
}