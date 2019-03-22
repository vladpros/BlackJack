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

        public async Task<List<Player>> GetByTypeNumber(PlayerType p, int number = 1)
        {
            var result = await Task.Run(() => _context.Players.Where(x => x.PlayerType == p).Take(number).ToList());
            return result;
        }

        public async Task<List<Player>> GetByType(PlayerType p)
        {
            var result = await Task.Run(() => _context.Players.Where(x => x.PlayerType == p).ToList());

            return result;
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            var result = await Task.Run(() => _context.Players.Where(x => x.Name == name).SingleOrDefault());
            return result;
        }

        public async Task<List<Player>> SearchPlayersWithIds(List<long> playersId)
        {
            var result = await Task.Run(() => _context.Players.Where(x => playersId.Contains(x.Id)).ToList());

            return result;
        }
    }
}
