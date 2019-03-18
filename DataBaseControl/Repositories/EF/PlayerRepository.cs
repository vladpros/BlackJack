using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.EF
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {

        private BlackJackContext _context;

        public PlayerRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public bool IsAPlayer (Player player)
        {
            return _context.Players.Any(x => x.Name == player.Name);
        }

       public async Task<List<Player>> GetByType(PlayerType p)
        {
            return await Task.Run(() => _context.Players.Where(x => x.PlayerType == p).ToList());
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            var b = await Task.Run(() => _context.Players.Where(x => x.Name == name).SingleOrDefault());
            return b;
        }
    }
}
