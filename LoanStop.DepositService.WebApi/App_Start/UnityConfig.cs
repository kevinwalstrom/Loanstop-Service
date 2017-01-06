
using System.Web.Http;
using Unity.WebApi;
using LoanStop.Services.WebApi.Defaults;
using LoanStop.Services.WebApi.Connections;
using Microsoft.Practices.Unity;


namespace LoanStop.Services.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            var connectionsSingleton = new ConnectionsSingleton();
            container.RegisterInstance<IConnectionsSingleton>(connectionsSingleton);

            var defaultsSingleton = new DefaultsSingleton(connectionsSingleton);
            container.RegisterInstance<IDefaultsSingleton>(defaultsSingleton);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}