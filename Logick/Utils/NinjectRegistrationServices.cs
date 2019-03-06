using BlackJack.BusinessLogic;
using BlackJack.DataBaseAccess.Utils;
using Logick.Interfases;
using Ninject.Modules;


namespace Logick.Utils
{
    public class NinjectRegistrationServices : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataService>().To<DataService>();
            Bind<IGameService>().To<GameService>();

            NinjectRegistrationRepository q = new NinjectRegistrationRepository();
        }
    }
}
