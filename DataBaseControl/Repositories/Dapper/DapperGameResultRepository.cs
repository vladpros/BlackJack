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
    public class DapperGameResultRepository : DapperGenericRepository<GameResult>, IGameResultRepository
    {
        private readonly string _connectionString;

        public DapperGameResultRepository(string connectionString) : base("GameResults", connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<GameResult>> GetGameResult(long gameId)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<GameResult>($"SELECT * FROM {_tableName} WHERE GameId=@gameId", new { gameId })).ToList();
                return result;
            }
        }
    }
}
