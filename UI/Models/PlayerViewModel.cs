using BlackJack.DataBaseAccess.Entities.Enum;
using Logick.Models;
using System.Collections.Generic;

namespace BlackJack.UI.Models
{
    public class PlayerViewModel
    {
        public string PlayerName { get; set; }
        public List<Card> Cards { get; set; }
        public int Point { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public long GameId { get; set; }
    }
}