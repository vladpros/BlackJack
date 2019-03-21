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
    }
}
