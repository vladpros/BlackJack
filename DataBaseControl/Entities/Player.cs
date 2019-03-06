using BlackJack.DataBaseAccess.Entities.Enum;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.DataBaseAccess.Entities
{
    public class Player : BasicEntities
    {
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }

        [Computed]
        public virtual List<Turn> Turns { get; set; }

        public Player()
        {
            Turns = new List<Turn>();
        }
    }
}
