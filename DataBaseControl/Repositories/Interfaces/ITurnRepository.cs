using BlackJack.DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Interfaces
{
    public interface ITurnRepository : IGenericRepository<Turn>
    {
        Task<List<Turn>> GetAllTurns(Game game);
    }
}
