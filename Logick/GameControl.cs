using DataBaseControl.Entities;
using DataBaseControl.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logick
{
    public class GameControl
    {
        Random _random;
        PlayerRepository _player;
        GameRepository _game;
        DataBaseControl.BlackJackContext _db;

        public GameControl()
        {
            _db = new DataBaseControl.BlackJackContext();
            _random = new Random();
            _player = new PlayerRepository(_db);
        }

        public void TakeCard(PlayerInTurn player)
        {
              
        }

        public Game StartGame (Player player, int botsNumber)
        {
            Game game = new Game
            {
                GameStatus = (DataBaseControl.Entities.Enam.GameStatus)1,
                Players = GenPlayers(player, botsNumber)
            };

            _game.Create(game);



        }

        public List<Player> GenPlayers (Player player, int botsNumber)
        {
            List<Player> players = new List<Player>();
            players.Add(player);
            List<Player> bots = _player.GetAllBots();

            for (int i = 0; i < botsNumber; i++)
            {
                int k = _random.Next(0, bots.Count);
                players.Add(bots[k]);
                bots.RemoveAt(k);
            }

            List<Player> dealer = _player.GetAllDealer();
            players.Add(bots[_random.Next(0, dealer.Count)]);

            return players;
        }
    }
}
