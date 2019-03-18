namespace BlackJack.DataAccess.Migrations
{
    using BlackJack.DataAccess;
    using BlackJack.DataAccess.Repository;
    using BlackJack.DataAccess.Repository.Interface;
    using System.Data.Entity.Migrations;


    internal sealed class Configuration : DbMigrationsConfiguration<BlackJackContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BlackJackContext context)
        {
            IPlayerRepository player = new PlayerRepository(context);

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Bot,
                    Name = "JohnBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Bot,
                    Name = "MaryBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Bot,
                    Name = "JonnyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Bot,
                    Name = "BobBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Bot,
                    Name = "KennyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enums.PlayerType.Dealer,
                    Name = "DenisDealer",
                });
        }
    }
}
          
