
namespace DataBaseControl.Entities
{
    public class Card
    {
        public Enam.LearCard LearCard { get; set; }
        public Enam.NumberCard NumberCard { get; set; }
        public int Point { get; set; }

        public Card()
        {
            if((int)NumberCard < 10)
            {
                Point = (int)NumberCard;
            }
            if((int)NumberCard >= 10)
            {
                Point = 10;
            }
        }
    }
}
