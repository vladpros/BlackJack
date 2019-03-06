using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Repository.Interface;
using System.Configuration;

namespace DataBaseControl.Repository.Dapper
{
    public class DapGameResultRepository : DapDefaultGenericRepository<GameResult>, IGameResultRepository
    {
        private readonly string _conString;

        public DapGameResultRepository() : base("GameResults")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }
    }
}
