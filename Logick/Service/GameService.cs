using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.DataAccess.Repositories.Interfaces;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BlackJack.BusinessLogic.Helpers;

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

        public async Task<long> StartGame(Player player, int botsNumber)
        {
            Game game = new Game
            {
                GameStatus = GameStatus.InProgress,
                PlayerId = player.Id,
                BotsNumber = botsNumber
            };

            return await _gameRepository.Create(game); ;
        }

        private async Task<IEnumerable<PlayerInGame>> CreatGameStat(Game game)
        {
            IEnumerable<PlayerInGame> gameStat = new List<PlayerInGame>();
            Player player = await _playerRepository.FindById(game.PlayerId);
            gameStat = await GenPlayers(player, game.BotsNumber, game.Id);

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGame>> DoFirstTwoRound(long gameId)
        {
            DeckHelper deck = new DeckHelper();
            Game game = await _gameRepository.FindById(gameId);
            IEnumerable<PlayerInGame> gameStat = await CreatGameStat(game);

            gameStat = await DoRound(gameStat, deck);
            CheckPoint(gameStat);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGame>> DoRound(IEnumerable<PlayerInGame> gameStat, DeckHelper deck)
        {
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGame>> DoRoundWithoutPlayerType(IEnumerable<PlayerInGame> gameStat, DeckHelper deck, PlayerType playerType)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerType != playerType && player.PlayerStatus == PlayerStatus.Play)
                {
                    Card card = await DoTurn(player.PlayerId, player.GameId, deck);
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGame>> DoRoundWithoutPlayerType(IEnumerable<PlayerInGame> gameStat, DeckHelper deck, PlayerType playerType1, PlayerType playerType2)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerType != playerType1 && player.PlayerType != playerType2 && player.PlayerStatus == PlayerStatus.Play)
                {
                    Card card = await Task.Run(() => DoTurn(player.PlayerId, player.GameId, deck));
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<Card> DoTurn(long playerId, long gameId, DeckHelper deck)
        {
            Card card = await Task.Run(() => _deckHelper.GiveCard(deck));
            Game game = await _gameRepository.FindById(gameId);

            await _turnRepository.Create(
                new Turn
                {
                    GameId = game.Id,
                    PlayerId = playerId,
                    LearCard = card.CardLear,
                    NumberCard = card.CardNumber,
                });

            return new Card { CardLear = card.CardLear, CardNumber = card.CardNumber };
        }

        private int CountPoint(PlayerInGame player)
        {
            player.Point = 0;
            foreach (var card in player.Cards)
            {
                player.Point += _deckHelper.GetCardPoint(card);
            }

            return player.Point;
        }

        private void CheckPoint(IEnumerable<PlayerInGame> playersInGame)
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

        public async Task<IEnumerable<PlayerInGame>> InitializationGameStat(long gameId)
        {
            Game game = await _gameRepository.FindById(gameId);
            IEnumerable<PlayerInGame> gameStat = new List<PlayerInGame>();
            gameStat = await PlayersInGame(game);

            CheckPoint(gameStat);

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGame>> ContinuePlay(long gameId, long choose)
        {
            Game game = await _gameRepository.FindById(gameId);
            IEnumerable<PlayerInGame> gameStat = await InitializationGameStat(gameId);
            int continueGame = 1;
            int stopGame = 2;

            if (choose == continueGame)
            {
                gameStat = await TakeCard(gameStat);
            }
            if (choose == stopGame)
            {
                gameStat.ElementAtOrDefault(await SearchUser(gameStat)).PlayerStatus = PlayerStatus.Wait;
                gameStat = await DropCard(gameStat);
            }

            await _gameRepository.Update(game);

            return gameStat;
        }

        private async Task<IEnumerable<PlayerInGame>> TakeCard(IEnumerable<PlayerInGame> gameStat)
        {
            DeckHelper deck = new DeckHelper();
            deck.GetDeck(gameStat);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.Dealer);
            CheckPoint(gameStat);

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGame>> DropCard(IEnumerable<PlayerInGame> gameStat)
        {
            DeckHelper deck = new DeckHelper();
            deck.GetDeck(gameStat);
            int dealerIndex = await SearchDealer(gameStat);

            while (CountPoint(gameStat.ElementAtOrDefault(dealerIndex)) <= _minPoint)
            {
                await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.Bot, PlayerType.User);
            }
            if (CountPoint(gameStat.ElementAtOrDefault(dealerIndex)) > _maxPoint)
            {
                gameStat.ElementAtOrDefault(dealerIndex).PlayerStatus = PlayerStatus.Lose;
            }

            return gameStat;
        }

        public async Task<IEnumerable<PlayerInGame>> GetGameResult(long gameId)
        {
            IEnumerable <PlayerInGame> gameStat = await InitializationGameStat(gameId);
            Game game = (await _gameRepository.FindById(gameId));
            game.GameStatus = GameStatus.Done;
            await _gameRepository.Update(game);
            foreach (var player in gameStat)
            {
                if (player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose)
                {
                    gameStat = DealerLose(gameStat);
                    return gameStat;
                }
            }

            int dealerIndex = await SearchDealer(gameStat);
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
            
            await SaveWinner(gameStat);

            return gameStat;
        }

        private IEnumerable<PlayerInGame> DealerLose(IEnumerable<PlayerInGame> gameStat)
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

        private bool RegisterNewPlayer(Player player)
        {
            if (player.Name != null)
            {
                player.PlayerType = PlayerType.User;
                _playerRepository.Create(player);

                return true;
            }

            return false;
        }

        public async Task<Player> SearchPlayerWithName(string name)
        {
            return await _playerRepository.SearchPlayerWithName(name);
        }

        public async Task PlayerChecked(string name)
        {
            if ((await SearchPlayerWithName(name)) == null)
            {
                RegisterNewPlayer(new Player { Name = name });
            }
        }

        private async Task<List<PlayerInGame>> PlayersInGame(Game game)
        {
            var turns = await _turnRepository.GetAllTurns(game);
            List<long> players = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<PlayerInGame> playersInGame = new List<PlayerInGame>();

            foreach (var playerId in players)
            {
                Player player = await _playerRepository.FindById(playerId);
                PlayerInGame playerInGame = new PlayerInGame();
                playerInGame.PlayerId = player.Id;
                playerInGame.PlayerName = player.Name;
                playerInGame.GameId = game.Id;
                playerInGame.Cards = PlayerCard(player.Id, turns);
                playerInGame.PlayerType = player.PlayerType;
                playersInGame.Add(playerInGame);
            }

            return playersInGame;
        }

        private List<Card> PlayerCard(long playerId, List<Turn> turns)
        {
            return turns.Where(p => p.PlayerId == playerId).Select(k => new Card { CardLear = k.LearCard, CardNumber = k.NumberCard }).ToList();
        }

        private async Task<int> SearchDealer(IEnumerable<PlayerInGame> players)
        {
            for (int i = 0; i < players.Count(); i++)
            {
                if ((await _playerRepository.FindById(players.ElementAtOrDefault(i).PlayerId)).PlayerType == PlayerType.Dealer)
                {
                    return i;
                }
            }

            return -1;
        }

        private async Task<int> SearchUser(IEnumerable<PlayerInGame> players)
        {
            for (int i = 0; i < players.Count(); i++)
            {
                if ((await _playerRepository.FindById(players.ElementAtOrDefault(i).PlayerId)).PlayerType == PlayerType.User)
                {
                    return i;
                }
            }

            return -1;
        }

        private async Task<List<PlayerInGame>> GenPlayers(Player player, int botsNumber, long gameId)
        {
            int rand;
            List<PlayerInGame> players = new List<PlayerInGame>();
            PlayerInGame playerTemp = new PlayerInGame();
            playerTemp.PlayerId = player.Id;
            playerTemp.GameId = gameId;
            playerTemp.PlayerName = player.Name;
            playerTemp.PlayerType = PlayerType.User;
            players.Add(playerTemp);

            List<Player> bots = await _playerRepository.GetByType(PlayerType.Bot);

            for (int i = 0; i < botsNumber; i++)
            {
                PlayerInGame playerTemp1 = new PlayerInGame();
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
            PlayerInGame playerTemp3 = new PlayerInGame();
            playerTemp3.PlayerId = dealer[rand].Id;
            playerTemp3.PlayerName = dealer[rand].Name;
            playerTemp3.PlayerType = PlayerType.Dealer;
            playerTemp3.GameId = gameId;
            players.Add(playerTemp3);

            return players;
        }

        private async Task SaveWinner(IEnumerable<PlayerInGame> gameStat)
        {
            foreach (var player in gameStat)
            {
                if (player.PlayerStatus == PlayerStatus.Won)
                {
                    await _gameResultRepository.Create(new GameResult
                    {
                        PlayerId = player.PlayerId
                    });
                }
            }
        }

        public async Task<Game> GetGame(long gameId)
        {
            var result = await _gameRepository.FindById(gameId);

            return result;
        }
    }
}
