using BlackJack.DataBaseAccess.Entities.Enum;
using System.Collections.Generic;


namespace BlackJack.DataBaseAccess.Entities
{
    public class GameStats
    {
        public long PlayerId { get; set;}
        
        public long GameId { get; set; }
        public PlayerType PlayerType { get; set; }

        public GameStats()
        {
            PlayerStatus = PlayerStatus.Play;
            Cards = new List<Card>();
            Point = 0;
        }
    }
}
