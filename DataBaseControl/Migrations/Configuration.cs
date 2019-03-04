namespace DataBaseControl.Migrations
{
    using DataBaseControl.Repository;
    using DataBaseControl.Repository.Interface;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataBaseControl.BlackJackContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataBaseControl.BlackJackContext context)
        {
            IPlayerRepository player = new PlayerRepository(context);

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Name = "JohnBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Name = "MaryBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Name = "JonnyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Name = "BobBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Name = "KennyBot",
                });

            player.AddOrUpdate(
                new Entities.Player
                {
                    PlayerType = Entities.Enam.PlayerType.Dealer,
                    Name = "DenisDealer",
                });
        }
    }
}
          
