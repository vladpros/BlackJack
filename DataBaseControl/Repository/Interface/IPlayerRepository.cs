using DataBaseControl.Entities;
using System.Collections.Generic;

namespace DataBaseControl.Repository.Interface
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        List<Player> GetAllUser();
        List<Player> GetAllDealer();
        List<Player> GetAllBots();
        bool IsAPlayer(Player player);
        void AddOrUpdate(Player player);
        Player SearchPlayerWithName(string name);

    }
}
