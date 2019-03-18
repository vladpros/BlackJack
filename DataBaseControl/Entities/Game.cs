using BlackJack.DataAccess.Entities.Enums;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace BlackJack.DataAccess.Entities
{
    public class Game : BasicEntities
    {
        public long TurnNumber { get; set; }
        public GameStatus GameStatus { get; set; }
        public int BotsNumber { get; set; }
        public long PlayerId { get; set; }

        [Computed]
        public List<Turn> Turns { get; set; }

        public Game()
        {
            TurnNumber = 0;
            Turns = new List<Turn>();
        }

    }
}
