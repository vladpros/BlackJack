using System.Data.Entity;
using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;

namespace DataBaseControl.Repository
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
