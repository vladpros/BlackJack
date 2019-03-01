using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public long RoundNumber { get; set; }
        public Enam.GameStatus GameStatus { get; set; }

        public List<Player> Players { get; set; }
        public List<Round> Rounds { get; set; }
        public Game()
        {
            RoundNumber = 0;
            Players = new List<Player>();
            Rounds = new List<Round>();
        }

    }
}
