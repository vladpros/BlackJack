using DataBaseControl.Entities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataBaseControl.GenericRepository
{
    public class GameRepository : DefaultGenericRepository<Game>
    {
        private BlackJackContext _context;

        public GameRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public List<Game>  GetAllGameWithPlayer(Player player)
        {
            var list = _context.Games.Include(c => c.Players).Where(x=>x.Players.FirstOrDefault(c => c.Id == player.Id).Id == player.Id).ToList();

            return list;
        }
 
        public new Game FindById(long id)
        {
            return _context.Games.Include(x=>x.Players).Where(c=>c.Id==id).SingleOrDefault();
        }

        public List<Game> GetAllGameWithStatus(Entities.Enam.GameStatus status)
        {
            var list = _context.Games.Include(c => c.Players).Where(c => c.GameStatus == status).ToList();

            return list;
        }

    }
}
