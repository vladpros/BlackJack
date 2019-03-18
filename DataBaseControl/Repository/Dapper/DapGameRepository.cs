using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repository.Interface;
using System.Configuration;

namespace DataBaseControl.Repository.Dapper
{
    public class DapGameRepository : DapDefaultGenericRepository<Game>, IGameRepository
    {
        private readonly string _conString;

        public DapGameRepository() : base("Games")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }
    }
}
