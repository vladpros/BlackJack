using Ninject;
using Ninject.Modules;

using Ninject.Web.Mvc;

namespace Logick.Utils
{
    public class NinjectStart
    {
        public NinjectStart()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            System.Web.Mvc.DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
