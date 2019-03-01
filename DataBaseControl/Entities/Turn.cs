
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseControl.Entities
{
    public class Turn
    {
        public long Id { get; set; }
       // public long PlayerId { get; set; }
       // public long GameId { get; set; }
        public Enam.LearCard LearCard { get; set; }
        public Enam.NumberCard NumberCard { get; set; }

        public Game Game { get; set; }
        public Player Player { get; set; }
    }
}
