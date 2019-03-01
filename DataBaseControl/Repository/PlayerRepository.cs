using DataBaseControl.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DataBaseControl.Repository
{
    public class PlayerRepository : DefaultGenericRepository<Player>
    {
        private BlackJackContext _context;

        public PlayerRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public new Player FindById(long id)
        {
            return _context.Players.Include(x => x.Games).Where(c => c.Id == id).SingleOrDefault();
        }

        public List<Player> GetAllPlayerGames (Game game)
        {
            return _context.Players.Include(c => c.Games).Where(x => x.Games.FirstOrDefault(c => c.Id == game.Id).Id == game.Id).ToList();
        }

        public List<Player> GetAllPlayer()
        {
            return _context.Players.Where(x => x.PlayerType == (Entities.Enam.PlayerType)1).ToList();
        }

        public bool IsAPlayer (Player player)
        {
            return _context.Players.Any(x => x.Name == player.Name);
        }

        public bool RegisterNewPlayer (Player player)
        {
            if (player.Name != null)
            {
                player.PlayerType = Entities.Enam.PlayerType.User;
                _context.Players.Add(player);
                _context.SaveChanges();

                return true;
            }

            return false;
        }

        public List<Player> GetAllBots()
        {
            return _context.Players.Where(x => x.PlayerType == (Entities.Enam.PlayerType)2).ToList();
        }

        public List<Player> GetAllDealer()
        {
            return _context.Players.Where(x => x.PlayerType == (Entities.Enam.PlayerType)3).ToList();
        }
    }
}
