using DataBaseControl.Entities.Enum;
using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Player : BasicEntities
    {
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }

        public List<Turn> Turns { get; set; }

        public Player()
        {
            Turns = new List<Turn>();
        }
    }
}
