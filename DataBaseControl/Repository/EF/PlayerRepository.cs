using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repository.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repository
{
    public class PlayerRepository : DefaultGenericRepository<Player>, IPlayerRepository
    {

        private BlackJackContext _context;

        public PlayerRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public bool IsAPlayer (Player player)
        {
            return _context.Players.Any(x => x.Name == player.Name);
        }

        public async Task<List<Player>> GetAllUser()
        {
            return await GetAllType(Entities.Enums.PlayerType.User);
        }

        public async Task<List<Player>> GetAllBots()
        {
            return await GetAllType(Entities.Enums.PlayerType.Bot);
        }

        public async Task<List<Player>> GetAllDealer()
        {
            return await GetAllType(Entities.Enums.PlayerType.Dealer);
        }

        private async Task<List<Player>> GetAllType(Entities.Enums.PlayerType p)
        {
            return await Task.Run(() => _context.Players.Where(x => x.PlayerType == p).ToList());
        }

        public async void AddOrUpdate (Player player)
        {
            if (IsAPlayer(player))
            {
                return;
            }

            await Create(player);
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            var b = await Task.Run(() => _context.Players.Where(x => x.Name == name).SingleOrDefault());
            return b;
        }
    }
}
