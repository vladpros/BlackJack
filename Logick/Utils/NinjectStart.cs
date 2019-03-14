using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;

namespace Logick.Utils
{
    public class NinjectStart
    {
        public NinjectStart()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
