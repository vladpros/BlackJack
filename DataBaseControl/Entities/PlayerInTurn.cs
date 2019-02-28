using System.Collections.Generic;


namespace DataBaseControl.Entities
{
    public class PlayerInTurn
    {
        public long Id { get; set; }
        public long TurnId { get; set; }
        public long PlayerId { get; set; }
        public string Card { get; set; }

        public Turn Turn { get; set; }
        public Player Player { get; set; }
    }
}
