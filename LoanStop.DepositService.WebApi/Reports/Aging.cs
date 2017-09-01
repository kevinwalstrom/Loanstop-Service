using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanStop.Entities.CommonTypes;
using LoanStop.DBCore.Repository;
using LoanStop.Entities.Export;
using LoanStop.Entities.Reports;

namespace LoanStop.Services.WebApi.Reports
{
    public class Aging
    {
        private List<StoreConnectionType> Connections;
        private Colorado colorado;
        private Wyoming wyoming;

        public Aging(List<StoreConnectionType> connections)
        {
            this.Connections = connections;
        }

        public object Execute()
        {
            var temp = new List<StoryEntity>();
            var rtrn = new List<object>();

            foreach (var connection in Connections)
            {
                var report = new LoanStop.DBCore.Repository.Reports(connection.ConnectionString());

                var aging = report.Aging();

                temp.Add(new StoryEntity() { store = connection.StoreName, records = aging });
            }


            List<int> current30 = new List<int>();
            List<int> current3160 = new List<int>();
            List<int> current6190 = new List<int>();
            List<int> current91120 = new List<int>();
            List<int> current121150 = new List<int>();
            List<int> current151180 = new List<int>();
            List<int> currentGreaterThan180 = new List<int>();

            List<int> defaultCurrent = new List<int>();
            List<int> default30DaysPast = new List<int>();
            List<int> default60DaysPast = new List<int>();
            List<int> default36MonthsPast = new List<int>();
            List<int> default612MonthsPast = new List<int>();
            List<int> default12YearsPast = new List<int>();
            List<int> defaultGreaterThan2YearsPast = new List<int>();

            foreach (var store in temp)
            {
                current30.Add(store.records.Where(record => record.AgingRange == "0-30").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                current3160.Add(store.records.Where(record => record.AgingRange == "31-60").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                current6190.Add(store.records.Where(record => record.AgingRange == "61-90").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                current91120.Add(store.records.Where(record => record.AgingRange == "91-120").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                current121150.Add(store.records.Where(record => record.AgingRange == "121-150").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                current151180.Add(store.records.Where(record => record.AgingRange == "151-180").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                currentGreaterThan180.Add(store.records.Where(record => record.AgingRange == "greater than 180").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());

                defaultCurrent.Add(store.records.Where(record => record.AgingRange == "default current").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                default30DaysPast.Add(store.records.Where(record => record.AgingRange == "30 days past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                default60DaysPast.Add(store.records.Where(record => record.AgingRange == "60 days past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                default36MonthsPast.Add(store.records.Where(record => record.AgingRange == "3 - 6 months past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                default612MonthsPast.Add(store.records.Where(record => record.AgingRange == "6 - 12 months past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                default12YearsPast.Add(store.records.Where(record => record.AgingRange == "1 - 2 years past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());
                defaultGreaterThan2YearsPast.Add(store.records.Where(record => record.AgingRange == "greater than 2 years past").Select((w, i) => w.NumberOfTransactions).FirstOrDefault());

            }

            var rtrnCurrent30 = new
            {
                type = "Current 30 Days",
                arvada = current30[0],
                aurora = current30[1],
                fortCollins = current30[2],
                grandJunction = current30[3],
                greeley = current30[4],
                littleton = current30[5],
                pueblo = current30[6],
                southDenver = current30[7],
                thornton = current30[8]
            };

            rtrn.Add(rtrnCurrent30);

            var rtrCurrent3160 = new
            {
                type = "Current 31-90",
                arvada = current3160[0],
                aurora = current3160[1],
                fortCollins = current3160[2],
                grandJunction = current3160[3],
                greeley = current3160[4],
                littleton = current3160[5],
                pueblo = current3160[6],
                southDenver = current3160[7],
                thornton = current3160[8]
            };

            rtrn.Add(rtrCurrent3160);

            var rtrCurrent6190 = new
            {
                type = "Current 31-90",
                arvada = current6190[0],
                aurora = current6190[1],
                fortCollins = current6190[2],
                grandJunction = current6190[3],
                greeley = current6190[4],
                littleton = current6190[5],
                pueblo = current6190[6],
                southDenver = current6190[7],
                thornton = current6190[8]
            };

            rtrn.Add(rtrCurrent3160);

            var rtrCurrent91120 = new
            {
                type = "Current 91-120",
                arvada = current91120[0],
                aurora = current91120[1],
                fortCollins = current91120[2],
                grandJunction = current91120[3],
                greeley = current91120[4],
                littleton = current91120[5],
                pueblo = current91120[6],
                southDenver = current91120[7],
                thornton = current91120[8]
            };

            rtrn.Add(rtrCurrent91120);

            var rtrCurrent121150 = new
            {
                type = "Current 121-150",
                arvada = current121150[0],
                aurora = current121150[1],
                fortCollins = current121150[2],
                grandJunction = current121150[3],
                greeley = current121150[4],
                littleton = current121150[5],
                pueblo = current121150[6],
                southDenver = current121150[7],
                thornton = current121150[8]
            };

            rtrn.Add(rtrCurrent121150);

            var rtrnCurrent151180 = new
            {
                type = "Current 151-180",
                arvada = current151180[0],
                aurora = current151180[1],
                fortCollins = current151180[2],
                grandJunction = current151180[3],
                greeley = current151180[4],
                littleton = current151180[5],
                pueblo = current151180[6],
                southDenver = current151180[7],
                thornton = current151180[8]
            };

            rtrn.Add(rtrnCurrent151180);

            var rtrnCurrentGreaterThan180 = new
            {
                type = "Current greater than 180",
                arvada = currentGreaterThan180[0],
                aurora = currentGreaterThan180[1],
                fortCollins = currentGreaterThan180[2],
                grandJunction = currentGreaterThan180[3],
                greeley = currentGreaterThan180[4],
                littleton = currentGreaterThan180[5],
                pueblo = currentGreaterThan180[6],
                southDenver = currentGreaterThan180[7],
                thornton = currentGreaterThan180[8]
            };

            rtrn.Add(rtrnCurrentGreaterThan180);


            var rtrnDefaultCurrent = new
            {
                type = "Default Current",
                arvada = defaultCurrent[0],
                aurora = defaultCurrent[1],
                fortCollins = defaultCurrent[2],
                grandJunction = defaultCurrent[3],
                greeley = defaultCurrent[4],
                littleton = defaultCurrent[5],
                pueblo = defaultCurrent[6],
                southDenver = defaultCurrent[7],
                thornton = defaultCurrent[8]
            };

            rtrn.Add(rtrnDefaultCurrent);

            var rtrndefault30DaysPast = new
            {
                type = "30 days past",
                arvada = default30DaysPast[0],
                aurora = default30DaysPast[1],
                fortCollins = default30DaysPast[2],
                grandJunction = default30DaysPast[3],
                greeley = default30DaysPast[4],
                littleton = default30DaysPast[5],
                pueblo = default30DaysPast[6],
                southDenver = default30DaysPast[7],
                thornton = default30DaysPast[8]
            };

            rtrn.Add(rtrndefault30DaysPast);

            var rtrndefault60DaysPast = new
            {
                type = "60 days past",
                arvada = default60DaysPast[0],
                aurora = default60DaysPast[1],
                fortCollins = default60DaysPast[2],
                grandJunction = default60DaysPast[3],
                greeley = default60DaysPast[4],
                littleton = default60DaysPast[5],
                pueblo = default60DaysPast[6],
                southDenver = default60DaysPast[7],
                thornton = default60DaysPast[8]
            };

            rtrn.Add(rtrndefault60DaysPast);

            var rtrndefault36MonthsPast = new
            {
                type = "3-6 months past",
                arvada = default36MonthsPast[0],
                aurora = default36MonthsPast[1],
                fortCollins = default36MonthsPast[2],
                grandJunction = default36MonthsPast[3],
                greeley = default36MonthsPast[4],
                littleton = default36MonthsPast[5],
                pueblo = default36MonthsPast[6],
                southDenver = default36MonthsPast[7],
                thornton = default36MonthsPast[8]
            };

            rtrn.Add(rtrndefault36MonthsPast);

            var rtrnDefault612MonthsPast = new
            {
                type = "6-12 months past",
                arvada = default612MonthsPast[0],
                aurora = default612MonthsPast[1],
                fortCollins = default612MonthsPast[2],
                grandJunction = default612MonthsPast[3],
                greeley = default612MonthsPast[4],
                littleton = default612MonthsPast[5],
                pueblo = default612MonthsPast[6],
                southDenver = default612MonthsPast[7],
                thornton = default612MonthsPast[8]
            };

            rtrn.Add(rtrnDefault612MonthsPast);

            var rtrnDefault12YearsPast = new
            {
                type = "1-2 years past",
                arvada = default12YearsPast[0],
                aurora = default12YearsPast[1],
                fortCollins = default12YearsPast[2],
                grandJunction = default12YearsPast[3],
                greeley = default12YearsPast[4],
                littleton = default12YearsPast[5],
                pueblo = default12YearsPast[6],
                southDenver = default12YearsPast[7],
                thornton = default12YearsPast[8]
            };

            rtrn.Add(rtrnDefault12YearsPast);

            var rtrndefaultGreaterThan2YearsPast = new
            {
                type = "greater than 2 years past",
                arvada = defaultGreaterThan2YearsPast[0],
                aurora = defaultGreaterThan2YearsPast[1],
                fortCollins = defaultGreaterThan2YearsPast[2],
                grandJunction = defaultGreaterThan2YearsPast[3],
                greeley = defaultGreaterThan2YearsPast[4],
                littleton = defaultGreaterThan2YearsPast[5],
                pueblo = defaultGreaterThan2YearsPast[6],
                southDenver = defaultGreaterThan2YearsPast[7],
                thornton = defaultGreaterThan2YearsPast[8]
            };

            rtrn.Add(rtrndefaultGreaterThan2YearsPast);

            return rtrn;

        }

    }

    public class StoryEntity
    {
        public string store;
        public List<AgingEntity> records;
    }


}