using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseControl.Entities
{
    public class Game
    {
        public long Id { get; set; }
        public long TurnNumber { get; set; }
        public Enam.GameStatus GameStatus { get; set; }
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public Player User { get; set; }

        [NotMapped]
        public List<Player> Players { get; set; }

        public List<Turn> Turns { get; set; }
        public Game()
        {
            TurnNumber = 0;
            Players = new List<Player>();
            Turns = new List<Turn>();
        }

    }
}
