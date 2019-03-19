using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperTurnRepository : DapperGenericRepository<Turn>, ITurnRepository
    {

        private readonly string _conString;

        public DapperTurnRepository(string conString) : base("Turns", conString)
        {
            _conString = conString;
        }

        public async Task<List<Turn>> GetAllTurns(Game game)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                var result = (await cn.QueryAsync<Turn>("SELECT * FROM Turns WHERE GameId=@gameId", new { gameId = game.Id })).ToList();
                return result;
            }
        }
    }
}
