using Logick.Utils;
using Ninject.Modules;

namespace BlackJack.UI.Utils
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            NinjectRegistrationServices ninjectRegistrationServices = new NinjectRegistrationServices();
        }
    }
}