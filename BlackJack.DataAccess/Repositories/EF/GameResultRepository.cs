using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.EF
{
    public class GameResultRepository : GenericRepository<GameResult>, IGameResultRepository
    {
        private BlackJackContext _context;

        public GameResultRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameResult>> GetGameResult(long gameId)
        {
            var result = await Task.Run(() => _context.GameResults.Where(x => x.GameId == gameId).ToList());

            return result;
        }
    }
}
