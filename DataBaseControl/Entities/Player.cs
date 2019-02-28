using DataBaseControl.Entities.Enam;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataBaseControl.Entities
{
    public class Player
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Point { get; set; }
        public PlayerType PlayerType { get; set; }

        public List<Game> Games { get; set; }
        public List<PlayerInTurn> PlayerInTurns { get; set; }
        public Player()
        {
            Games = new List<Game>();
            PlayerInTurns = new List<PlayerInTurn>();
            Point = 0;
        }
    }
}
