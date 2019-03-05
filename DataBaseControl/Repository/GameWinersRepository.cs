using System.Data.Entity;
using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;

namespace DataBaseControl.Repository
{
    public class GameWinersRepository : DefaultGenericRepository<GameWinner>, IGameWinnersRepository
    {

        private BlackJackContext _context;

        public GameWinersRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }
    }
}
