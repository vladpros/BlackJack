using BlackJack.DataAccess.Entities.Enums;
using System.Collections.Generic;


namespace BlackJack.BusinessLogic.ViewModel
{
    public class ShowGameView
    {
        public IEnumerable<ShowGameViewItem> playerInGameViewItems { get; set; }

        public ShowGameView()
        {
            playerInGameViewItems = new List<ShowGameViewItem>();
        }
    }
    public class ShowGameViewItem
    {
        public long PlayerId { get; set; }
        public PlayerType PlayerType { get; set; }
        public string PlayerName { get; set; }
        public List<CardView> Cards { get; set; }
        public int Points { get; set; }
        public PlayerStatus PlayerStatus { get; set; }
        public long GameId { get; set; }

        public ShowGameViewItem()
        {
            PlayerStatus = PlayerStatus.Play;
            Cards = new List<CardView>();
        }
    }
}
