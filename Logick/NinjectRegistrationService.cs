using BlackJack.BusinessLogic.Service.Interface;
using Ninject.Modules;
using BlackJack.BusinessLogic.Service;

namespace BlackJack.BusinessLogic
{
    public class NinjectRegistrationService : NinjectModule
    {

        public override void Load()
        {
            Bind<IGameService>().To<GameService>();
        }
    }
}
