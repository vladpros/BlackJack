using BlackJack.DataAccess.Entities.Enums;

namespace BlackJack.DataAccess.Entities
{
    public class GameResult : BasicEntitie
    {
        public long GameId { get; set; }
        public long PlayerId { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
    }
}
