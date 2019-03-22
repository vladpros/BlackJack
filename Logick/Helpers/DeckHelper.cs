using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities.Enums;
using System;
using System.Collections.Generic;

namespace BlackJack.BusinessLogic.Helpers
{
    public class DeckHelper
    {
        private Random _random;
        private int _maxCardLear = 4;
        private int _maxCardNumber = 14;
        private int _minCardLear = 1;
        private int _minCardNumber = 2;
        public List<CardView> Cards { get; set; }
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
            Cards = new List<CardView>();

            for (int i = _minCardLear; i <= _maxCardLear; i++)
            {
                for (int j = _minCardNumber; j <= _maxCardNumber; j++)
                {
                    Cards.Add(
                              new CardView
                              {
                                  CardLear = (CardLear)i,
                                  CardNumber = (CardNumber)j
                              });
                }

            }
        }

        public DeckHelper(IEnumerable<PlayerInGameView> players) : this()
        {
            foreach (var player in players)
            {
                foreach (var card in player.Cards)
                {
                    Cards.RemoveAt(Cards.FindIndex(p => p.CardLear == card.CardLear && p.CardNumber == card.CardNumber));
                }
            }
        }

        public CardView GiveCard()
        {
            int rand = _random.Next(0, Cards.Count);
            CardView card = Cards[rand];
            Cards.RemoveAt(rand);

            return card;
        }

    }
}
