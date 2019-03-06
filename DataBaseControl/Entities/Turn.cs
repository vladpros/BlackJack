using DataBaseControl.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseControl.Entities
{
    public class Turn : BasicEntities
    {
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public CardLear LearCard { get; set; }
        public CardNumber NumberCard { get; set; }

        [ForeignKey("GameId")]
        public Game Game { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
    }
}
