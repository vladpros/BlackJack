﻿using BlackJack.DataAccess.Entities;
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

        private readonly string _conString;

        public DapperPlayerRepository(string conString) : base ("Players", conString)
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public async Task<List<Player>> GetByType(PlayerType playerType)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                var result = (await cn.QueryAsync<Player>("SELECT * FROM Players WHERE PlayerType=@playerType", new { playerType })).ToList();
                return result;
            }
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                var result = (await cn.QueryAsync<Player>("SELECT * FROM Players WHERE Name=@name", new { name })).SingleOrDefault();
                return result;
            }
        }
    }
}
