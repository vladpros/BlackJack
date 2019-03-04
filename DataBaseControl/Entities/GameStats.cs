using System.Collections.Generic;


namespace DataBaseControl.Entities
{
    public class GameStats
    {
        public long PlayerId { get; set;}
        public string PlayerName { get; set; }
        public List<Card> Cards { get; set; }
        public int Point { get; set; }
        public Enam.PlayerStatus PlayerStatus { get; set; }

        public GameStats()
        {
            PlayerStatus = Enam.PlayerStatus.Play;
            Cards = new List<Card>();
            Point = 0;
        }
    }
}
