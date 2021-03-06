﻿using BlackJack.DataAccess.Entities;
using System.Data.Entity;

namespace BlackJack.DataAccess
{
    public class BlackJackContext : DbContext
    {
        public BlackJackContext() : base("DbConnection")
        { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<GameResult> GameResults { get; set; }

    }
}