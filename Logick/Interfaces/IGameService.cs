using BlackJack.DataAccess.Entities;
using BlackJack.BusinessLogic.Models;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Interfaces
{
    public interface IGameService
    {
        Task<long> StartGame(Player player, int botsNumber);
        Task<GameStat> DoFirstTwoRound(long gameId);
        Task<GameStat> ContinuePlay(long gameId, long choose);
        Task<GameStat> DropCard(GameStat gameStat);
        Task<GameStat> GetGameResult(long gameId);
        Task<GameStat> InitializationGameStat(long gameId);
    }
}
