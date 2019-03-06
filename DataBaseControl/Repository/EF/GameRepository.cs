using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack.DataBaseAccess.Repository
{
    public class GameRepository : DefaultGenericRepository<Game>, IGameRepository
    {
        private BlackJackContext _context;

        public GameRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public List<Game>  GetAllGameWithPlayer(Player player)
        {
            return _context.Turns.Where(c => c.PlayerId == player.Id).Select(p => p.Game).Distinct().ToList(); ;
        }

        public bool CreatNewGame (Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();

            return true;
        }
    }
}
