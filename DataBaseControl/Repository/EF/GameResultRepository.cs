using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;

namespace BlackJack.DataBaseAccess.Repository
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
