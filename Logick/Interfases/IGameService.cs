using BlackJack.DataBaseAccess.Entities;
using Logick.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logick.Interfases
{
    public interface IGameService
    {
        Task<long> StartGame(Player player, int botsNumber);
        Task<GameStat> DoFirstTwoRound(long gameId);
        Task<GameStat> ContinuePlay(long gameId, long choose);
        Task<GameStat> DropCard(GameStat gameStat);
        Task<GameStat> GetGameResult(long gameId);
    }
}
