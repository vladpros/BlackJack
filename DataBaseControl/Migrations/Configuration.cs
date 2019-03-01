namespace DataBaseControl.Migrations
{
    using DataBaseControl.Entities;
    using DataBaseControl.Repository;
    using DataBaseControl.Repository.Interface;
    using System.Data.Entity.Migrations;


    internal sealed class Configuration : DbMigrationsConfiguration<DataBaseControl.BlackJackContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataBaseControl.BlackJackContext context)
        {
            IPlayerRepository playerRepository = new PlayerRepository(context);

            playerRepository.AddOrUpdate(
                new Player
                {
                    Name = "BobB",
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Point = 100
                });
            playerRepository.AddOrUpdate(
                new Player
                {
                    Name = "TomB",
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Point = 100
                });
            playerRepository.AddOrUpdate(
                new Player
                {
                    Name = "JarryB",
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Point = 100
                });
            playerRepository.AddOrUpdate(
                new Player
                {
                    Name = "MarryB",
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Point = 100
                });
            playerRepository.AddOrUpdate(
                new Player
                {
                    Name = "TonyB",
                    PlayerType = Entities.Enam.PlayerType.Bot,
                    Point = 100
                });
        }
    }
}
