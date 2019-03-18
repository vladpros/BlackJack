using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using System.Configuration;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameResultRepository : DapperGenericRepository<GameResult>, IGameResultRepository
    {
        private readonly string _conString;

        public DapperGameResultRepository() : base("GameResults")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }
    }
}
