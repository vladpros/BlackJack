using BlackJack.DataBaseAccess;
using BlackJack.DataBaseAccess.Repository;
using BlackJack.DataBaseAccess.Repository.Interface;
using DataBaseControl.Repository.Dapper;
using Ninject.Modules;

namespace BlackJack.DataBaseAccess.Utils
{
    public class NinjectRegistrationRepository : NinjectModule
    {
        public override void Load()
        {
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
