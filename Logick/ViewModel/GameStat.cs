using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Models
{
    public class GameStat
    {
        public long GameId { get; set; }
        public List<PlayerInGame> Players { get; set; }
        
        public GameStat ()
        {
            Players = new List<PlayerInGame>();
        }
    }
}
