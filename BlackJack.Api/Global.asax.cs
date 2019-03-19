using BlackJack.BusinessLogic.Utils;
using DataBaseControl.Util;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BlackJack.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
