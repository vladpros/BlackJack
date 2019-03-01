using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Entities.Enam;
using DataBaseControl.Repository;
using DataBaseControl.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logick
{
    public class DataControl
    {
        private BlackJackContext _db;
        private IPlayerRepository _player;

        public DataControl()
        {
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
        }

        public List<Player> GetUserOrdered()
        {
            return _player.GetAllUser().OrderByDescending(x => x.Name).ToList();
        }

        private bool RegisterNewPlayer(Player player)
        {
            if (player.Name != null)
            {
                player.PlayerType = PlayerType.User;
                _player.Create(player);

                return true;
            }

            return false;
        }

        public Player SearchPlayerWithName(string name)
        {
            return _player.SearchPlayerWithName(name);
        }

        public void PlayerChecked (string name)
        {
            if (_player.SearchPlayerWithName(name) == null)
            {
                RegisterNewPlayer(new Player { Name = name });
            }
        }

    }
}
