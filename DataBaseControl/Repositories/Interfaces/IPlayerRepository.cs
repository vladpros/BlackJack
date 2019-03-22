using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<List<Player>> GetByTypeNumber(PlayerType p, int number = 1);
        Task<List<Player>> GetByType(PlayerType p);
        Task<Player> SearchPlayerWithName(string name);
        Task<List<Player>> SearchPlayersWithIds(List<long> playersId);
    }
}
