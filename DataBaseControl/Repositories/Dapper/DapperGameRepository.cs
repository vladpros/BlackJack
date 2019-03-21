using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameRepository : DapperGenericRepository<Game>, IGameRepository
    {
        private readonly string _connectionString;

        public DapperGameRepository(string connectionString) : base("Games", connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
