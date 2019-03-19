using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities.Enums;
using System;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Helpers
{
    public class DeckHelper
    {
        private Random _random;
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

        public DeckHelper()
        {
            _random = new Random();
            Cards = new List<Card>();

            for (int i = 1; i < _numberLeaf + 1; i++)
            {
                for (int j = 2; j < _nuberCardNumber + 1; j++)
                {
                    Cards.Add(
                              new Card
                              {
                                  CardLear = (CardLear)i,
                                  CardNumber = (CardNumber)j
                              });
                }

            }
        }

        public DeckHelper(IEnumerable<PlayerInGameViewModel> players) : this()
        {
            foreach (var player in players)
            {
                foreach (var card in player.Cards)
                {
                    Cards.RemoveAt(Cards.FindIndex(p => p.CardLear == card.CardLear && p.CardNumber == card.CardNumber));
                }
            }
        }

        public Card GiveCard(DeckHelper deck)
        {
            int rand = _random.Next(0, deck.NumberCard);
            Card card = deck.Cards[rand];
            deck.Cards.RemoveAt(rand);

            return card;
        }

        public int GetCardPoint(Card card)
        {
            if ((int)card.CardNumber < 10)
            {
                return (int)card.CardNumber;
            }
            if ((int)card.CardNumber >= 10)
            {
                return 10;
            }

            return (int)card.CardNumber;
        }

    }
}
