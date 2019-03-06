using BlackJack.DataBaseAccess.Entities;
using System.Collections.Generic;

namespace Logick.Interfases
{
    public interface IDataService
    {
        List<Player> GetUserOrdered();
        Player SearchPlayerWithName(string name);
        void PlayerChecked(string name);
        List<GameStats> GetGameStats(Game game);
        Deck GetDeck(List<GameStats> gameStats);
        int SearchDealer(List<GameStats> gameStats);
        int SearchUser(List<GameStats> gameStats);
        List<GameStats> GenPlayers(long player, int botsNumber);
        void SaveWinner(List<GameStats> gameStats);
    }
}
