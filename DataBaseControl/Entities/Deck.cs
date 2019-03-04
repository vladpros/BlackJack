﻿using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Deck
    {
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

            for (int i = 1; i < 5; i++)
            {
                for (int j = 2; j < 15; j++)
                {
                    Cards.Add(
                        new Card
                        {
                            LearCard = (Enam.LearCard)i,
                            NumberCard = (Enam.NumberCard)j
                        });
                }
            }
        }
    }
}
