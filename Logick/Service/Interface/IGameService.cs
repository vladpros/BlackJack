using BlackJack.DataAccess.Entities;
using BlackJack.BusinessLogic.ViewModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Service.Interface
{
    public interface IGameService
    {
        Task<long> StartGame(Player player, int botsNumber);
        Task<IEnumerable<PlayerInGameViewModel>> DoFirstTwoRound(long gameId);
        Task<IEnumerable<PlayerInGameViewModel>> ContinuePlay(long gameId, long choose);
        Task<IEnumerable<PlayerInGameViewModel>> DropCard(IEnumerable<PlayerInGameViewModel> gameStat);
        Task<IEnumerable<PlayerInGameViewModel>> GetGameResult(long gameId);
        Task<IEnumerable<PlayerInGameViewModel>> InitializationGameStat(long gameId);
        Task ChekPlayer(string name);
        Task<List<string>> GetUserOrdered();
        Task<Player> SearchPlayerWithName(string name);
        Task<Game> GetGame(long gameId);
    }
}