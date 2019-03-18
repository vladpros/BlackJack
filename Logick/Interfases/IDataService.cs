using BlackJack.DataAccess.Entities;
using Logick.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logick.Interfases
{
    public interface IDataService
    {
        Task<List<string>> GetUserOrdered();
        Task<Player> SearchPlayerWithName(string name);
        Task PlayerChecked(string name);
        Task<List<PlayerInGame>> PlayersInGame(Game game);
        Deck GetDeck(List<PlayerInGame> gameStats);
        Task<int> SearchDealer(List<PlayerInGame> players);
        Task<int> SearchUser(List<PlayerInGame> players);
        Task<List<PlayerInGame>> GenPlayers(long player, int botsNumber);
        void SaveWinner(GameStat gameStat);
        Task<Game> GetGame(long id);
    }
}
