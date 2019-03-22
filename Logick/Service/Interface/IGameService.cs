using BlackJack.DataAccess.Entities;
using BlackJack.BusinessLogic.ViewModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Service.Interface
{
    public interface IGameService
    {
        Task<long> StartGame(string playerName, int botsNumber);
        Task<IEnumerable<PlayerInGameViewModel>> DoFirstTwoRounds(long gameId);
        Task<IEnumerable<PlayerInGameViewModel>> ContinuePlaying(long gameId, long choose);
        Task CheсkPlayer(string name);
        Task<List<string>> GetUserOrdered();
        Task<Game> GetGame(long gameId);
        Task<IEnumerable<PlayerInGameViewModel>> GetGameResult(IEnumerable<PlayerInGameViewModel> gameStat);
        Task<bool> IsNewGame(long gameId);
        Task<IEnumerable<PlayerInGameViewModel>> LoadGame(long gameId);
    }
}