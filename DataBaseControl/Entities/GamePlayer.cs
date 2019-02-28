using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseControl.Entities
{
    public class GamePlayer
    {
        public long GameId { get; set; }
        public Game Game { get; set; }

        public long PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
