using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public long TurnNumber { get; set; }
        public Enam.GameStatus GameStatus { get; set; }

        public List<Player> Players { get; set; }
        public List<Turn> Turns { get; set; }
        public Game()
        {
            Players = new List<Player>();
            Turns = new List<Turn>();
        }

    }
}
