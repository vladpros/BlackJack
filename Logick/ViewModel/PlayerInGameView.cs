using BlackJack.BusinessLogic.Helpers;
using BlackJack.DataAccess.Entities.Enums;
using System.Collections.Generic;


namespace BlackJack.BusinessLogic.ViewModel
{

    public class PlayerInGameView
    {
        public long PlayerId { get; set; }
        public PlayerType PlayerType { get; set; }
        public string PlayerName { get; set; }
        public List<CardHelper> Cards { get; set; }
        public int Point { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public long GameId { get; set; }

        public PlayerInGameView()
        {
            PlayerStatus = PlayerStatus.Play;
            Cards = new List<CardHelper>();
        }
    }
}
