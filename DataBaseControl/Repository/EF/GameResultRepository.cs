using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repository.Interface;

namespace BlackJack.DataAccess.Repository
{
    public class GameResultRepository : DefaultGenericRepository<GameResult>, IGameResultRepository
    {

        private BlackJackContext _context;

        public GameResultRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }
    }
}
