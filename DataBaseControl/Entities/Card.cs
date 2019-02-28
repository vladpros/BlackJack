using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Card
    {
        public long Id { get; set; }
        public Enam.NumberCard NumberCard { get; set; }
        public Enam.LearCard LearCard { get; set; }

        public List<PlayerInTurn> PlayerInTurns { get; set; }
    }
}
