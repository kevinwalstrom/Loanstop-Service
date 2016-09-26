using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanStop.Entities.Reports
{

    public class TotalsLoans
    {
        public int count { get; set; }
        public int sum { get; set; }
    }

    public class ByAmount
    {
        public string range { get; set; }
        public int count { get; set; }
        public int amount { get; set; }
    }

    public class LoansOutstanding
    {
        public int count { get; set; }
        public int amount { get; set; }
    }

    public class Rescinded
    {
        public int amount { get; set; }
        public int count { get; set; }
    }

    public class ActiveMilitary
    {
        public int count { get; set; }
        public int amount { get; set; }
    }

    public class Defaulted
    {
        public int count { get; set; }
        public int amount { get; set; }
    }

    public class AmountRecovered
    {
        public int count { get; set; }
        public double amountPaid { get; set; }
    }

    public class AmountchargeOffs
    {
        public int num { get; set; }
        public double amountRecieved { get; set; }
    }

    public class NsfFees
    {
        public int count { get; set; }
        public int amountPaid { get; set; }
    }

    public class BySSNumber
    {
        public int total { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }

    public class LoansPerCustomer
    {
        public int count { get; set; }
        public string range { get; set; }
    }

    public class A
    {
        public decimal financeCharge { get; set; }
        public int amount { get; set; }
    }

    public class I
    {
        public decimal financeCharge { get; set; }
    }

    public class Ii
    {
        public decimal financeCharge { get; set; }
    }

    public class Iii
    {
        public decimal financeCharge { get; set; }
    }

    public class Question9partA
    {
        public A a { get; set; }
        public I i { get; set; }
        public Ii ii { get; set; }
        public Iii iii { get; set; }
    }

    public class A2
    {
        public decimal financeCharge { get; set; }
        public decimal amount { get; set; }
    }

    public class I2
    {
        public decimal financeCharge { get; set; }
    }

    public class Ii2
    {
        public decimal financeCharge { get; set; }
    }

    public class Iii2
    {
        public decimal financeCharge { get; set; }
    }

    public class Question9partB
    {
        public A2 a { get; set; }
        public I2 i { get; set; }
        public Ii2 ii { get; set; }
        public Iii2 iii { get; set; }
    }

    public class A3
    {
        public decimal financeCharge { get; set; }
        public decimal amount { get; set; }
    }

    public class I3
    {
        public decimal financeCharge { get; set; }
    }

    public class Ii3
    {
        public decimal financeCharge { get; set; }
    }

    public class Iii3
    {
        public decimal financeCharge { get; set; }
    }

    public class Question9partC
    {
        public A3 a { get; set; }
        public I3 i { get; set; }
        public Ii3 ii { get; set; }
        public Iii3 iii { get; set; }
    }

    public class Question9partD
    {
        public decimal averageAPR { get; set; }
    }

    public class Question9partE
    {
        public decimal averageAPR { get; set; }
    }

    public class Question9
    {
        public Question9partA question9partA { get; set; }
        public Question9partB question9partB { get; set; }
        public Question9partC question9partC { get; set; }
        public Question9partD question9partD { get; set; }
        public Question9partE question9partE { get; set; }
        public decimal question9partF { get; set; }
        public decimal question9partG { get; set; }
        public int question9partH { get; set; }
    }

    public class Question10
    {
        public int total { get; set; }
        public int partA { get; set; }
        public int partB { get; set; }
        public int partC { get; set; }
        public int partD { get; set; }
        public int partE { get; set; }
        public int partF { get; set; }
    }

    public class ColoradoYearlyReport
    {
        public TotalsLoans totalsLoans { get; set; }
        public List<ByAmount> byAmount { get; set; }
        public List<LoansOutstanding> loansOutstanding { get; set; }
        public Rescinded rescinded { get; set; }
        public ActiveMilitary activeMilitary { get; set; }
        public Defaulted defaulted { get; set; }
        public AmountRecovered amountRecovered { get; set; }
        public AmountchargeOffs amountchargeOffs { get; set; }
        public NsfFees nsfFees { get; set; }
        public BySSNumber bySSNumber { get; set; }
        public LoansPerCustomer loansPerCustomer { get; set; }
        public Question9 question9 { get; set; }
        public Question10 question10 { get; set; }
    }

    public class RootObject
    {
        public bool ResponseError { get; set; }
        public object Message { get; set; }
        public object Status { get; set; }
        public object Entity { get; set; }
        //public Item Item { get; set; }
    }
}
