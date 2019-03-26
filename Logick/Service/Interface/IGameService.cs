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
        Task<IEnumerable<ShowGameViewItem>> DoFirstTwoRounds(long gameId);
        Task<IEnumerable<ShowGameViewItem>> ContinuePlaying(long gameId, PlayerChoose choose);
        Task CheсkAndRegisterPlayer(string name);
        Task<NameView> GetOrderedUsers();
        Task<Game> GetGame(long gameId);
        Task<IEnumerable<ShowGameViewItem>> GetGameResult(IEnumerable<ShowGameViewItem> gameStat);
        Task<bool> IsNewGame(long gameId);
        Task<IEnumerable<ShowGameViewItem>> LoadGame(long gameId);
    }
}