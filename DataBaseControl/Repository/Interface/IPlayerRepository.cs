using BlackJackDataBaseAccess.Entities;
using System.Collections.Generic;

namespace BlackJackDataBaseAccess.Repository.Interface
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
