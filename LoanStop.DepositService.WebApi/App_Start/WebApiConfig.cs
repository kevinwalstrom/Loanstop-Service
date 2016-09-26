using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Http;
using System.Web.Http.ExceptionHandling;

using LoanStop.Services.WebApi.ExceptionHandling;


namespace LoanStop.Services.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors();
            
            // enable attribute routing
            config.MapHttpAttributeRoutes();

            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new LogSeneExceptionLogger());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{state}/{store}/{queryDate}"
            );

            config.Routes.MapHttpRoute(
                name: "Client", 
                routeTemplate: "api/{controller}/{verifyId}",
                defaults: new { verifyId = RouteParameter.Optional}
            );

            //ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            //builder.EntitySet<TransactionEntity>("TransactionEntities");


            //IList<IODataRoutingConvention> conventions = ODataRoutingConventions.CreateDefault();
            //conventions.Insert(0, new NonBindableActionRoutingConvention("NonBindableActions"));

            //config.Routes.MapODataRoute(
            //    routeName: "odata",
            //    routePrefix: "odata",
            //    model: ModelBuilder.GetEdmModel(),
            //    pathHandler: new DefaultODataPathHandler(),
            //    routingConventions: conventions);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 
        }
    }
}
