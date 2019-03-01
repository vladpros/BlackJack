using System;
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

        public PlayerInTurnRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public List<Card> GetPlayerCard (Player player, Turn turn)
        {
            string cardBuff = _context.PlayerInTurns.Where(x => x.TurnId == turn.Id && x.PlayerId == player.Id).SingleOrDefault().Card;

            string[] buff = cardBuff.Split(';');
            string[][] buff2 = new string[256][];
            int i = 0;
            foreach (var p in buff)
            {
                buff2[i] = p.Split(',');
                i++;
            }
            List<Card> cards = new List<Card>();
            foreach (var p in buff2)
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
