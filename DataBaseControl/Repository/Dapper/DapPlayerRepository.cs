using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Entities.Enum;
using BlackJackDataBaseAccess.Repository.Interface;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseControl.Repository.Dapper
{
    public class DapPlayerRepository : DapDefaultGenericRepository<Player>, IPlayerRepository
    {

        private readonly string _conString;

        public DapPlayerRepository() : base ("Players")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public void AddOrUpdate(Player player)
        {
            if (FindById(player.Id) == null)
            {
                return;
            }

            Create(player);
        }

        public List<Player> GetAllBots()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.Bot }).ToList();
            }
        }

        public List<Player> GetAllDealer()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.Dealer }).ToList();
            }
        }

        public List<Player> GetAllUser()
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<Player>("SELECT * FROM Players WHERE PlayerType=@playertype", new { playertype = PlayerType.User }).ToList();
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

        public Player SearchPlayerWithName(string name)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<Player>("SELECT * FROM Players WHERE Name=@Name", new { Name = name }).SingleOrDefault();
            }
        }

    }
}
