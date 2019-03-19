using BlackJack.DataAccess;
using BlackJack.DataAccess.Repositories.Dapper;
using BlackJack.DataAccess.Repositories.EF;
using BlackJack.DataAccess.Repositories.Interfaces;
using Ninject.Modules;
using System.Configuration;

namespace DataBaseControl.Util
{
    public class NinjectRegistrationRepository : NinjectModule
    {
        public override void Load()
        {
            bool useDapper = true;
            if (useDapper)
            {
                string conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

                Bind<IGameRepository>().To<DapperGameRepository>().WithConstructorArgument("conString", conString);
                Bind<IGameResultRepository>().To<DapperGameResultRepository>().WithConstructorArgument("conString", conString);
                Bind<IPlayerRepository>().To<DapperPlayerRepository>().WithConstructorArgument("conString", conString);
                Bind<ITurnRepository>().To<DapperTurnRepository>().WithConstructorArgument("conString", conString);

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

