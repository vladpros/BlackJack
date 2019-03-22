using BlackJack.DataAccess.Entities.Enums;

namespace BlackJack.BusinessLogic.Helpers
{
    public class CardHelper
    {
        public CardLear CardLear { get; set; }
        public CardNumber CardNumber { get; set; }
        private int _pointCard = 10;

        public int GetCardPoint(CardHelper card)
        {
            if ((int)card.CardNumber < _pointCard)
            {
                return (int)card.CardNumber;
            }
            if ((int)card.CardNumber >= _pointCard)
            {
                return _pointCard;
            }

            return (int)card.CardNumber;
        }
    }
}
