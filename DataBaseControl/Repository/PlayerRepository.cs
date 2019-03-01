using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace DataBaseControl.Repository
{
    public class PlayerRepository : DefaultGenericRepository<Player>, IPlayerRepository
    {

        private BlackJackContext _context;

        public PlayerRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public List<Player> GetAllPlayerGames (Game game)
        {
            return _context.Players.Include(c => c.Games).Where(x => x.Games.FirstOrDefault(c => c.Id == game.Id).Id == game.Id).ToList();
        }

        public bool IsAPlayer (Player player)
        {
            return _context.Players.Any(x => x.Name == player.Name);
        }

        public List<Player> GetAllUser()
        {
            return GetAllType(Entities.Enam.PlayerType.User);
        }

        public List<Player> GetAllBots()
        {
            return GetAllType(Entities.Enam.PlayerType.Bot);
        }

        public List<Player> GetAllDealer()
        {
            return GetAllType(Entities.Enam.PlayerType.Dealer);
        }

        private List<Player> GetAllType(Entities.Enam.PlayerType p)
        {
            return _context.Players.Where(x => x.PlayerType == p).ToList();
        }

        public void AddOrUpdate (Player player)
        {
            if (IsAPlayer(player))
            {
                return;
            }
            if (!IsAPlayer(player))
            {
                Create(player);
            }
        }

        public Player SearchPlayerWithName(string name)
        {
            return _context.Players.Include(c => c.Games).Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
