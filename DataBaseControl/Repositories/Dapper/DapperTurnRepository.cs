using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Repositories.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.DataAccess.Repositories.Dapper
{
    public class DapperTurnRepository : DapperGenericRepository<Turn>, ITurnRepository
    {

        private readonly string _connectionString;

        public DapperTurnRepository(string connectionString) : base("Turns", connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Turn>> GetAllTurns(long gameId)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<Turn>($"SELECT * FROM {_tableName} WHERE GameId=@gameId", new { gameId = gameId })).ToList();
                return result;
            }
        }
    }
}
