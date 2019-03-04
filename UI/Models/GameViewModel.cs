using DataBaseControl.Entities;
using System.Collections.Generic;


namespace UI.Models
{
    public class GameViewModel
    {
        public long Id { get; set; }
        public long TurnNumber { get; set; }
        public DataBaseControl.Entities.Enam.GameStatus GameStatus { get; set; }
        public long UserId { get; set; }
        public Player User { get; set; }
        public List<Player> Players { get; set; }
        public List<Turn> Turns { get; set; }

    }
}