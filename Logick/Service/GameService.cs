using BlackJack.BusinessLogic.Helpers;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.BusinessLogick.ViewModel.Enum;
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
            if(player == null)
            {
                throw new Exception("Player not found");
            }
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
            await DoRoundWithoutPlayerWithType(gameStatistics, deck, PlayerType.None);
            await DoRoundWithoutPlayerWithType(gameStatistics, deck, PlayerType.None);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DoRoundWithoutPlayerWithType(IEnumerable<PlayerInGameView> gameStatistics, DeckHelper deck, PlayerType playerType)
        {
            List<Turn> turns = new List<Turn>();
            foreach (var player in gameStatistics)
            {
                if (player.PlayerType == playerType || player.PlayerStatus != PlayerStatus.Play)
                {
                    continue;
                }
                turns.Add(DoTurn(player, deck));
            }
            await _turnRepository.Create(turns);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DoRoundWithPlayerType(IEnumerable<PlayerInGameView> gameStatistics, DeckHelper deck, PlayerType playerType)
        {
            List<Turn> turns = new List<Turn>();
            foreach (var player in gameStatistics)
            {
                if (player.PlayerType != playerType || player.PlayerStatus != PlayerStatus.Play)
                {
                    continue;
                }
                turns.Add(DoTurn(player, deck));
            }
            await _turnRepository.Create(turns);

            return gameStatistics;
        }

        private Turn DoTurn(PlayerInGameView player, DeckHelper deck)
        {
            CardView card = deck.GiveCard();
            Turn turn = new Turn();
            turn.GameId = player.GameId;
            turn.PlayerId = player.PlayerId;
            turn.CardLear = card.CardLear;
            turn.CardNumber = card.CardNumber;
            player.Cards.Add(card);

            return turn;
        }

        private int CountPoint(PlayerInGameView player)
        {
            CardHelper cardHelper = new CardHelper();
            player.Points = player.Cards.Sum(card => cardHelper.GetCardPoint(card));

            return player.Points;
        }

        private void CheckPoint(IEnumerable<PlayerInGameView> gameStatistics)
        {
            foreach (var player in gameStatistics)
            {
                player.Points = CountPoint(player);
                if (player.Points > _maxPoint)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            return;
        }

        private async Task<IEnumerable<PlayerInGameView>> InitializGameStatistics(long gameId)
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

        public async Task<IEnumerable<PlayerInGameView>> ContinuePlaying(long gameId, PlayerChoos choose)
        {
            IEnumerable<PlayerInGameView> gameStatistics = await InitializGameStatistics(gameId);

            if (choose == PlayerChoos.ContinueGame)
            {
                gameStatistics = await TakeCard(gameStatistics);
            }
            if (choose == PlayerChoos.StopGame)
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
            await DoRoundWithoutPlayerWithType(gameStatistics, deck, PlayerType.Dealer);
            CheckPoint(gameStatistics);

            return gameStatistics;
        }

        private async Task<IEnumerable<PlayerInGameView>> DropCard(IEnumerable<PlayerInGameView> gameStatistics)
        {
            DeckHelper deck = new DeckHelper(gameStatistics);
            var dealer = gameStatistics.SingleOrDefault(x => x.PlayerType == PlayerType.Dealer);
            if (dealer == null)
            {
                throw new Exception("Could not find dealer");
            }

            while (CountPoint(dealer) <= _minPoint)
            {
                await DoRoundWithPlayerType(gameStatistics, deck, PlayerType.Dealer);
            }
            if (CountPoint(dealer) > _maxPoint)
            {
                dealer.PlayerStatus = PlayerStatus.Lose;
            }

            return gameStatistics;
        }

        public async Task<IEnumerable<PlayerInGameView>> GetGameResult(IEnumerable<PlayerInGameView> gameStatistics)
        {
            Game game = await _gameRepository.FindById(gameStatistics.FirstOrDefault().GameId);
            game.GameStatus = GameStatus.Done;
            await _gameRepository.Update(game);

            var dealer = gameStatistics.SingleOrDefault(p => p.PlayerType == PlayerType.Dealer);

            if (dealer.PlayerStatus == PlayerStatus.Lose)
            {
                gameStatistics = LoseDealer(gameStatistics);

                return gameStatistics;
            }
                
            dealer.PlayerStatus = PlayerStatus.Won;

            foreach (var player in gameStatistics)
            {
                if (player.PlayerStatus != PlayerStatus.Lose && player.Points > dealer.Points)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    dealer.PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Points <= dealer.Points && player.PlayerType != PlayerType.Dealer)
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
            return (await _playerRepository.GetByType(PlayerType.User)).OrderByDescending(x => x.Name).Select(s => s.Name).ToList();
        }

        private async Task RegisterNewPlayer(Player player)
        {
            if (String.IsNullOrEmpty(player.Name))
            {
                return;
            }
            player.PlayerType = PlayerType.User;
            await _playerRepository.Create(player);
        }

        public async Task CheсkAndRegisterPlayer(string name)
        {
            if (await _playerRepository.SearchPlayerWithName(name) == null)
            {
                await RegisterNewPlayer(new Player { Name = name });
            }
        }

        private async Task<IEnumerable<PlayerInGameView>> CreatePlayersInGame(Game game)
        {
            var turns = await _turnRepository.GetAllTurns(game.Id);
            List<long> playersId = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<PlayerInGameView> playersInGame = new List<PlayerInGameView>();
            List<Player> players = await _playerRepository.SearchPlayersWithIds(playersId);
            if (players == null)
            {
                throw new Exception("Player not found");
            }
            foreach (var player in players)
            {                
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

        private List<CardView> GetPlayerCards(long playerId, List<Turn> turns)
        {
            var playerTurns = turns.Where(p => p.PlayerId == playerId);
            var playerCards = playerTurns.Select(k => new CardView { CardLear = k.CardLear, CardNumber = k.CardNumber }).ToList();

            return playerCards;
        }

        private async Task<List<PlayerInGameView>> GeneratePlayers(Player player, int botsNumber, long gameId)
        {
            List<PlayerInGameView> players = new List<PlayerInGameView>();
            List<Player> bots = await _playerRepository.GetByTypeNumber(PlayerType.Bot, botsNumber);
            Player dealer = (await _playerRepository.GetByTypeNumber(PlayerType.Dealer)).SingleOrDefault();
            players.Add(GetPlayerInGameView(player, gameId));
            players.Add(GetPlayerInGameView(dealer, gameId));
            players.AddRange(bots.Select(bot => GetPlayerInGameView(bot,gameId)));

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
            IEnumerable<GameResult> gameResults = gameStatistics.Select(x => new GameResult
                                                                            {
                                                                                PlayerId = x.PlayerId,
                                                                                GameId = x.GameId,
                                                                                PlayerStatus = x.PlayerStatus,

                                                                            });

            await _gameResultRepository.Create(gameResults);
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
            return IsPlayerLose(player) || IsDealerLose(player);
        }

        private bool IsPlayerLose(PlayerInGameView player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play;
        }

        private bool IsDealerLose(PlayerInGameView player)
        {
            return player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }

        private async Task<IEnumerable<PlayerInGameView>> CheckEndGame(IEnumerable<PlayerInGameView> gameStatistics)
        {
            if(gameStatistics.Where(p => p.PlayerType != PlayerType.Dealer).All(x => x.PlayerStatus == PlayerStatus.Lose))
            {
                gameStatistics.SingleOrDefault(x => x.PlayerType == PlayerType.Dealer).PlayerStatus = PlayerStatus.Won;

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

            return turn == null;
        }

        public async Task<IEnumerable<PlayerInGameView>> LoadGame(long gameId)
        {
            IEnumerable<PlayerInGameView> gameStatistics = await InitializGameStatistics(gameId);

            return gameStatistics;
        }
    }
}
