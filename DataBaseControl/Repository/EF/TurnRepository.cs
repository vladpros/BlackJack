using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;

namespace BlackJack.DataBaseAccess.Repository
{
    public class TurnRepository : DefaultGenericRepository<Turn>, ITurnRepository
    {
        private BlackJackContext _context;

        public TurnRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Turn>> GetAllTurns (Game game)
        {
            return await Task.Run(() => _context.Turns.Where(c => c.GameId == game.Id).ToList());
        }
    }
}
