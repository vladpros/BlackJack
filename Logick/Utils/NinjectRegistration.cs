using BlackJack.BusinessLogic;
using BlackJack.DataAccess;
using BlackJack.DataAccess.Repository;
using BlackJack.DataAccess.Repository.Interface;
using DataBaseControl.Repository.Dapper;
using Logick.Interfases;
using Ninject.Modules;


namespace Logick.Utils
{
    public class NinjectRegistration : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataService>().To<DataService>();
            Bind<IGameService>().To<GameService>();

            bool useDapper = true;
            if (useDapper)
            {
                Bind<IGameRepository>().To<DapGameRepository>();
                Bind<IGameResultRepository>().To<DapGameResultRepository>();
                Bind<IPlayerRepository>().To<DapPlayerRepository>();
                Bind<ITurnRepository>().To<DapTurnRepository>();

                return;
            }

            Bind<IGameRepository>().To<GameRepository>();
            Bind<IGameResultRepository>().To<GameResultRepository>();
            Bind<IPlayerRepository>().To<PlayerRepository>();
            Bind<ITurnRepository>().To<TurnRepository>();
            Bind<BlackJackContext>().To<BlackJackContext>();
        }
    }
}
