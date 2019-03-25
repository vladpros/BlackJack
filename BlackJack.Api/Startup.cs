using System.Web.Http;
using BlackJack.BusinessLogic;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

[assembly: OwinStartup(typeof(BlackJack.Api.Startup))]

namespace BlackJack.Api
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = CreateHttpConfiguration();
            webApiConfiguration
                        .Formatters
                        .JsonFormatter
                        .SerializerSettings
                        .ContractResolver = new CamelCasePropertyNamesContractResolver();
            webApiConfiguration
                        .Formatters
                        .JsonFormatter
                        .SerializerSettings
                        .NullValueHandling = NullValueHandling.Ignore;
            app.UseNinjectMiddleware(CreateKernel).UseNinjectWebApi(webApiConfiguration);
        }

        public static HttpConfiguration CreateHttpConfiguration()
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            return httpConfiguration;
        }

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new NinjectRepositoryRegistration(), new NinjectServiceRegistration());
            return kernel;
        }
    }
}
