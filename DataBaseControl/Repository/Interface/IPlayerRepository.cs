using DataBaseControl.Entities;
using System.Collections.Generic;

namespace DataBaseControl.Repository.Interface
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        List<Player> GetAllUser();
        List<Player> GetAllDealer();
        List<Player> GetAllBots();
        bool RegisterNewPlayer(Player player);
        bool IsAPlayer(Player player);
        void AddOrUpdate(Player player);
        List<Player> GetAllPlayerGames(Game game);
    }
}
