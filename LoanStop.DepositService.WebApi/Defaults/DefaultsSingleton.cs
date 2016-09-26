using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Practices.Unity;

using MySql.Data.MySqlClient;

using LoanStop.Entities.CommonTypes;
using LoanStop.Services.WebApi.Connections;

namespace LoanStop.Services.WebApi.Defaults
{

    public class DefaultsSingleton : IDefaultsSingleton
    {
        private IConnectionsSingleton storeConnections;

        public List<DefaultType> Items { get; set; }

        public DefaultsSingleton(IConnectionsSingleton storeConnections)
        {
            this.storeConnections = storeConnections;
            
            Items = new List<DefaultType>();

            GetDefaults();
        }
       
        private void GetDefaults()
        {

            foreach (StoreConnectionType store in storeConnections.Items)
            {
                var newItem = new DefaultType();

                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM defaults"))
                using (var storeCnn = new MySqlConnection(store.ConnectionString()))
                {
                    
                    storeCnn.Open();
                    cmd.Connection = storeCnn;

                    newItem.Store = store.DatabaseName;
                    newItem.ConnectionString = store.ConnectionString();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch (reader["Name"].ToString())
                            {
                                case "number_of_rollovers":
                                    newItem.NumberOfRollovers = int.Parse(reader["value"].ToString());
                                    break;
                                case "allow_rollover":
                                    newItem.AllowRollover = bool.Parse(reader["value"].ToString());
                                    break;
                                case "rebates":
                                    newItem.Rebates = bool.Parse(reader["value"].ToString());
                                    break;
                                case "check_fee":
                                    newItem.CheckFee = int.Parse(reader["value"].ToString());
                                    break;
                                case "1 week":
                                    newItem.OneWeek = int.Parse(reader["value"].ToString());
                                    break;
                                case "2 week":
                                    newItem.TwoWeek = int.Parse(reader["value"].ToString());
                                    break;
                                case "3 week":
                                    newItem.ThreeWeek = int.Parse(reader["value"].ToString());
                                    break;
                                case "4 week":
                                    newItem.FourWeek = int.Parse(reader["value"].ToString());
                                    break;
                                case "wyo":
                                    if (reader["Value"].ToString().ToUpper() == "TRUE")
                                        newItem.Wyoming = true;
                                    else
                                        newItem.Wyoming = false;
                                    break;
                                case "br_address":
                                    newItem.BRAddress = reader["value"].ToString();
                                    break;
                                case "city_st_zip":
                                    newItem.CityStateZip = reader["value"].ToString();
                                    break;
                                case "phone":
                                    newItem.Phone = reader["value"].ToString();
                                    break;
                                case "experation":
                                    newItem.Experation = reader["value"].ToString();
                                    break;
                                case "max_limit":
                                    newItem.MaxLimit = int.Parse(reader["value"].ToString());
                                    break;
                                case "disclosure":
                                    newItem.Disclosure = reader["value"].ToString();
                                    break;
                                case "disclosure_pages":
                                    newItem.DisclosurePages = reader["value"].ToString();
                                    break;
                                case "cash_dispersed":
                                    newItem.CashDispersion = reader["value"].ToString();
                                    break;
                                case "cash_cushion":
                                    newItem.CashCushion = reader["value"].ToString();
                                    break;
                                case "mc_fee":
                                    newItem.MCFee = reader["value"].ToString();
                                    break;
                                case "mc_cost":
                                    newItem.MCCos = reader["value"].ToString();
                                    break;
                                case "reload_fee":
                                    newItem.ReloadFee = reader["value"].ToString();
                                    break;
                                case "reload_cost":
                                newItem.ReloadCost = reader["value"].ToString();
                                    break;
                                case "international_fee":
                                    newItem.InternationalFee = reader["value"].ToString();
                                    break;
                                case "international_cost":
                                    newItem.InternationalCost = reader["value"].ToString();
                                    break;
                                case "check_cash_cushion":
                                    newItem.CheckCashCushion = reader["value"].ToString();
                                    break;
                                case "colorado_payment_plans":
                                    if (reader["value"].ToString().ToUpper() == "TRUE")
                                        newItem.ColoradoPaymentPlans = true;
                                    else
                                        newItem.ColoradoPaymentPlans = false;
                                    break;
                                case "wyoming_payment_plans":
                                    if (reader["value"].ToString().ToUpper() == "TRUE")
                                        newItem.WyomingPaymentPlans = true;
                                    else
                                        newItem.WyomingPaymentPlans = false;
                                    break;
                                case "money_order_fee":
                                    newItem.MoneyOrderFee = reader["value"].ToString();
                                    break;
                                case "money_order_cost":
                                    newItem.MoneyOrderCost = reader["value"].ToString();
                                    break;
                                case "Weekday Hours":
                                    newItem.WeekdayHours = reader["value"].ToString();
                                    break;
                                case "Saturday Hours":
                                    newItem.SaturdayHours = reader["value"].ToString();
                                    break;
                                case "min500":
                                    newItem.Min500Payment = reader["value"].ToString();
                                    break;
                                case "min400":
                                    newItem.Min400Payment = reader["value"].ToString();
                                    break;
                                case "min300":
                                    newItem.Min300Payment = reader["value"].ToString();
                                    break;
                                case "min200":
                                    newItem.Min200Payment = reader["value"].ToString();
                                    break;
                                case "min100":
                                    newItem.Min100Payment = reader["value"].ToString();
                                    break;
                                case "receipt":
                                    newItem.ReceiptPrinter = reader["value"].ToString();
                                    break;
                                case "receipt printer name":
                                    newItem.ReceiptPrinterName = reader["value"].ToString();
                                    break;
                                case "store_code":
                                    newItem.StoreCode = reader["value"].ToString();
                                    break;
                                case "teletrack score":
                                    newItem.TeleTrackScore = int.Parse(reader["value"].ToString());
                                    break;
                                case "teletrack username":
                                    newItem.TeleTrackUserName = reader["value"].ToString();
                                    break;
                                case "teletrack subscriber id":
                                    newItem.TeleTrackSubscriberID = reader["value"].ToString();
                                    break;
                                case "teletrack password":
                                    newItem.TeleTrackPassword = reader["value"].ToString();
                                    break;
                                case "payroll_business":
                                    newItem.RatePayrollBusiness = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "payroll_hand_written":
                                    newItem.RatePayrollHandWritten = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "personal":
                                    newItem.RatePersonal = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "money_order":
                                    newItem.RateMoneyOrder = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "government":
                                    newItem.RateGovernment = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "cashier":
                                    newItem.RateCashier = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "no_fee":
                                    newItem.RateNoFee = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "gold":
                                    newItem.Gold = decimal.Parse(reader["value"].ToString());
                                    break;
                                case "ACH Works LocId":
                                    newItem.ACHWorksLocId = reader["value"].ToString();
                                    break;
                                case "ACH Works CompanyId":
                                    newItem.ACHWorksCompanyId = reader["value"].ToString();
                                    break;
                                case "ACH Works CompanyKey":
                                    newItem.ACHWorksCompanyKey = reader["value"].ToString();
                                    break;
                            
                            }
                        }
                        reader.Close();

                    }
                    Items.Add(newItem);
                }
            }

        }

    }


}