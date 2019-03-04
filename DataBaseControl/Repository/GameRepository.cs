using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataBaseControl.Repository
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
            var list = _context.Games.Where(c=> c.UserId == player.Id).ToList();

            return list;
        }

        public bool CreatNewGame (Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();

            return true;
        }
    }
}
