using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repository.Interface;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseControl.Repository.Dapper
{
    public class DapTurnRepository : DapDefaultGenericRepository<Turn>, ITurnRepository
    {

        private readonly string _conString;

        public DapTurnRepository() : base("Turns")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public async Task<List<Turn>> GetAllTurns(Game game)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() => cn.Query<Turn>("SELECT * FROM Turns WHERE GameId=@gameId", new { gameId = game.Id }).ToList());
            }
        }
    }
}
