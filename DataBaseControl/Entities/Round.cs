using System.Collections.Generic;

namespace DataBaseControl.Entities
{
    public class Round
    {
        public long Id { get; set; }
        public long NumberInGame { get; set; }
        public long GameId { get; set; }

        public Game Game { get; set; }
        public List<Turn> Turns { get; set; }
    }
}
