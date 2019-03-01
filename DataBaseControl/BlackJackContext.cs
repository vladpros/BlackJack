﻿using DataBaseControl.Entities;
using System.Data.Entity;

namespace DataBaseControl
{
    public class BlackJackContext : DbContext
    {

        public BlackJackContext() : base("DbConnection")
        { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Round> Turns { get; set; }
        public DbSet<Turn> PlayerInTurns { get; set; }

    }
}