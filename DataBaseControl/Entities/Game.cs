using BlackJack.DataAccess.Entities.Enums;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Entities
{
    public class Game : BasicEntity
    {
        public GameStatus GameStatus { get; set; }
        public int BotsNumber { get; set; }
        public long PlayerId { get; set; }

        [Write(false)]
        public List<Turn> Turns { get; set; }

        public Game()
        {
            Turns = new List<Turn>();
        }

    }
}
