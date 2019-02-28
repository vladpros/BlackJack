using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Turn
    {
        public long Id { get; set; }
        public long NumberInGame { get; set; }
        public long GameId { get; set; }

        public Game Game { get; set; }
        public List<PlayerInTurn> PlayerInTurns { get; set; }
    }
}
