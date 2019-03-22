using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
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
    public class DapperPlayerRepository : DapperGenericRepository<Player>, IPlayerRepository
    {
        private readonly string _connectionString;

        public DapperPlayerRepository(string connectionString) : base ("Players", connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Player>> GetByType(PlayerType playerType)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<Player>($"SELECT * FROM {_tableName} WHERE PlayerType=@playerType", new { playerType })).ToList();
                return result;
            }
        }

        public async Task<List<Player>> GetByTypeNumber(PlayerType playerType, int number = 1)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<Player>($"SELECT TOP (@number) * FROM {_tableName} WHERE PlayerType=@playerType", new { number, playerType })).ToList();
                return result;
            }
        }

        public async Task<List<Player>> SearchPlayersWithIds(List<long> playersId)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<Player>($"SELECT * FROM {_tableName} WHERE Id IN @playersId", new { playersId })).ToList();

                return result;
            }
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            using (IDbConnection cn = new SqlConnection(_connectionString))
            {
                var result = (await cn.QueryAsync<Player>($"SELECT * FROM {_tableName} WHERE Name=@name", new { name })).SingleOrDefault();
                return result;
            }
        }
    }
}
