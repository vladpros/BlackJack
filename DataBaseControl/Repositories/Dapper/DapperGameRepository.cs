using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameRepository : DapperGenericRepository<Game>, IGameRepository
    {
        private readonly string _conString;

        public DapperGameRepository(string conString) : base("Games", conString)
        {
            _conString = conString;
        }
    }
}
