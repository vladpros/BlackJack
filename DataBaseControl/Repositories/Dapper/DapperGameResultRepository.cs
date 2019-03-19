using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameResultRepository : DapperGenericRepository<GameResult>, IGameResultRepository
    {
        private readonly string _conString;

        public DapperGameResultRepository(string conString) : base("GameResults", conString)
        {
            _conString = conString;
        }
    }
}
