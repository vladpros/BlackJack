using DataBaseControl.Entities.Enam;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseControl.Entities
{
    public class Player
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }

        public List<Turn> Turns { get; set; }

        public Player()
        {
            Turns = new List<Turn>();
        }
    }
}
