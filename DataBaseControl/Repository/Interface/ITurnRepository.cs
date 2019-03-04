using DataBaseControl.Entities;
using System.Collections.Generic;

namespace DataBaseControl.Repository.Interface
{
    public interface ITurnRepository : IGenericRepository<Turn>
    {
        List<Turn> GetAllTurns(Game game);
    }
}
