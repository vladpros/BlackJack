namespace BlackJack.DataBaseAccess.Migrations
{
    using BlackJack.DataBaseAccess;
    using BlackJack.DataBaseAccess.Repository;
    using BlackJack.DataBaseAccess.Repository.Interface;
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
                    PlayerType = Entities.Enum.PlayerType.Bot,
                    Name = "JohnBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enum.PlayerType.Bot,
                    Name = "MaryBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enum.PlayerType.Bot,
                    Name = "JonnyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enum.PlayerType.Bot,
                    Name = "BobBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enum.PlayerType.Bot,
                    Name = "KennyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enum.PlayerType.Dealer,
                    Name = "DenisDealer",
                });
        }
    }
}
          
