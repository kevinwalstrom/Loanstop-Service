using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

using LoanStop.Entities.CommonTypes;

namespace LoanStop.Services.WebApi.Connections
{
    public class ConnectionsSingleton : IConnectionsSingleton
    {

        private string MASTERCONNECTIONSTRING;

        public List<StoreConnectionType> Items {get; set;}

        public ConnectionsSingleton()
        {
            MASTERCONNECTIONSTRING = ConfigurationManager.ConnectionStrings["master"].ConnectionString;
            GetDatabaseConnections();
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetDatabaseConnections()
        {
            string sql = "SELECT store, ip, database_name, checking_database_name, username, password, port, account_info, store_code, voice_servo_id, text_servo_id, voice_phone_number, text_message, phone_number, location FROM mysql_ips ORDER BY store";

            Items = new List<StoreConnectionType>();

            using (MySqlCommand cmd = new MySqlCommand(sql))
            using (var MasterCnn = new MySqlConnection(MASTERCONNECTIONSTRING))
            {
                MasterCnn.Open();
                cmd.Connection = MasterCnn;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StoreConnectionType StoreConnection = new StoreConnectionType();
                        StoreConnection.StoreName = reader["store"].ToString();
                        StoreConnection.StoreIP = reader["ip"].ToString();
                        StoreConnection.DatabaseName = reader["database_name"].ToString();
                        StoreConnection.CheckbookDatabaseName = reader["checking_database_name"].ToString();
                        StoreConnection.Username = reader["username"].ToString();
                        StoreConnection.Password = reader["password"].ToString();
                        StoreConnection.Port = reader["port"].ToString();
                        StoreConnection.VoiceServoID = reader["voice_servo_id"].ToString();
                        StoreConnection.TextServoID = reader["text_servo_id"].ToString();
                        StoreConnection.VoicePhoneNumber = reader["voice_phone_number"].ToString();
                        StoreConnection.TextPhoneNumber = reader["phone_number"].ToString();
                        StoreConnection.TextMessage = reader["text_message"].ToString();
                        StoreConnection.State = reader["location"].ToString();
                        StoreConnection.AccountInfo = reader["account_info"].ToString();
                        StoreConnection.StoreCode = reader["store_code"].ToString();
                        //if (reader["location"].ToString() == "Colorado")           
                        //    StoreConnection.Email = "<html><head><title>Untitled document</title><style type='text/css'>ol{margin:0;padding:0}.c3{max-width:468pt;background-color:#ffffff;padding:72pt 72pt 72pt 72pt}.c0{font-size:10pt}.c1{direction:ltr}.c2{height:11pt}.title{padding-top:24pt;line-height:1.15;text-align:left;color:#000000;font-size:36pt;font-family:Arial;font-weight:bold;padding-bottom:6pt}.subtitle{padding-top:18pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:24pt;font-family:Georgia;padding-bottom:4pt}p{color:#000000;font-size:11pt;margin:0;font-family:Arial}h1{padding-top:24pt;line-height:1.15;text-align:left;color:#000000;font-size:18pt;font-family:Arial;font-weight:bold;padding-bottom:6pt}h2{padding-top:18pt;line-height:1.15;text-align:left;color:#000000;font-size:14pt;font-family:Arial;font-weight:bold;padding-bottom:4pt}h3{padding-top:14pt;line-height:1.15;text-align:left;color:#666666;font-size:12pt;font-family:Arial;font-weight:bold;padding-bottom:4pt}h4{padding-top:12pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:11pt;font-family:Arial;padding-bottom:2pt}h5{padding-top:11pt;line-height:1.15;text-align:left;color:#666666;font-size:10pt;font-family:Arial;font-weight:bold;padding-bottom:2pt}h6{padding-top:10pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:10pt;font-family:Arial;padding-bottom:2pt}</style></head><body class='c3'><p class='c1'><img height='37' src='images/image00.png' width='151'></p><p class='c1 c2'><span></span></p><p class='c1'><span class='c0'>Hi {name}</span></p><p class='c1'><span class='c0'>This is Loan Stop with an important reminder.</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>You have a payment due on {date}</span></p><p class='c1'><span class='c0'>Your payment is scheduled for {status}</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>If you have any questions or concerns, please call the office at {phone}</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>Please note that reduced payments may be made in cash if you don&#39;t have the full payment. &nbsp;Let us know if you want to take advantage of this option.</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>Sincerely,</span></p><p class='c1'><span class='c0'>&nbsp;</span></p><p class='c1'><span class='c0'>The LoanStop Team</span></p><p class='c1 c2'><span class='c0'></span></p><p class='c1 c2'><span></span></p><p class='c1 c2'><span></span></p></body></html>";
                        //if (reader["location"].ToString() == "Wyoming")
                        StoreConnection.Email = "<html><head><title>Untitled document</title><style type='text/css'>ol{margin:0;padding:0}.c1{max-width:468pt;background-color:#ffffff;padding:72pt 72pt 72pt 72pt}.c2{font-size:10pt}.c3{height:11pt}.c0{direction:ltr}.title{padding-top:24pt;line-height:1.15;text-align:left;color:#000000;font-size:36pt;font-family:Arial;font-weight:bold;padding-bottom:6pt}.subtitle{padding-top:18pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:24pt;font-family:Georgia;padding-bottom:4pt}p{color:#000000;font-size:11pt;margin:0;font-family:Arial}h1{padding-top:24pt;line-height:1.15;text-align:left;color:#000000;font-size:18pt;font-family:Arial;font-weight:bold;padding-bottom:6pt}h2{padding-top:18pt;line-height:1.15;text-align:left;color:#000000;font-size:14pt;font-family:Arial;font-weight:bold;padding-bottom:4pt}h3{padding-top:14pt;line-height:1.15;text-align:left;color:#666666;font-size:12pt;font-family:Arial;font-weight:bold;padding-bottom:4pt}h4{padding-top:12pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:11pt;font-family:Arial;padding-bottom:2pt}h5{padding-top:11pt;line-height:1.15;text-align:left;color:#666666;font-size:10pt;font-family:Arial;font-weight:bold;padding-bottom:2pt}h6{padding-top:10pt;line-height:1.15;text-align:left;color:#666666;font-style:italic;font-size:10pt;font-family:Arial;padding-bottom:2pt}</style></head><body class='c1'><p class='c0'><img height='37' src='images/image00.png' width='151'></p><p class='c3 c0'><span></span></p><p class='c0'><span class='c2'>Hi {name}</span></p><p class='c0'><span class='c2'>This is Loan Stop with an important reminder.</span></p><p class='c0'><span class='c2'>&nbsp;</span></p><p class='c0'><span class='c2'>You have a payment due on {date}</span></p><p class='c0'><span class='c2'>Your payment is scheduled for {status}</span></p><p class='c0'><span class='c2'>&nbsp;</span></p><p class='c0'><span class='c2'>If you have any questions or concerns, please call the office at {phone}</span></p><p class='c0'><span class='c2'>&nbsp;</span></p><p class='c0'><span class='c2'>Sincerely,</span></p><p class='c0'><span class='c2'>&nbsp;</span></p><p class='c0'><span class='c2'>The LoanStop Team</span></p><p class='c0 c3'><span></span></p><p class='c3 c0'><span></span></p></body></html>";
                        ;

                        Items.Add(StoreConnection);
                    }
                    reader.Close();
                }
            }
        }

    }
}