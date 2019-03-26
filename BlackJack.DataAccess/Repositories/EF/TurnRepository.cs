using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.EF
{
    public class TurnRepository : GenericRepository<Turn>, ITurnRepository
    {
        private BlackJackContext _context;

        public TurnRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Turn>> GetAllTurns (long gameId)
        {
            var result = await Task.Run(() => _context.Turns.Where(c => c.GameId == gameId).ToList()); 
            return result;
        }
    }
}
