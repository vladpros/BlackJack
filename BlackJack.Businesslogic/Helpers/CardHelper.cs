using BlackJack.BusinessLogic.ViewModel;

namespace BlackJack.BusinessLogic.Helpers
{
    public class CardHelper
    {
        private int _pointCard = 10;

        public int GetCardPoint(CardView card)
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
