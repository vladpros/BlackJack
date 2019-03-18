using BlackJack.DataAccess.Entities.Enums;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlackJack.DataAccess.Entities
{
    public class Turn : BasicEntities
    {
        public long PlayerId { get; set; }
        public long GameId { get; set; }
        public CardLear LearCard { get; set; }
        public CardNumber NumberCard { get; set; }

        [ForeignKey("GameId")]
        [Computed]
        public Game Game { get; set; }
        [ForeignKey("PlayerId")]
        [Computed]
        public Player Player { get; set; }
    }
}
