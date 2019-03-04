using System.Collections.Generic;
using System.Linq;
using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;

namespace DataBaseControl.Repository
{
    public class TurnReposytory : DefaultGenericRepository<Turn>, ITurnRepository
    {
        private BlackJackContext _context;

        public TurnReposytory(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public List<Turn> GetAllTurns (Game game)
        {
            return _context.Turns.Where(c => c.GameId == game.Id).ToList();
        }
    }
}
