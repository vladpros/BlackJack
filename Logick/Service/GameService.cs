using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic.Service
{
    public class GameService : IGameService
    {
        private IPlayerRepository _playerRepository;
        private IGameRepository _gameRepository;
        private ITurnRepository _turnRepository;
        private int _maxPoint;
        private int _minPoint;
        private IGameResultRepository _gameResultRepository;

        public GameService(IGameRepository gameRepository, ITurnRepository turnRepository, IPlayerRepository playerRepository, IGameResultRepository gameResultRepository)
        {
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _turnRepository = turnRepository;
            _maxPoint = 21;
            _minPoint = 17;
            _gameResultRepository = gameResultRepository;
        }

        public async Task<long> StartGame(string playerName, int botsNumber)
        {
            Player player = await _playerRepository.SearchPlayerWithName(playerName);
            Game game = new Game();
            game.GameStatus = GameStatus.InProgress;
            game.PlayerId = player.Id;
            game.BotsNumber = botsNumber;

            return await _gameRepository.Create(game); 
        }

        private async Task<IEnumerable<PlayerInGameView>> CreateGameStatistics(Game game)
        {
            IEnumerable<PlayerInGameView> gameStatistics = new List<PlayerInGameView>();
            Player player = await _playerRepository.FindById(game.PlayerId);
            if (player == null)
            {
                throw new Exception("Player Not Found");
            }
            gameStatistics = await GeneratePlayers(player, game.BotsNumber, game.Id);

            return gameStatistics;
        }

        public async Task<IEnumerable<PlayerInGameView>> DoFirstTwoRounds(long gameId)
        {
            DeckHelper deck = new DeckHelper();
            Game game = await _gameRepository.FindById(gameId);
            if (game == null)
            {
                throw new Exception("Game not found");
            }
            IEnumerable<PlayerInGameView> gameStatistics = await CreateGameStatistics(game);
            gameStatistics = await DoRound(gameStatistics, deck);
            CheckPoint(gameStatistics);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DoRound(IEnumerable<PlayerInGameView> gameStatistics, DeckHelper deck)
        {
            await DoRoundWithoutPlayerType(gameStatistics, deck, PlayerType.None);
            await DoRoundWithoutPlayerType(gameStatistics, deck, PlayerType.None);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DoRoundWithoutPlayerType(IEnumerable<PlayerInGameView> gameStatistics, DeckHelper deck, PlayerType playerType)
        {
            foreach (var player in gameStatistics)
            {
                if (!(player.PlayerType != playerType && player.PlayerStatus == PlayerStatus.Play))
                {
                    continue;
                }
                CardHelper card = await DoTurn(player.PlayerId, player.GameId, deck);
                player.Cards.Add(card);
            }

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DoRoundWithPlayerType(IEnumerable<PlayerInGameView> gameStatistics, DeckHelper deck, PlayerType playerType)
        {
            foreach (var player in gameStatistics)
            {
                if (!(player.PlayerType == playerType && player.PlayerStatus == PlayerStatus.Play))
                {
                    continue;
                }
                CardHelper card = await Task.Run(() => DoTurn(player.PlayerId, player.GameId, deck));
                player.Cards.Add(card);
            }

            return gameStatistics;
        }

        private async Task<CardHelper> DoTurn(long playerId, long gameId, DeckHelper deck)
        {
            CardHelper card = deck.GiveCard();
            Turn turn = new Turn();
            turn.GameId = gameId;
            turn.PlayerId = playerId;
            turn.CardLear = card.CardLear;
            turn.CardNumber = card.CardNumber;
            await _turnRepository.Create(turn);

            return new CardHelper { CardLear = card.CardLear, CardNumber = card.CardNumber };
        }

        private int CountPoint(PlayerInGameView player)
        {
            CardHelper cardHelper = new CardHelper();
            player.Point = 0;
            foreach (var card in player.Cards)
            {
                player.Point += cardHelper.GetCardPoint(card);
            }

            return player.Point;
        }

        private void CheckPoint(IEnumerable<PlayerInGameView> gameStatistics)
        {
            foreach (var player in gameStatistics)
            {
                player.Point = CountPoint(player);
                if (player.Point > _maxPoint)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            return;
        }

        private async Task<IEnumerable<PlayerInGameView>> InitializationGameStatistics(long gameId)
        {
            Game game = await _gameRepository.FindById(gameId);
            if (game == null)
            {
                throw new Exception("Game not found");
            }
            IEnumerable<PlayerInGameView> gameStatistics = new List<PlayerInGameView>();
            gameStatistics = await CreatePlayersInGame(game);
            CheckPoint(gameStatistics);

            return gameStatistics;
        }

        public async Task<IEnumerable<PlayerInGameView>> ContinuePlaying(long gameId, long choose)
        {
            IEnumerable<PlayerInGameView> gameStatistics = await InitializationGameStatistics(gameId);
            int continueGame = 1;
            int stopGame = 2;
            if (choose == continueGame)
            {
                gameStatistics = await TakeCard(gameStatistics);
            }
            if (choose == stopGame)
            {
                gameStatistics.ToList().Find(p => p.PlayerType == PlayerType.User).PlayerStatus = PlayerStatus.Wait;
                gameStatistics = await DropCard(gameStatistics);
            }
            gameStatistics = await CheckEndGame(gameStatistics);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> TakeCard(IEnumerable<PlayerInGameView> gameStatistics)
        {
            DeckHelper deck = new DeckHelper(gameStatistics);
            await DoRoundWithoutPlayerType(gameStatistics, deck, PlayerType.Dealer);
            CheckPoint(gameStatistics);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DropCard(IEnumerable<PlayerInGameView> gameStatistics)
        {
            DeckHelper deck = new DeckHelper(gameStatistics);
            int dealerIndex = gameStatistics.ToList().FindIndex(p => p.PlayerType == PlayerType.Dealer);

            while (CountPoint(gameStatistics.ElementAtOrDefault(dealerIndex)) <= _minPoint)
            {
                await DoRoundWithPlayerType(gameStatistics, deck, PlayerType.Dealer);
            }
            if (CountPoint(gameStatistics.ElementAtOrDefault(dealerIndex)) > _maxPoint)
            {
                gameStatistics.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
            }

            return gameStatistics;
        }

        public async Task<IEnumerable<PlayerInGameView>> GetGameResult(IEnumerable<PlayerInGameView> gameStatistics) // Enter!!!!!
        {
            Game game = await _gameRepository.FindById(gameStatistics.FirstOrDefault().GameId);
            game.GameStatus = GameStatus.Done;
            await _gameRepository.Update(game);

            int dealerIndex = gameStatistics.ToList().FindIndex(p => p.PlayerType == PlayerType.Dealer);

            if (gameStatistics.ElementAtOrDefault(dealerIndex).PlayerStatus == PlayerStatus.Lose)
            {
                gameStatistics = LoseDealer(gameStatistics);

                return gameStatistics;
            }


            gameStatistics.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Won;

            foreach (var player in gameStatistics)
            {
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStatistics.ElementAtOrDefault(dealerIndex).Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStatistics.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStatistics.ElementAtOrDefault(dealerIndex).Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStatistics.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point <= gameStatistics.ElementAtOrDefault(dealerIndex).Point && player.PlayerType != PlayerType.Dealer)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            await SaveResult(gameStatistics);

            return gameStatistics;
        }

        private IEnumerable<PlayerInGameView> LoseDealer(IEnumerable<PlayerInGameView> gameStatistics)
        {
            foreach (var player in gameStatistics)
            {
                if (player.PlayerStatus != PlayerStatus.Lose)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                }
            }

            return gameStatistics;
        }

        public async Task<List<string>> GetOrderedUsers()
        {
            int numberUser = 1000;
            return (await _playerRepository.GetByType(PlayerType.User, numberUser)).OrderByDescending(x => x.Name).Select(s => s.Name).ToList();
        }

        private async Task RegisterNewPlayer(Player player)
        {
            if (player.Name == null)
            {
                return;
            }
            player.PlayerType = PlayerType.User;
            await _playerRepository.Create(player);
        }

        public async Task CheсkPlayer(string name)
        {
            if (await _playerRepository.SearchPlayerWithName(name) == null)
            {
                await RegisterNewPlayer(new Player { Name = name });
            }
        }

        private async Task<IEnumerable<PlayerInGameView>> CreatePlayersInGame(Game game)
        {
            var turns = await _turnRepository.GetAllTurns(game.Id);
            List<long> players = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<PlayerInGameView> playersInGame = new List<PlayerInGameView>();

            foreach (var playerId in players)
            {
                Player player = await _playerRepository.FindById(playerId);
                if (player == null)
                {
                    throw new Exception("Player not found");
                }
                PlayerInGameView playerInGame = new PlayerInGameView();
                playerInGame.PlayerId = player.Id;
                playerInGame.PlayerName = player.Name;
                playerInGame.GameId = game.Id;
                playerInGame.Cards = GetPlayerCards(player.Id, turns);
                playerInGame.PlayerType = player.PlayerType;
                playersInGame.Add(playerInGame);
            }

            return playersInGame;
        }

        private List<CardHelper> GetPlayerCards(long playerId, List<Turn> turns)
        {
            var playerTurns = turns.Where(p => p.PlayerId == playerId);
            var playerCards = playerTurns.Select(k => new CardHelper { CardLear = k.CardLear, CardNumber = k.CardNumber }).ToList();

            return playerCards;
        }

        private async Task<List<PlayerInGameView>> GeneratePlayers(Player player, int botsNumber, long gameId)
        {
            int dealerNumber = 1;
            List<PlayerInGameView> players = new List<PlayerInGameView>();
            List<Player> bots = await _playerRepository.GetByType(PlayerType.Bot, botsNumber);
            List<Player> dealer = await _playerRepository.GetByType(PlayerType.Dealer, dealerNumber);
            players.Add(GetPlayerInGameView(player, gameId));
            players.Add(GetPlayerInGameView(dealer.FirstOrDefault(), gameId));
            foreach(var bot in bots)
            {
                players.Add(GetPlayerInGameView(bot,gameId));
            }

            return players;
        }

        private PlayerInGameView GetPlayerInGameView(Player player, long gameId)
        {
            PlayerInGameView playerInGameView = new PlayerInGameView();
            playerInGameView.PlayerId = player.Id;
            playerInGameView.GameId = gameId;
            playerInGameView.PlayerName = player.Name;
            playerInGameView.PlayerType = PlayerType.User;

            return playerInGameView;
        }

        private async Task SaveResult(IEnumerable<PlayerInGameView> gameStatistics)
        {
            foreach (var player in gameStatistics)
            {
                GameResult gameResult = new GameResult();
                gameResult.PlayerId = player.PlayerId;
                gameResult.GameId = player.GameId;
                gameResult.PlayerStatus = player.PlayerStatus;
                await _gameResultRepository.Create(gameResult);
            }
        }

        public async Task<Game> GetGame(long gameId)
        {
            var game = await _gameRepository.FindById(gameId);
            if (game == null)
            {
                throw new Exception("Game not found");
            }

            return game;
        }

        private bool IsEndGame(PlayerInGameView player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play || player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }

        private async Task<IEnumerable<PlayerInGameView>> CheckEndGame(IEnumerable<PlayerInGameView> gameStatistics)
        {
            if(gameStatistics.Where(p => p.PlayerType != PlayerType.Dealer).All(x => x.PlayerStatus == PlayerStatus.Lose))
            {
                var dealer = gameStatistics.ToList().FindIndex(x => x.PlayerType == PlayerType.Dealer);
                gameStatistics.ElementAt(dealer).PlayerStatus = PlayerStatus.Won;

                return gameStatistics;
            }

            foreach (var player in gameStatistics)
            {
                if (!IsEndGame(player))
                {
                    continue;
                }
                await DropCard(gameStatistics);

                return await GetGameResult(gameStatistics);
            }

            return gameStatistics;
        }

        public async Task<bool> IsNewGame(long gameId)
        {
            var turn = (await _turnRepository.GetAllTurns(gameId)).FirstOrDefault();
            if (turn == null)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<PlayerInGameView>> LoadGame(long gameId)
        {
            IEnumerable<PlayerInGameView> gameStatistics = await InitializationGameStatistics(gameId);

            return gameStatistics;
        }
    }
}
