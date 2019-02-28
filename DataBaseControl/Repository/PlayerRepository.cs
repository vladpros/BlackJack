using DataBaseControl.Entities;
using DataBaseControl.GenericRepository;
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
            return _context.Players.ToList();
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

                return true;
            }

            return false;
        }
    }
}
