using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoanStop.DailyBalance
{
    public enum SummaryColumns
    {
        UnDepositExcludeCO,
        UnDepositCO,
        PaymentsBalance,
        Bounced,
        Paid,
        AmountDispursed,
        AmountRecieved,
        AmountCardFees,
        AmountCash,
        AmountChecking,
        AmountTransfers,
        AmountExpenses
    }
    
    class Queries
    {
        public static string DatabaseName;
        public static string CheckbookDatabaseName;

        private static StringBuilder _SummaryUnDepositExcludeCO;
        private static string _SummaryUnDepositCO;
        private static string _SummaryPaymentsBalance;
        private static string _SummaryReceivablesBalance;
        private static string _SummaryBounced;
        private static string _SummaryPaid;
        private static string _SummaryAmountDispursed;
        private static string _SummaryAmountRecieved;
        private static string _SummaryAmountCardFees;
        private static string _SummaryAmountCash;
        private static string _SummaryAmountChecking;
        private static StringBuilder _SummaryAmountTransfers;
        private static StringBuilder _SummaryAmountExpenses;

        public static DO SetObject(SummaryColumns QueryType)
        {
            string sql = "";

            SetQueries();
            
            switch (QueryType)
            {

                case SummaryColumns.UnDepositExcludeCO:
                    sql = _SummaryUnDepositExcludeCO.ToString() ;
                    break;
                case SummaryColumns.UnDepositCO:
                    sql = _SummaryUnDepositCO;
                    break;
                case SummaryColumns.PaymentsBalance:
                    sql = _SummaryPaymentsBalance;
                    break;
                case SummaryColumns.Bounced:
                    sql = _SummaryBounced;
                    break;
                case SummaryColumns.Paid:
                    sql = _SummaryPaid;
                    break;
                case SummaryColumns.AmountDispursed:
                    sql =_SummaryAmountDispursed;
                    break;
                case SummaryColumns.AmountRecieved:
                    sql =_SummaryAmountRecieved;
                    break;
                case SummaryColumns.AmountCardFees:
                    sql = _SummaryAmountCardFees;
                    break;
                case SummaryColumns.AmountCash:
                    sql = _SummaryAmountCash;
                    break;
                case SummaryColumns.AmountChecking:
                    sql = _SummaryAmountChecking;
                    break;
                case SummaryColumns.AmountTransfers:
                    sql = _SummaryAmountTransfers.ToString();
                    break;
                case SummaryColumns.AmountExpenses:
                    sql = _SummaryAmountExpenses.ToString();
                    break;
            }

            return new DO(sql);

        }

        public static void SetQueries()
        {

            //UnDepositExcludeCO
            _SummaryUnDepositExcludeCO = new StringBuilder("");
            _SummaryUnDepositExcludeCO.Append(" SELECT ");
            _SummaryUnDepositExcludeCO.Append("   SUM(amount_recieved) as amount_recieved ");
            _SummaryUnDepositExcludeCO.Append(" FROM (" + DatabaseName + ".transactions) ");
            _SummaryUnDepositExcludeCO.Append(" WHERE ");
            _SummaryUnDepositExcludeCO.Append(" ( ");
            _SummaryUnDepositExcludeCO.Append("	( ");
            _SummaryUnDepositExcludeCO.Append("		TO_DAYS(trans_date) <= TO_DAYS(@TheDate)  ");
            _SummaryUnDepositExcludeCO.Append("			AND  ");
            _SummaryUnDepositExcludeCO.Append("		TO_DAYS(date_cleared)  > TO_DAYS(@TheDate) ");
            _SummaryUnDepositExcludeCO.Append("	) ");
            _SummaryUnDepositExcludeCO.Append("	AND (status != 'Void' OR status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default' ) ");
            _SummaryUnDepositExcludeCO.Append(" )  ");
            _SummaryUnDepositExcludeCO.Append("	OR  ");
            _SummaryUnDepositExcludeCO.Append(" ( ");
            _SummaryUnDepositExcludeCO.Append("	TO_DAYS(trans_date) <= TO_DAYS(@TheDate)  ");
            _SummaryUnDepositExcludeCO.Append("	  AND  ");
            _SummaryUnDepositExcludeCO.Append("	(status = 'Pickup' OR status = 'Deposit' OR status = 'Pickup-NC' OR status = 'Pick Up' OR  status ='Payment Plan') ");
            _SummaryUnDepositExcludeCO.Append(" ) ");
            //UnDepositExcludeCO

            //
            _SummaryUnDepositCO =
              " SELECT " +
              "     SUM(amount_dispursed)-SUM(issuer) as amount_dispursed " +
              " FROM (" + DatabaseName + ".transactions) " +
              " WHERE " +
              " ( " +
              "   ( " +
              "    TO_DAYS(trans_date) <= TO_DAYS(@TheDate) " +
              "      AND " +
              "    TO_DAYS(date_cleared)  > TO_DAYS(@TheDate) " +
              "      AND " +
              "   (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default') ) " +
              "   ) " +
              "   OR " +
              "   ( " +
              "    TO_DAYS(trans_date) <= TO_DAYS(@TheDate) " +
              "      AND " +
              "    TO_DAYS(date_returned)  > TO_DAYS(@TheDate) " +
              "      AND " +
              "   (status != 'Void' AND (status = 'Open' OR status = 'Closed' OR status = 'Late' OR status = 'Default' OR status = 'Pd Default') ) " +
              "   ) " +
              " ) " +
              " " +
              "   OR " +
              "   ( 	TO_DAYS(trans_date) <= TO_DAYS(@TheDate) ) " +
              "   AND " +
              "   (status = 'Open' OR status = 'Late' ) ";
  

            //

            //
            _SummaryPaymentsBalance =
                "SELECT SUM(A.balance) AS balance FROM " + 
                "(SELECT  SUM(payments.balance) as balance " +
                "FROM " + DatabaseName + ".transactions JOIN payments on transactions.id = payments.transaction_id " +
                "WHERE payment_type = 'Fee Income' " +
                " AND " +
                " payments.date_paid <= @TheDate " +
                " AND " +
                " transactions.trans_date <=  @TheDate " +
                " AND " +
                " (transactions.date_cleared >  @TheDate OR transactions.date_returned >  @TheDate) " +
                " UNION " +
                "SELECT  SUM(payments.balance) as balance " +
                "FROM " + DatabaseName + ".transactions JOIN payments on transactions.id = payments.transaction_id " +
                "WHERE payment_type = 'Fee Income' " +
                " AND " +
                " payments.date_paid <=  @TheDate " +
                " AND " +
                " transactions.trans_date <=  @TheDate " +
                " AND " +
                " (transactions.status IN ('Open','Late','Payment Plan' ))) as A";
            //

            // CSng(UnDepositExcludeCO) + CSng(UnDepositCO) - CSng(Balance)
            _SummaryReceivablesBalance = "";
            
            //
            _SummaryBounced =
                "SELECT SUM(amount_recieved) as amount_bounced " +
                "FROM " + DatabaseName + ".transactions " +
                "WHERE (TO_DAYS(date_returned) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND status != 'void' ";

            _SummaryPaid =
                "SELECT SUM(amount_paid) as amount_paid " +
                "FROM " + DatabaseName + ".payments " +
                "WHERE (TO_DAYS(date_paid) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate))";

            _SummaryAmountDispursed =
                "SELECT " +
                "   SUM(amount_dispursed) As amount_dispursed " +
                "FROM " + DatabaseName + ".transactions WHERE (TO_DAYS(trans_date) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND (status!='void') AND (status!='Open') AND (status!='Closed') ";

            _SummaryAmountRecieved =
                "SELECT " +
                "   SUM(amount_recieved) As amount_recieved " +
                "FROM " + DatabaseName + ".transactions WHERE (TO_DAYS(trans_date) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND (status!='void') AND (status!='Open') AND (status!='Closed') ";

            _SummaryAmountCardFees =
                "SELECT SUM(total) - SUM(total_due) as amount_paid " +
                "   FROM " + DatabaseName + ".card_transactions " +
                "WHERE (TO_DAYS(datetime) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate))";

            _SummaryAmountCash =
                " SELECT SUM(amount) as cash " +
                " FROM " + CheckbookDatabaseName  + ".cash_log  " +
                "  WHERE (TO_DAYS(date) <= TO_DAYS(@TheDate)) ";

            _SummaryAmountChecking =
                " SELECT SUM(amount) as checking  " +
                " FROM " + CheckbookDatabaseName + ".checkbook   " +
                "  WHERE (TO_DAYS(date_entered) <= TO_DAYS(@TheDate)) ";

            _SummaryAmountTransfers = new StringBuilder("");
            _SummaryAmountTransfers.Append("SELECT SUM(transfers) FROM ( ");
            _SummaryAmountTransfers.Append("SELECT SUM(amount) as transfers ");
            _SummaryAmountTransfers.Append(" FROM " + CheckbookDatabaseName  + ".cash_log ");
            _SummaryAmountTransfers.Append("   WHERE (TO_DAYS(date) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND transaction_type = 'Transfer' ");
            _SummaryAmountTransfers.Append(" UNION " );
            _SummaryAmountTransfers.Append("   SELECT SUM(amount) as transfers ");
            _SummaryAmountTransfers.Append("  FROM " + CheckbookDatabaseName  + ".checkbook ");
            _SummaryAmountTransfers.Append("   WHERE (TO_DAYS(date_entered) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND transaction_type = 'Transfer'");
            _SummaryAmountTransfers.Append(") AS A ");

            _SummaryAmountExpenses = new StringBuilder("");
            _SummaryAmountExpenses.Append("SELECT SUM(expenses) FROM ( ");
            _SummaryAmountExpenses.Append("SELECT SUM(amount) as expenses ");
            _SummaryAmountExpenses.Append(" FROM " + CheckbookDatabaseName + ".cash_log ");
            _SummaryAmountExpenses.Append("  WHERE (TO_DAYS(date) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND transaction_type = 'Expense' ");
            _SummaryAmountExpenses.Append(" UNION ");
            _SummaryAmountExpenses.Append("  SELECT sum(amount) as expenses ");
            _SummaryAmountExpenses.Append(" FROM " + CheckbookDatabaseName + ".checkbook ");
            _SummaryAmountExpenses.Append(" WHERE (TO_DAYS(date_entered) BETWEEN TO_DAYS(@TheDate) AND TO_DAYS(@TheDate)) AND transaction_type = 'Expense' ");
            _SummaryAmountExpenses.Append(") AS A ");
        }
   
    }
}
