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

            DoRound(game, deck);
            DoRound(game, deck);
            List<GameStats> gameStats = CreatGameStats(game);
            

            _game.Update(game);


            return gameStats;
        }

        private List<GameStats> CreatGameStats(Game game)
        {
            var gameStats = _data.GetAllTurns(game);
            CheckPoint(game, gameStats);

            return gameStats;
        }

        private void DoRound(Game game, Deck deck)
        {
            foreach (var player in game.Players)
            {
                DoTurn(player, game, deck);
            }
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
            gameStats = CountPoint(game, gameStats);

            foreach (var player in gameStats)
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

        public List<GameStats> ContinuePlay (Game game, int choose)
        {
            List<GameStats> gameStats = new List<GameStats>();

            if(choose == 1)
            {
                gameStats = TakeCard(game);
            }
            if(choose == 2)
            {

            }

            return gameStats;
        }

        private List<GameStats> TakeCard(Game game)
        {
            List<GameStats> gameStats = CreatGameStats(game);
            Deck deck = _data.GetDeck(gameStats);
            DoRoundToPlayerType(game, deck, PlayerType.Bot);
            DoRoundToPlayerType(game, deck, PlayerType.User);
            gameStats = CreatGameStats(game);


            return gameStats;
        }

        private void DoRoundToPlayerType(Game game, Deck deck, PlayerType playerType)
        {
            foreach (var player in game.Players)
            {
                if (player.PlayerType == playerType)
                {
                    DoTurn(player, game, deck);
                }
            }
        }
    }
}
