using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseControl.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public long TurnNumber { get; set; }
        public Enam.GameStatus GameStatus { get; set; }
        public int BotsNumber { get; set; }
        public long PlayerId { get; set; }

        public List<Turn> Turns { get; set; }

        public Game()
        {
            TurnNumber = 0;
            Turns = new List<Turn>();
        }

    }
}
