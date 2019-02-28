﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DataBaseControl.Entities;
using DataBaseControl.Entities.Enam;
using DataBaseControl.GenericRepository;


namespace DataBaseControl.Repository
{
    public class PlayerInTurnRepository : DefaultGenericRepository<PlayerInTurn>
    {
        private BlackJackContext _context;

        public PlayerInTurnRepository(DbContext context) : base(context)
        {

        }

        public List<Card> GetPlayerCard (Player player, Turn turn)
        {
            string cardBuff = _context.PlayerInTurns.Where(x => x.TurnId == turn.Id && x.PlayerId == player.Id).SingleOrDefault().Card;

            string[] card = cardBuff.Split(';');
            string[][] vs = new string[256][];
            int i = 0;
            foreach (var p in card)
            {
                vs[i] = p.Split(',');
                i++;
            }
            List<Card> cards = new List<Card>();
            foreach (var p in vs)
            {
                cards.Add(new Card { LearCard = (LearCard)Convert.ToInt32(p[0]), NumberCard = (NumberCard)Convert.ToInt32(p[1]) });
            }

            return cards;
        }

        public void AddCardToPlayerInTurn(Player player, Turn turn, Card card)
        {
            string cardBuff = _context.PlayerInTurns.Where(x => x.TurnId == turn.Id && x.PlayerId == player.Id).SingleOrDefault().Card;

            cardBuff += $"{card.LearCard},{card.NumberCard};";
        }
    }
}
