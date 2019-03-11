using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Entities.Enum;
using BlackJack.DataBaseAccess.Repository.Interface;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseControl.Repository.Dapper
{
    public class DapPlayerRepository : DapDefaultGenericRepository<Player>, IPlayerRepository
    {

        private readonly string _conString;

        public DapPlayerRepository() : base ("Players")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public async void AddOrUpdate(Player player)
        {
            if (FindById(player.Id) == null)
            {
                return;
            }

            await Create(player);
        }

        public async Task<List<Player>> GetAllBots()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() => cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.Bot }).ToList());
            }
        }

        public async Task<List<Player>> GetAllDealer()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() =>  cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.Dealer }).ToList());
            }
        }

        public async Task<List<Player>> GetAllUser()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return await Task.Run(() => cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.User }).ToList());
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
