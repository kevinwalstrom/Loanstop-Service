using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Repository = LoanStop.DBCore.Repository;
using LoanStop.Entities.Accounting;

namespace LoanStop.Services.WebApi.Business.Accounting
{
    public class CashData
    {
        
        private string connectionString;

        public CashData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<AccountingTableEntity> TableData(DateTime startDate, DateTime endDate)
        {

            var rep = new Repository.CashLog(connectionString);

            return rep.TableData(startDate, endDate, false);
        
            //Dim debit As String
            //Dim credit As String

            //Do While reader.Read
            //    Dim r As New dgItem

            //    Dim enc = New System.Text.UTF8Encoding()

            //    If IsArray(reader("debit")) Then
            //        debit = enc.GetString(reader("debit"))
            //    Else
            //        debit = reader("debit").ToString
            //    End If

            //    If IsArray(reader("credit")) Then
            //        credit = enc.GetString(reader("credit"))
            //    Else
            //        credit = reader("credit").ToString
            //    End If

            //    r.ID = reader("id").ToString
            //    r.dgDate = reader("date").ToString
            //    r.dgCheckNumberTransType = reader("transaction_type").ToString
            //    r.dgPayto = reader("payable_to").ToString
            //    r.dgDescription = reader("description").ToString
            //    If IsNumeric(debit) Then
            //        r.dgDebit = debit
            //        r.dgCredit = credit
            //        BegingSelectedCashBalance = BegingSelectedCashBalance - CSng(debit)
            //        r.dgBalance = Format(BegingSelectedCashBalance, "####0.00#")
            //    End If

            //    If IsNumeric(credit) Then
            //        r.dgDebit = debit
            //        r.dgCredit = credit
            //        BegingSelectedCashBalance = BegingSelectedCashBalance + CSng(credit)
            //        r.dgBalance = Format(BegingSelectedCashBalance, "####0.00#")
            //    End If
            //    colCash.Add(r)
            //    r = Nothing
            //Loop

        
        
        
        }

    
    
    
    
    
    }
}