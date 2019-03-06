﻿using DataBaseControl.Entities;
using DataBaseControl.Repository.Interface;
using System.Collections.Generic;
using System.Linq;


namespace DataBaseControl.Repository
{
    public class PlayerRepository : DefaultGenericRepository<Player>, IPlayerRepository
    {

        private BlackJackContext _context;

        public PlayerRepository(BlackJackContext context) : base(context)
        {
            _context = context;
        }

        public bool IsAPlayer (Player player)
        {
            return _context.Players.Any(x => x.Name == player.Name);
        }

        public List<Player> GetAllUser()
        {
            return GetAllType(Entities.Enum.PlayerType.User);
        }

        public List<Player> GetAllBots()
        {
            return GetAllType(Entities.Enum.PlayerType.Bot);
        }

        public List<Player> GetAllDealer()
        {
            return GetAllType(Entities.Enum.PlayerType.Dealer);
        }

        private List<Player> GetAllType(Entities.Enum.PlayerType p)
        {
            return _context.Players.Where(x => x.PlayerType == p).ToList();
        }

        public void AddOrUpdate (Player player)
        {
            if (IsAPlayer(player))
            {
                return;
            }
            if (!IsAPlayer(player))
            {
                Create(player);
            }
        }

        public Player SearchPlayerWithName(string name)
        {
            return _context.Players.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
