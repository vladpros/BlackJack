using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameResultRepository : DapperGenericRepository<GameResult>, IGameResultRepository
    {
        private readonly string _connectionString;

        public DapperGameResultRepository(string connectionString) : base("GameResults", connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
