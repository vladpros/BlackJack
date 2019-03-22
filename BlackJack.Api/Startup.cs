using System.Web.Http;
using BlackJack.BusinessLogic;
using BlackJack.DataAccess;
using Microsoft.Owin;
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
