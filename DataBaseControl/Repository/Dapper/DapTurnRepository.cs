﻿using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Repository.Interface;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataBaseControl.Repository.Dapper
{
    public class DapTurnRepository : DapDefaultGenericRepository<Turn>, ITurnRepository
    {

        private readonly string _conString;

        public DapTurnRepository() : base("Turns")
        {
            _conString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        }

        public List<Turn> GetAllTurns(Game game)
        {
            using (IDbConnection cn = new SqlConnection(_conString))
            {
                return cn.Query<Turn>("SELECT * FROM Turns WHERE GameId=@gameId", new { gameId = game.Id }).ToList();
            }
        }
    }
}
