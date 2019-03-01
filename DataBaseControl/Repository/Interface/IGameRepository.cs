using DataBaseControl.Entities;
using System.Collections.Generic;

namespace DataBaseControl.Repository.Interface
{
    public interface IGameRepository : IGenericRepository<Game>
    {
        List<Game> GetAllGameWithPlayer(Player player);
        bool CreatNewGame(Game game);
    }
}
