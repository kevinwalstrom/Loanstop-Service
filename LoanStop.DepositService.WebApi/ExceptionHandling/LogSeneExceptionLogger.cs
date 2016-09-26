using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;
using PlainElastic.Net;
using Newtonsoft.Json;

using LoanStop.Entities.logsene;

namespace LoanStop.Services.WebApi.ExceptionHandling
{
    public class LogSeneExceptionLogger : ExceptionLogger
    {

        private const string HttpContextBaseKey = "MS_HttpContext";

        public override void Log(ExceptionLoggerContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            HttpContext httpContext = GetHttpContext(context.Request);

            if (httpContext == null)
            {
                return;
            }

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            Exception exceptionToRaise = new HttpUnhandledException(message: null, innerException: context.Exception);

            string env = string.Empty;
#if DEBUG
            env = "-DEBUG";
#endif

            // Send the exception to LogSene (for logging, mailing, filtering, etc.).
            var connection = new ElasticConnection();
            string command = string.Format("http://logsene-receiver.sematext.com:80/73652425-e371-4697-a221-0507c5aa3d83/general{0}",env);
            string jsonData = JsonConvert.SerializeObject(context.Exception.Message);
            if (context.Exception.InnerException != null)
            {
                jsonData = jsonData + JsonConvert.SerializeObject(context.Exception.InnerException);
            }
            LogSeneEntity logSeneEntity = new LogSeneEntity();
            logSeneEntity.facility = httpContext.Request.Url.ToString();
            logSeneEntity.timestamp = DateTime.Now;
            logSeneEntity.message = jsonData;

            string post = JsonConvert.SerializeObject(logSeneEntity);

            try
            {
                var logsene = connection.Post(command, post);
            }
            catch { }
        }

        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContextBase contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }
}