using BlackJack.DataAccess.Entities.Enums;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Entities
{
    public class Player : BasicEntity
    {
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }

        [Write(false)]
        public virtual List<Turn> Turns { get; set; }

        public Player()
        {
            Turns = new List<Turn>();
        }
    }
}
