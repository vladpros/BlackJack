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

        private readonly string _conString;

        public DapperPlayerRepository() : base ("Players")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public async Task<List<Player>> GetByType(PlayerType playerType)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() => cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playerType", new { playerType }).ToList());
            }
        }

        public bool IsAPlayer(Player player)
        {
            if (FindById(player.Id) == null)
            {
                return false;
            }

            return true;
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() => cn.Query<Player>("SELECT * FROM Players WHERE Name=@Name", new { Name = name }).SingleOrDefault());
            }
        }

    }
}
