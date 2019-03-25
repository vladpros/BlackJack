using BlackJack.DataAccess.Entities;
using BlackJack.BusinessLogic.ViewModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlackJack.BusinessLogic.ViewModel.Enum;


namespace BlackJack.BusinessLogic.Service.Interface
{
    public interface IGameService
    {
        Task<long> StartGame(string playerName, int botsNumber);
        Task<IEnumerable<PlayerInGameView>> DoFirstTwoRounds(long gameId);
        Task<IEnumerable<PlayerInGameView>> ContinuePlaying(long gameId, PlayerChoos choose);
        Task CheсkAndRegisterPlayer(string name);
        Task<NameView> GetOrderedUsers();
        Task<Game> GetGame(long gameId);
        Task<IEnumerable<PlayerInGameView>> GetGameResult(IEnumerable<PlayerInGameView> gameStat);
        Task<bool> IsNewGame(long gameId);
        Task<IEnumerable<PlayerInGameView>> LoadGame(long gameId);
    }
}