using BlackJack.DataBaseAccess.Entities;
using System.Collections.Generic;

namespace Logick.Interfases
{
    public interface IGameService
    {
        long StartGame(Player player, int botsNumber);
        List<GameStats> DoFirstTwoRound(long gameId);
        List<GameStats> ContinuePlay(long gameId, long choose);
        List<GameStats> DropCard(List<GameStats> gameStats);
        List<GameStats> GetGameResult(long gameId);
        List<GameStats> DealerLose(List<GameStats> gameStats);
    }
}
