using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Repository.Interface;

namespace BlackJackDataBaseAccess.Repository
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
