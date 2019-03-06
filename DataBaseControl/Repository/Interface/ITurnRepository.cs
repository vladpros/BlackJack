using BlackJack.DataBaseAccess.Entities;
using System.Collections.Generic;

namespace BlackJack.DataBaseAccess.Repository.Interface
{
    public interface ITurnRepository : IGenericRepository<Turn>
    {
        List<Turn> GetAllTurns(Game game);
    }
}
