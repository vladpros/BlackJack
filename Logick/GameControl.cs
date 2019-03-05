using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Entities.Enam;
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

        public long StartGame (Player player, int botsNumber)
        {
            Game game = new Game
            {
                GameStatus = GameStatus.InProgress,
                PlayerId = player.Id,
                BotsNumber = botsNumber
            };

            _game.Create(game);

            return game.Id;
        }

        private Card GiveCard(Deck deck)
        {
            int rand = _random.Next(0, deck.NumberCard);
            Card card = deck.Cards[rand];
            deck.Cards.RemoveAt(rand);

            return card;
        }

        private List<GameStats> CreatGameStats(Game game)
        {
            List<GameStats> gameStats = new List<GameStats>();
            gameStats = _data.GenPlayers(game.PlayerId, game.BotsNumber);
            foreach(var g in gameStats)
            {
                g.GameId = game.Id;
            }

            return gameStats;
        }

        public List<GameStats> DoFirstTwoRound(long gameId)
        {
            Deck deck = new Deck();
            Game game = _game.FindById(gameId);
            List<GameStats> gameStats = CreatGameStats(game);
            gameStats = DoRound(gameStats, deck);
            CheckPoint(gameStats);
            _game.Update(game);

            return gameStats;
        }

        private List<GameStats> DoRound(List<GameStats> gameStats, Deck deck)
        {
            DoRoundToPlayerType(gameStats, deck, PlayerType.User);
            DoRoundToPlayerType(gameStats, deck, PlayerType.User);
            DoRoundToPlayerType(gameStats, deck, PlayerType.Bot);
            DoRoundToPlayerType(gameStats, deck, PlayerType.Bot);
            DoRoundToPlayerType(gameStats, deck, PlayerType.Dealer);
            DoRoundToPlayerType(gameStats, deck, PlayerType.Dealer);

            return gameStats;
        }

        private List<GameStats> DoRoundToPlayerType(List<GameStats> gameStats, Deck deck, PlayerType playerType)
        {
            foreach (var player in gameStats)
            {
                if (_player.FindById(player.PlayerId).PlayerType == playerType && player.PlayerStatus == PlayerStatus.Play)
                {
                    Card card = DoTurn(player.PlayerId, player.GameId, deck);
                    player.Cards.Add(new Card
                    {
                        LearCard = card.LearCard,
                        NumberCard = card.NumberCard,
                    });
                }
            }

            return gameStats;
        }

        private Card DoTurn(long playerId, long gameId, Deck deck)
        {
            Card card = GiveCard(deck);
            Game game = _game.FindById(gameId);

            game.TurnNumber+=1;
            _turn.Create(
                new Turn
                {
                    GameId = game.Id,
                    PlayerId = playerId,
                    LearCard = card.LearCard,
                    NumberCard = card.NumberCard,
                });

            _game.Update(game);

            return new Card { LearCard = card.LearCard, NumberCard = card.NumberCard };
        }

        private int CountPoint(GameStats player)
        {
                player.Point = 0;
                foreach (var card in player.Cards)
                {
                    player.Point += GetCardPoint(card);
                }

            return player.Point;
        }

        private void CheckPoint(List<GameStats> gameStats)
        {
            foreach(var player in gameStats)
            {
                player.Point = CountPoint(player);
            }

            foreach (var player in gameStats)
            {
                if (player.Point > 21)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }
            
        }

        private int GetCardPoint(Card card)
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

        private List<GameStats> InitializationGameStats(long gameId)
        {
            Game game = _game.FindById(gameId);
            var gameStats = _data.GetGameStats(game);

            CheckPoint(gameStats);

            return gameStats;
        }
       
        public List<GameStats> ContinuePlay (long gameId, int choose)
        {
            Game game = _game.FindById(gameId);
            List<GameStats> gameStats = InitializationGameStats(gameId);

            if (choose == 1)
            {
                gameStats = TakeCard(gameStats);
            }
            if(choose == 2)
            {
                gameStats[_data.SearchUser(gameStats)].PlayerStatus = PlayerStatus.Wait;
                gameStats = DropCard(gameStats);
            }

            _game.Update(game);

            return gameStats;
        }

        private List<GameStats> TakeCard(List<GameStats> gameStats)
        {
            Deck deck = _data.GetDeck(gameStats);
            DoRoundToPlayerType(gameStats, deck, PlayerType.Bot);
            DoRoundToPlayerType(gameStats, deck, PlayerType.User);
            CheckPoint(gameStats);

            return gameStats;
        }

        public List<GameStats> DropCard(List<GameStats> gameStats)
        {
            Deck deck = _data.GetDeck(gameStats);
            int dealer = _data.SearchDealer(gameStats);
            while (CountPoint(gameStats[dealer]) <= 17)
            {
                DoRoundToPlayerType(gameStats, deck, PlayerType.Dealer);                
            }
            if(CountPoint(gameStats[dealer]) > 21)
            {
                gameStats[dealer].PlayerStatus = PlayerStatus.Lose;
            }

            return gameStats;
        }

        public List<GameStats> GetGameResult(long gameId)
        {
            List<GameStats> gameStats = InitializationGameStats(gameId);
            foreach(var player in gameStats)
            {
                if(player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose)
                {
                    gameStats = DealerLose(gameStats);
                    return gameStats;
                }
            }

            int dealerIndex = _data.SearchDealer(gameStats);
            gameStats[dealerIndex].PlayerStatus = PlayerStatus.Won;
            foreach (var player in gameStats)
            {
                if(player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStats[dealerIndex].Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStats[dealerIndex].PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point <= gameStats[dealerIndex].Point && player.PlayerType != PlayerType.Dealer)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            return gameStats;
        }

        public List<GameStats> DealerLose(List<GameStats> gameStats)
        {
            foreach(var player in gameStats)
            {
                if(player.PlayerStatus != PlayerStatus.Lose)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                }
            }

            return gameStats;
        }
    }
}
