using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using LoanStop.Entities.Reports;

namespace LoanStop.Services.WebApi.Reports
{
    public class SumColoradoStores
    {


        public ColoradoYearlyReport Execute(List<ColoradoYearlyReport> list)
        {
            var rtrn = new ColoradoYearlyReport();


            rtrn.totalsLoans = this.TotalLoans(list);                   // question 1
            rtrn.byAmount = this.ByAmount(list);                        // question 2
            rtrn.loansOutstanding = this.LoansOutstanding(list);        // question 3
            rtrn.rescinded = this.Rescinded(list);                      // question 4
            rtrn.activeMilitary = this.ActiveMilitary(list);            // question 6
            rtrn.defaulted = this.Defaulted(list);                      // question 7
            rtrn.amountRecovered = this.AmountRecovered(list);          // question 7
            rtrn.amountchargeOffs = this.ChargeOffs(list);              // question 7
            rtrn.nsfFees = this.NSFFees(list);                          // question 7
            rtrn.bySSNumber = this.BySSNumber(list);                    // question 8
            //rtrn.loansPerCustomer = this.CustomersByNumberLoans(list);  // question 8
            rtrn.question9 = this.Question9(list);                      // question 9 
            rtrn.question10 = this.Question10(list);                    // question 10 

            return rtrn;

        }

        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected TotalsLoans TotalLoans(List<ColoradoYearlyReport> list)
        {
            var rtrn = new TotalsLoans();

            rtrn.count = list.Sum(s => s.totalsLoans.count);
            rtrn.sum = list.Sum(s => s.totalsLoans.sum);

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected List<ByAmount> ByAmount(List<ColoradoYearlyReport> list)
        {
            var rtrn = new List<ByAmount>();

            var newByAmount1 = new ByAmount()
            {
                range = "< 300",
                count = list.Sum(s => s.byAmount[0].count),
                amount = list.Sum(s => s.byAmount[0].amount)
            };
            rtrn.Add(newByAmount1);

            var newByAmount2 = new ByAmount()
            {
                range = "> 300",
                count = list.Sum(s => s.byAmount[1].count),
                amount = list.Sum(s => s.byAmount[1].amount)
            };
            rtrn.Add(newByAmount2);

            return rtrn;
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected List<LoansOutstanding> LoansOutstanding(List<ColoradoYearlyReport> list)
        {
            var rtrn = new List<LoansOutstanding>();

            var newLoansOutstanding = new LoansOutstanding()
            {
                count = list.Sum(s => s.loansOutstanding[0].count),
                amount = list.Sum(s => s.loansOutstanding[0].amount)
            };
            rtrn.Add(newLoansOutstanding);

            return rtrn;
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected Rescinded Rescinded(List<ColoradoYearlyReport> list)
        {
            var rtrn = new Rescinded()
            {
                amount = list.Sum(s => s.rescinded.amount),
                count = list.Sum(s => s.rescinded.count)
            };
            return rtrn;
        }

        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected ActiveMilitary ActiveMilitary(List<ColoradoYearlyReport> list)
        {
            var rtrn = new ActiveMilitary()
            {
                count = list.Sum(s => s.activeMilitary.count),
                amount = list.Sum(s => s.activeMilitary.amount)
            };

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected Defaulted Defaulted(List<ColoradoYearlyReport> list)
        {
            var rtrn = new Defaulted()
            {
                count = list.Sum(s => s.defaulted.count),
                amount = list.Sum(s => s.defaulted.amount)
            };

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected AmountRecovered AmountRecovered(List<ColoradoYearlyReport> list)
        {
            var rtrn = new AmountRecovered()
            {
                count = list.Sum(s => s.amountRecovered.count),
                amountPaid = list.Sum(s => s.amountRecovered.amountPaid)
            };

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected AmountchargeOffs ChargeOffs(List<ColoradoYearlyReport> list)
        {
            var rtrn = new AmountchargeOffs()
            {
                num = list.Sum(s => s.amountchargeOffs.num),
                amountRecieved = list.Sum(s => s.amountchargeOffs.amountRecieved)
            };

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected NsfFees NSFFees(List<ColoradoYearlyReport> list)
        {

            var rtrn = new NsfFees()
            {
                count = list.Sum(s => s.nsfFees.count),
                amountPaid = list.Sum(s => s.nsfFees.amountPaid)
            };

            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected BySSNumber BySSNumber(List<ColoradoYearlyReport> list)
        {

            var rtrn = new BySSNumber()
            {
                total = list.Sum(s => s.bySSNumber.total),
                a = list.Sum(s => s.bySSNumber.a),
                b = list.Sum(s => s.bySSNumber.b),
                c = list.Sum(s => s.bySSNumber.c)
            };

            return rtrn;
        }



        // /////////////////////////////////////////////////////////////////////////////////////////////
        //protected LoansPerCustomer CustomersByNumberLoans(List<ColoradoYearlyReport> list)
        //{
        //    var rtrn = new LoansPerCustomer()
        //    {
        //        count = list.Sum(s => s.loansPerCustomer.count),
        //        range = list.Sum(s => s.loansPerCustomer.range)
        //    };

        //    return rtrn;
        //}


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected Question9 Question9(List<ColoradoYearlyReport> list)
        {
            var question9partA = new Question9partA()
            {
                a = new A
                {
                    amount = Convert.ToInt32(list.Average(s =>s.question9.question9partA.a.amount)),
                    financeCharge = list.Average(s => s.question9.question9partA.a.financeCharge)
                },
                i = new I
                {
                    financeCharge = list.Average(s => s.question9.question9partA.i.financeCharge),
                },
                ii = new Ii
                {
                    financeCharge = list.Average(s => s.question9.question9partA.ii.financeCharge),
                },
                iii = new Iii
                {
                    financeCharge = list.Average(s => s.question9.question9partA.iii.financeCharge)
                }
            };

            var question9partB = new Question9partB()
            {
                a = new A2
                {
                    amount = Convert.ToInt32(list.Average(s => s.question9.question9partB.a.amount)),
                    financeCharge = list.Average(s => s.question9.question9partB.a.financeCharge)
                },
                i = new I2
                {
                    financeCharge = list.Average(s => s.question9.question9partB.i.financeCharge),
                },
                ii = new Ii2
                {
                    financeCharge = list.Average(s => s.question9.question9partB.ii.financeCharge),
                },
                iii = new Iii2
                {
                    financeCharge = list.Average(s => s.question9.question9partB.iii.financeCharge)
                }
            };

            var question9partC = new Question9partC()
            {
                a = new A3
                {
                    amount = Convert.ToInt32(list.Average(s => s.question9.question9partC.a.amount)),
                    financeCharge = list.Average(s => s.question9.question9partC.a.financeCharge)
                },
                i = new I3
                {
                    financeCharge = list.Average(s => s.question9.question9partC.i.financeCharge),
                },
                ii = new Ii3
                {
                    financeCharge = list.Average(s => s.question9.question9partC.ii.financeCharge),
                },
                iii = new Iii3
                {
                    financeCharge = list.Average(s => s.question9.question9partC.iii.financeCharge)
                }
            };

            var question9partD = new Question9partD()
            {
                averageAPR = list.Average(s => s.question9.question9partD.averageAPR)
            };

            var question9partE = new Question9partE()
            {
                averageAPR = list.Average(s => s.question9.question9partE.averageAPR)
            };

            var rtrn = new Question9()
            {
                question9partA = question9partA,
                question9partB = question9partB,
                question9partC = question9partC,
                question9partD = question9partD,
                question9partE = question9partE,
                question9partF = list.Average(s => s.question9.question9partF),
                question9partG = list.Average(s => s.question9.question9partG),
                question9partH = 184
            };



            return rtrn;
        }


        // /////////////////////////////////////////////////////////////////////////////////////////////
        protected Question10 Question10(List<ColoradoYearlyReport> list)
        {
            var rtrn = new Question10()
            {
                total = list.Sum(s => s.question10.total),
                partA = list.Sum(s => s.question10.partA),
                partB = list.Sum(s => s.question10.partB),
                partC = list.Sum(s => s.question10.partC),
                partD = list.Sum(s => s.question10.partD),
                partE = list.Sum(s => s.question10.partE),
                partF = list.Sum(s => s.question10.partF)
            };

            return rtrn;
        }



    }
}