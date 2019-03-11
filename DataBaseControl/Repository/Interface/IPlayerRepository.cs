using BlackJack.DataBaseAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.DataBaseAccess.Repository.Interface
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        Task<List<Player>> GetAllUser();
        Task<List<Player>> GetAllDealer();
        Task<List<Player>> GetAllBots();
        bool IsAPlayer(Player player);
        void AddOrUpdate(Player player);
        Task<Player> SearchPlayerWithName(string name);

    }
}
