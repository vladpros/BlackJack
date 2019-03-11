using System.Collections.Generic;

namespace BlackJack.DataBaseAccess.Entities
{
    public class Deck
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

        public Deck ()
        {
            Cards = new List<Card>();

            for (int i = 1; i < _numberLeaf + 1; i++)
            {
                for (int j = 2; j < _nuberCardNumber + 1; j++)
                {
                    Cards.Add(
                              new Card
                              {
                              LearCard = (Enum.CardLear)i,
                              NumberCard = (Enum.CardNumber)j
                              });
                }

            }
        }
    }
}
