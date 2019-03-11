using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataBaseAccess.Repository
{
    public class GameRepository : DefaultGenericRepository<Game>, IGameRepository
    {
        private BlackJackContext _context;

        public GameRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Game>>  GetAllGameWithPlayer(Player player)
        {
            return await Task.Run(() => _context.Turns.Where(c => c.PlayerId == player.Id).Select(p => p.Game).Distinct().ToList());
        }
    }
}
