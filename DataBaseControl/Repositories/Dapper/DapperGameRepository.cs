using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using System.Configuration;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperGameRepository : DapperGenericRepository<Game>, IGameRepository
    {
        private readonly string _conString;

        public DapperGameRepository() : base("Games")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }
    }
}
