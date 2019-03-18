using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using System.Web.Mvc;

namespace BlackJack.BusinessLogic.Utils
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
