using BlackJack.DataAccess;
using BlackJack.DataAccess.Repositories.Dapper;
using BlackJack.DataAccess.Repositories.EF;
using BlackJack.DataAccess.Repositories.Interfaces;
using Ninject.Modules;
using System.Configuration;

namespace BlackJack.BusinessLogic
{
    public class NinjectRepositoryRegistration : NinjectModule
    {

        public override void Load()
        {
            bool useDapper = true;
            if (useDapper)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

                Bind<IGameRepository>().To<DapperGameRepository>().WithConstructorArgument("connectionString", connectionString);
                Bind<IGameResultRepository>().To<DapperGameResultRepository>().WithConstructorArgument("connectionString", connectionString);
                Bind<IPlayerRepository>().To<DapperPlayerRepository>().WithConstructorArgument("connectionString", connectionString);
                Bind<ITurnRepository>().To<DapperTurnRepository>().WithConstructorArgument("connectionString", connectionString);

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

