using System.Collections.Generic;

namespace Logick.Models
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
