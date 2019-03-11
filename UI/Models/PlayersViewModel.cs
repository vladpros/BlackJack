using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Entities.Enum;
using System.Collections.Generic;

namespace BlackJack.UI.Models
{
    public class PlayersViewModel
    {
        public string PlayerName { get; set; }
        public List<Card> Cards { get; set; }
        public int Point { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
    }
}