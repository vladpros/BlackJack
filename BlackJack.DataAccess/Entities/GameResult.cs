using BlackJack.DataAccess.Entities.Enums;

namespace BlackJack.DataAccess.Entities
{
    public class GameResult : BasicEntity
    {
        public long GameId { get; set; }
        public long PlayerId { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
    }
}
