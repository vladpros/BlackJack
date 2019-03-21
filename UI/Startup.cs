using System.Web.Optimization;
using System.Web.Routing;
using BlackJack.BusinessLogic;
using BlackJack.DataAccess;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Owin;

[assembly: OwinStartup(typeof(BlackJack.UI.Startup))]

namespace BlackJack.UI
{
    public class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            app.UseNinjectMiddleware(CreateKernel);
        }

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(new NinjectRegistrationRepository(), new NinjectRegistrationService());
            return kernel;
        }
    }
}
