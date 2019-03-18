using BlackJack.DataAccess.Entities;
using BlackJack.BusinessLogic.ViewModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Service.Interface
{
    public interface IGameService
    {
        Task<long> StartGame(Player player, int botsNumber);
        Task<IEnumerable<PlayerInGame>> DoFirstTwoRound(long gameId);
        Task<IEnumerable<PlayerInGame>> ContinuePlay(long gameId, long choose);
        Task<IEnumerable<PlayerInGame>> DropCard(IEnumerable<PlayerInGame> gameStat);
        Task<IEnumerable<PlayerInGame>> GetGameResult(long gameId);
        Task<IEnumerable<PlayerInGame>> InitializationGameStat(long gameId);
        Task PlayerChecked(string name);
        Task<List<string>> GetUserOrdered();
        Task<Player> SearchPlayerWithName(string name);
        Task<Game> GetGame(long gameId);
    }
}