using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.EF
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private BlackJackContext _context;

        public GameRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Game>>  GetAllGameWithPlayer(Player player)
        {
            var result = await Task.Run(() => _context.Turns.Where(c => c.PlayerId == player.Id).Select(p => p.Game).Distinct().ToList());
            return result;
        }
    }
}
