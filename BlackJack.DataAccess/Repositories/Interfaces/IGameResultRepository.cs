using BlackJack.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Interfaces
{
    public interface IGameResultRepository : IGenericRepository<GameResult>
    {
        Task<IEnumerable<GameResult>> GetGameResult(long gameId);
    }
}
