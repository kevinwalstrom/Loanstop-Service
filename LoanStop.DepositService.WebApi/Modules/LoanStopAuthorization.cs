using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Reflection;

namespace LoanStop.Services.WebApi
{

    public class LoanStopAuthorization : IHttpModule
    {

        private string ACCESS_KEY = ConfigurationManager.AppSettings["AccessKey"];
        
        public void Dispose() { }
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (new EventHandler(this.Authenticate));
        }

        private void Authenticate(object sender, EventArgs e)
        {
            var valid = false;
            
            HttpRequest request = ((HttpApplication) sender).Request;

            var accessKey = request.Headers["access-key"]; 

            if (accessKey != null)
            {
                if (IsRegistered(accessKey) == 0)
                {
                    valid = true;
                }
                else
                {
                    valid = false;
                }
            }
            else
            {
                valid = false;
            }

            if (!valid)
            {
                var Response = ((HttpApplication) sender).Response;

                Response.Clear();
                Response.ClearHeaders();

                Response.StatusCode = 200;
                Response.StatusDescription = "Not Authorized";

                Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        private int IsRegistered(string accessKey)
        {
            return string.Compare(ACCESS_KEY, accessKey);
        }
    }
}
