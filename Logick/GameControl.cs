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
        private DataControl _data;

        public GameControl()
        {
            _db = new BlackJackContext();
            _random = new Random();
            _player = new PlayerRepository(_db);
            _game = new GameRepository(_db);
            _turn = new TurnReposytory(_db);
            _data = new DataControl();
        }

        public Game StartGame (Player player, int botsNumber)
        {
            Game game = new Game
            {
                GameStatus = (DataBaseControl.Entities.Enam.GameStatus)1,
                Players = GenPlayers(player, botsNumber),
                UserId = player.Id
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

        private List<Player> GenPlayers(Player player, int botsNumber)
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

        public List<GameStats> DoFirstRound(Game game)
        {
            Deck deck = new Deck();
            foreach (var player in game.Players)
            {
                DoTurn(player, game, deck);
                DoTurn(player, game, deck);
            }

            var gameStats = CreatGameStats(game);
            _game.Update(game);

            return gameStats;
        }

        private List<GameStats> CreatGameStats(Game game)
        {
            var turns = _data.GetAllTurns(game);
            turns = CountPoint(game, turns);
            CheckPoint(game, turns);
            return turns;
        }

        private void DoTurn(Player player, Game game, Deck deck)
        {
            Card card = GiveCard(deck);
            game.TurnNumber+=1;
            _turn.Create(
                new Turn
                {
                    GameId = game.Id,
                    PlayerId = player.Id,
                    LearCard = card.LearCard,
                    NumberCard = card.NumberCard,
                });
        }

        private List<GameStats> CountPoint(Game game, List<GameStats> gameStats)
        {
            var c = new Card { NumberCard= DataBaseControl.Entities.Enam.NumberCard.Five, LearCard = DataBaseControl.Entities.Enam.LearCard.Diamond };
            foreach (var player in gameStats)
            {
                foreach (var card in player.Cards)
                {
                    player.Point += GetCardPoint(card);
                }
            }

            return gameStats;
        }

        private void CheckPoint(Game game, List<GameStats> gameStats)
        {
            foreach(var player in gameStats)
            {
                if (player.Point > 21)
                {
                    player.PlayerStatus = DataBaseControl.Entities.Enam.PlayerStatus.Lose;
                }
            }
        }

        private int GetCardPoint (Card card)
        {
            if ((int)card.NumberCard < 10)
            {
                return (int)card.NumberCard;
            }
            if ((int)card.NumberCard >= 10)
            {
                return 10;
            }

            return (int)card.NumberCard;
        }


    }
}
