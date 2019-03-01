
namespace DataBaseControl.Entities
{
    public class Turn
    {
        public long Id { get; set; }
        public long TurnId { get; set; }
        public long PlayerId { get; set; }
        public Enam.LearCard LearCard { get; set; }
        public Enam.NumberCard NumberCard { get; set; }
        public Enam.PlayerStatus PlayerStatus { get; set; }

        public Round Round { get; set; }
        public Player Player { get; set; }
    }
}
