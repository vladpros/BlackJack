using BlackJack.DataAccess.Entities.Enums;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Helpers
{
    public class DeckHelper
    {
        private int _numberLeaf = 4;
        private int _nuberCardNumber = 14;
        public List<Card> Cards { get; set; }
        public int NumberCard
        {
            get
            {
                return Cards.Count;
            }
        }

        
    }
}
