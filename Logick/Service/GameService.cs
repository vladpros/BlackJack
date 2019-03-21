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
        private Random _random;
        private IPlayerRepository _playerRepository;
        private IGameRepository _gameRepository;
        private ITurnRepository _turnRepository;
        private int _maxPoint;
        private int _minPoint;
        private IGameResultRepository _gameResultRepository;
        private DeckHelper _deckHelper;

        public GameService(IGameRepository gameRepository, ITurnRepository turnRepository, IPlayerRepository playerRepository, IGameResultRepository gameResultRepository)
        {
            _random = new Random();
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _turnRepository = turnRepository;
            _maxPoint = 21;
            _minPoint = 17;
            _gameResultRepository = gameResultRepository;
            _deckHelper = new DeckHelper();
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

        private async Task<IEnumerable<PlayerInGameViewModel>> CreatGameStat(Game game)
        {
            IEnumerable<PlayerInGameViewModel> gameStat = new List<PlayerInGameViewModel>();
            Player player = await _playerRepository.FindById(game.PlayerId);
            gameStat = await GeneratePlayers(player, game.BotsNumber, game.Id);

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGameViewModel>> DoFirstTwoRound(long gameId)
        {
            DeckHelper deck = new DeckHelper();
            Game game = await _gameRepository.FindById(gameId);
            IEnumerable<PlayerInGameViewModel> gameStat = await CreatGameStat(game);

            gameStat = await DoRound(gameStat, deck);
            CheckPoint(gameStat);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> DoRound(IEnumerable<PlayerInGameViewModel> gameStat, DeckHelper deck)
        {
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> DoRoundWithoutPlayerType(IEnumerable<PlayerInGameViewModel> gameStat, DeckHelper deck, PlayerType playerType)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerType != playerType && player.PlayerStatus == PlayerStatus.Play)
                {
                    CardHelper card = await DoTurn(player.PlayerId, player.GameId, deck);
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> DoRoundWithPlayerType(IEnumerable<PlayerInGameViewModel> gameStat, DeckHelper deck, PlayerType playerType1)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerType == playerType1 && player.PlayerStatus == PlayerStatus.Play)
                {
                    CardHelper card = await Task.Run(() => DoTurn(player.PlayerId, player.GameId, deck));
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<CardHelper> DoTurn(long playerId, long gameId, DeckHelper deck)
        {
            CardHelper card = deck.GiveCard();
            Game game = await _gameRepository.FindById(gameId);

            await _turnRepository.Create(
                new Turn
                {
                    GameId = game.Id,
                    PlayerId = playerId,
                    CardLear = card.CardLear,
                    CardNumber = card.CardNumber,
                });

            return new CardHelper { CardLear = card.CardLear, CardNumber = card.CardNumber };
        }

        private int CountPoint(PlayerInGameViewModel player)
        {
            player.Point = 0;
            foreach (var card in player.Cards)
            {
                player.Point += _deckHelper.GetCardPoint(card);
            }

            return player.Point;
        }

        private void CheckPoint(IEnumerable<PlayerInGameViewModel> playersInGame)
        {
            foreach (var player in playersInGame)
            {
                player.Point = CountPoint(player);
                if (player.Point > _maxPoint)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            return;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> InitializationGameStat(long gameId)
        {
            Game game = await _gameRepository.FindById(gameId);
            IEnumerable<PlayerInGameViewModel> gameStat = new List<PlayerInGameViewModel>();
            gameStat = await CreatPlayersInGame(game);

            CheckPoint(gameStat);

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGameViewModel>> ContinuePlay(long gameId, long choose)
        {
            IEnumerable<PlayerInGameViewModel> gameStat = await InitializationGameStat(gameId);
            int continueGame = 1;
            int stopGame = 2;

            if (choose == continueGame)
            {
                gameStat = await TakeCard(gameStat);
            }
            if (choose == stopGame)
            {
                gameStat.ToList().Find(p => p.PlayerType == PlayerType.User).PlayerStatus = PlayerStatus.Wait;
                gameStat = await DropCard(gameStat);
            }

            gameStat = await CheckEndGame(gameStat);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> TakeCard(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            DeckHelper deck = new DeckHelper(gameStat);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.Dealer);
            CheckPoint(gameStat);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> DropCard(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            DeckHelper deck = new DeckHelper(gameStat);
            int dealerIndex = gameStat.ToList().FindIndex(p => p.PlayerType == PlayerType.Dealer);

            while (CountPoint(gameStat.ElementAtOrDefault(dealerIndex)) <= _minPoint)
            {
                await DoRoundWithPlayerType(gameStat, deck, PlayerType.Dealer);
            }
            if (CountPoint(gameStat.ElementAtOrDefault(dealerIndex)) > _maxPoint)
            {
                gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
            }

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGameViewModel>> GetGameResult(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            Game game = (await _gameRepository.FindById(gameStat.FirstOrDefault().GameId));
            game.GameStatus = GameStatus.Done;
            await _gameRepository.Update(game);

            int dealerIndex = gameStat.ToList().FindIndex(p => p.PlayerType == PlayerType.Dealer);

            if (gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus == PlayerStatus.Lose)
            {
                gameStat = LoseDealer(gameStat);
                return gameStat;
            }


            gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Won;

            foreach (var player in gameStat)
            {
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStat.ElementAtOrDefault(dealerIndex).Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStat.ElementAtOrDefault(dealerIndex).Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point <= gameStat.ElementAtOrDefault(dealerIndex).Point && player.PlayerType != PlayerType.Dealer)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            await SaveResult(gameStat);

            return gameStat;
        }

        private IEnumerable<PlayerInGameViewModel> LoseDealer(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerStatus != PlayerStatus.Lose)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                }
            }

            return gameStat;
        }

        public async Task<List<string>> GetUserOrdered()
        {
            return (await _playerRepository.GetByType(PlayerType.User)).OrderByDescending(x => x.Name).Select(s => s.Name).ToList();
        }

        private async Task<bool> RegisterNewPlayer(Player player)
        {
            if (player.Name == null)
            {
                return false;
            }
            player.PlayerType = PlayerType.User;
            await _playerRepository.Create(player);

            return true;
        }

        public async Task ChekPlayer(string name)
        {
            if ((await _playerRepository.SearchPlayerWithName(name)) == null)
            {
                await RegisterNewPlayer(new Player { Name = name });
            }
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> CreatPlayersInGame(Game game)
        {
            var turns = await _turnRepository.GetAllTurns(game.Id);
            List<long> players = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<PlayerInGameViewModel> playersInGame = new List<PlayerInGameViewModel>();

            foreach (var playerId in players)
            {
                Player player = await _playerRepository.FindById(playerId);
                PlayerInGameViewModel playerInGame = new PlayerInGameViewModel();
                playerInGame.PlayerId = player.Id;
                playerInGame.PlayerName = player.Name;
                playerInGame.GameId = game.Id;
                playerInGame.Cards = GetPlayerCard(player.Id, turns);
                playerInGame.PlayerType = player.PlayerType;
                playersInGame.Add(playerInGame);
            }

            return playersInGame;
        }

        private List<CardHelper> GetPlayerCard(long playerId, List<Turn> turns)
        {
            var result = turns.Where(p => p.PlayerId == playerId);
            var result1 = result.Select(k => new CardHelper { CardLear = k.CardLear, CardNumber = k.CardNumber }).ToList();
            return result1;
        }

        private async Task<List<PlayerInGameViewModel>> GeneratePlayers(Player player, int botsNumber, long gameId)
        {
            int rand;
            List<PlayerInGameViewModel> players = new List<PlayerInGameViewModel>();
            PlayerInGameViewModel playerTemp = new PlayerInGameViewModel();
            playerTemp.PlayerId = player.Id;
            playerTemp.GameId = gameId;
            playerTemp.PlayerName = player.Name;
            playerTemp.PlayerType = PlayerType.User;
            players.Add(playerTemp);

            List<Player> bots = await _playerRepository.GetByType(PlayerType.Bot);

            for (int i = 0; i < botsNumber; i++)
            {
                PlayerInGameViewModel playerTemp1 = new PlayerInGameViewModel();
                rand = _random.Next(0, bots.Count);
                playerTemp1.PlayerId = bots[rand].Id;
                playerTemp1.PlayerName = bots[rand].Name;
                playerTemp1.GameId = gameId;
                playerTemp1.PlayerType = PlayerType.Bot;
                players.Add(playerTemp1);
                bots.RemoveAt(rand);
            }

            List<Player> dealer = await _playerRepository.GetByType(PlayerType.Dealer);

            rand = _random.Next(0, dealer.Count);
            PlayerInGameViewModel playerTemp3 = new PlayerInGameViewModel();
            playerTemp3.PlayerId = dealer[rand].Id;
            playerTemp3.PlayerName = dealer[rand].Name;
            playerTemp3.PlayerType = PlayerType.Dealer;
            playerTemp3.GameId = gameId;
            players.Add(playerTemp3);

            return players;
        }

        private async Task SaveResult(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            foreach (var player in gameStat)
            {
                await _gameResultRepository.Create(new GameResult
                {
                    PlayerId = player.PlayerId,
                    GameId = player.GameId,
                    PlayerStatus = player.PlayerStatus
                });
            }
        }

        public async Task<Game> GetGame(long gameId)
        {
            var result = await _gameRepository.FindById(gameId);

            return result;
        }

        private bool IsEndGame(PlayerInGameViewModel player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play || player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }

        private async Task<IEnumerable<PlayerInGameViewModel>> CheckEndGame(IEnumerable<PlayerInGameViewModel> gameStat)
        {
            if(gameStat.Where(p => p.PlayerType != PlayerType.Dealer).All(x => x.PlayerStatus == PlayerStatus.Lose))
            {
                var dealer = gameStat.ToList().FindIndex(x => x.PlayerType == PlayerType.Dealer);
                gameStat.ElementAt(dealer).PlayerStatus = PlayerStatus.Won;

                return gameStat;
            }

            foreach (var player in gameStat)
            {
                if (IsEndGame(player))
                {
                    await DropCard(gameStat);

                    return await GetGameResult(gameStat);
                }
            }

            return gameStat;
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

        public async Task<IEnumerable<PlayerInGameViewModel>> LoadGame(long gameId)
        {
            IEnumerable<PlayerInGameViewModel> gameStat = await InitializationGameStat(gameId);

            return gameStat;
        }
    }
}
