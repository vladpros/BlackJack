using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Repository;
using DataBaseControl.Repository.Interface;
using System;
using System.Collections.Generic;

namespace Logick
{
    public class GameControl
    {
        Random _random;
        IPlayerRepository _player;
        IGameRepository _game;
        BlackJackContext _db;

        public GameControl()
        {
            _db = new BlackJackContext();
            _random = new Random();
            _player = new PlayerRepository(_db);
        }

        public void TakeCard(Turn player)
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

            return new Game();
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
