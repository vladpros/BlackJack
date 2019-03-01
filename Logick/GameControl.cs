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
        private Random _random;
        private IPlayerRepository _player;
        private IGameRepository _game;
        private BlackJackContext _db;
        private ITurnRepository _turn;

        public GameControl()
        {
            _db = new BlackJackContext();
            _random = new Random();
            _player = new PlayerRepository(_db);
            _game = new GameRepository(_db);
        }

        public Game StartGame (Player player, int botsNumber)
        {
            Game game = new Game
            {
                GameStatus = (DataBaseControl.Entities.Enam.GameStatus)1,
                Players = GenPlayers(player, botsNumber)
            };

            _game.Create(game);

            return game;
        }

        private Card GiveCard(Deck deck)
        {
            int rand = _random.Next(0, deck.NumberCard);
            Card card = deck.Cards[rand];
            deck.Cards.RemoveAt(rand);

            return card;
        }

        private List<Player> GenPlayers (Player player, int botsNumber)
        {
            List<Player> players = new List<Player>();
            players.Add(player);
            List<Player> bots = _player.GetAllBots();

            for (int i = 0; i < botsNumber; i++)
            {
                int rand = _random.Next(0, bots.Count);
                players.Add(bots[rand]);
                bots.RemoveAt(rand);
            }

            List<Player> dealer = _player.GetAllDealer();
            players.Add(dealer[_random.Next(0, dealer.Count)]);

            return players;
        }

        private void DoFirstRound(Game game)
        {
            Deck deck = new Deck();
            foreach(var player in game.Players)
            {
                DoTurn(player, game, deck);
                DoTurn(player, game, deck);
            }
        }

        private void DoTurn(Player player, Game game, Deck deck)
        {
            Card card = GiveCard(deck);
            _turn.Create(
                new Turn
                {
                    Game = game,
                    Player = player,
                    LearCard = card.LearCard,
                    NumberCard = card.NumberCard,
                });
        }
    }
}
