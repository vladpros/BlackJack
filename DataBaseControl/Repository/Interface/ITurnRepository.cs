using BlackJackDataBaseAccess.Entities;
using System.Collections.Generic;

namespace BlackJackDataBaseAccess.Repository.Interface
{
    public interface ITurnRepository : IGenericRepository<Turn>
    {
        List<Turn> GetAllTurns(Game game);
    }
}
