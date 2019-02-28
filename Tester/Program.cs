using DataBaseControl;
using System.Data.Entity;
using System;
using System.Linq;
using DataBaseControl.GenericRepository;
using DataBaseControl.Repository;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BlackJackContext db = new BlackJackContext())
            {
                GameRepository _game = new GameRepository(db);
                PlayerRepository _player = new PlayerRepository(db);

                var l = _game.GetAllGameWithPlayer(new DataBaseControl.Entities.Player { Id=1 });
                var k = _game.FindById(1);

                var p = _player.GetAllPlayerGames(new DataBaseControl.Entities.Game { Id = 3 });
                Console.WriteLine(k.Players);
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
