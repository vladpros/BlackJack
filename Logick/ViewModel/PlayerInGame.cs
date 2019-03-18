using BlackJack.DataAccess.Entities.Enums;
using System.Collections.Generic;


namespace BlackJack.BusinessLogic.Models
{

    public class PlayerInGame
    {
        public long PlayerId { get; set; }
        public PlayerType PlayerType { get; set; }
        public string PlayerName { get; set; }
        public List<Card> Cards { get; set; }
        public int Point { get; set; }
        public PlayerStatus PlayerStatus { get; set; }

        public PlayerInGame()
        {
            PlayerStatus = PlayerStatus.Play;
            Cards = new List<Card>();
        }
    }
}
