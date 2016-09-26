using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LoanStop.DailyBalance;

namespace TestSummaryDefault
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            listView1.Columns.Add("Store", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("UnDepositExcludeCO", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("UnDepositCO", -2, HorizontalAlignment.Right);
            //listView1.Columns.Add("Balance", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("Bounced", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("Paid", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountDispursed", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountRecieved", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountCardFees", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountCash", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountChecking", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("Not Sure", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountTransfers", -2, HorizontalAlignment.Right);
            listView1.Columns.Add("AmountExpenses", -2, HorizontalAlignment.Right);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Report.ReportDate = DateTime.Now.ToString("yyyy-MM-dd");
            Report.RunReport();

            foreach (Store CurrentStore in Report.Stores)
            {

                string[] lvi = new string[CurrentStore.SummaryDOs.Count + 1];

                string str = CurrentStore.GetSummaryResult();
                lvi[0] = CurrentStore.StoreName;
                int count = 1;
                foreach (DO Total in CurrentStore.SummaryDOs)
                {
                    lvi[count] = Total.Total.ToString("#####0.00");
                    count++;
                }

                listView1.Items.Add(new ListViewItem(lvi));
            }

        }
    }
}
