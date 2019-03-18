using BlackJack.BusinessLogic;
using BlackJack.DataAccess;
using BlackJack.DataAccess.Repositories.EF;
using BlackJack.DataAccess.Repositories.Interfaces;
using BlackJack.DataAccess.Repositories.Dapper;
using BlackJack.BusinessLogic.Interfaces;
using Ninject.Modules;


namespace BlackJack.BusinessLogic.Utils
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
                Bind<IGameRepository>().To<DapperGameRepository>();
                Bind<IGameResultRepository>().To<DapperGameResultRepository>();
                Bind<IPlayerRepository>().To<DapperPlayerRepository>();
                Bind<ITurnRepository>().To<DapperTurnRepository>();

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
