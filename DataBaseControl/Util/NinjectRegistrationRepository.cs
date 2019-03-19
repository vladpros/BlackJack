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
            bool useDapper = false;
            if (useDapper)
            {
                string conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

                Bind<IGameRepository>().To<DapperGameRepository>().WithConstructorArgument("conString", conString);
                Bind<IGameResultRepository>().To<DapperGameResultRepository>().WithConstructorArgument("conString", conString);
                Bind<IPlayerRepository>().To<DapperPlayerRepository>().WithConstructorArgument("conString", conString);
                Bind<ITurnRepository>().To<DapperTurnRepository>().WithConstructorArgument("conString", conString);

                return;
            }

            BlackJackContext blackJackContext = new BlackJackContext();

            Bind<IGameRepository>().To<GameRepository>().WithConstructorArgument("BlackJackContext", blackJackContext);
            Bind<IGameResultRepository>().To<GameResultRepository>().WithConstructorArgument("BlackJackContext", blackJackContext);
            Bind<IPlayerRepository>().To<PlayerRepository>().WithConstructorArgument("BlackJackContext", blackJackContext);
            Bind<ITurnRepository>().To<TurnRepository>().WithConstructorArgument("BlackJackContext", blackJackContext);
        }
    }
}

