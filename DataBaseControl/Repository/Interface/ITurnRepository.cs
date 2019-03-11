using BlackJack.DataBaseAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataBaseAccess.Repository.Interface
{
    public interface ITurnRepository : IGenericRepository<Turn>
    {
        Task<List<Turn>> GetAllTurns(Game game);
    }
}
