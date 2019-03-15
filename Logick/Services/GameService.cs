using BlackJack.DataBaseAccess.Entities;
using BlackJack.DataBaseAccess.Entities.Enum;
using BlackJack.DataBaseAccess.Repository.Interface;
using Logick.Interfases;
using Logick.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic
{
    public class GameService : IGameService
    {
        private Random _random;
        private IPlayerRepository _playerRepository;
        private IGameRepository _gameRepository;
        private ITurnRepository _turnRepository;
        private IDataService _data;
        private int _maxPoint;
        public GameService(IGameRepository gameRepository, ITurnRepository turnRepository, IPlayerRepository playerRepository, IDataService dataService)
        {
            _random = new Random();
            _playerRepository = playerRepository;
            _gameRepository = gameRepository;
            _turnRepository = turnRepository;
            _data = dataService;
            _maxPoint = 21;
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

        private Card GiveCard(Deck deck)
        {
            int rand = _random.Next(0, deck.NumberCard);
            Card card = deck.Cards[rand];
            deck.Cards.RemoveAt(rand);

            return card;
        }

        private async Task<GameStat> CreatGameStat(Game game)
        {
            GameStat gameStat = new GameStat();
            gameStat.GameId = game.Id;
            gameStat.Players = await _data.GenPlayers(game.PlayerId, game.BotsNumber);

            return gameStat;
        }

        public async Task<GameStat> DoFirstTwoRound(long gameId)
        {
            Deck deck = new Deck();
            Game game = await _gameRepository.FindById(gameId);
            GameStat gameStat = await CreatGameStat(game);

            gameStat = await DoRound(gameStat, deck);
            await CheckPoint(gameStat.Players);
            await _gameRepository.Update(game);

            return gameStat;
        }

        private async Task<GameStat> DoRound(GameStat gameStat, Deck deck)
        {
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.None);

            return gameStat;
        }

        private async Task<GameStat> DoRoundWithoutPlayerType(GameStat gameStat, Deck deck, PlayerType playerType)
        {
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerType != playerType && player.PlayerStatus == PlayerStatus.Play)
                {
                    Card card = await DoTurn(player.PlayerId, gameStat.GameId, deck);
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<GameStat> DoRoundWithoutPlayerType(GameStat gameStat, Deck deck, PlayerType playerType1, PlayerType playerType2)
        {
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerType != playerType1 && player.PlayerType != playerType2 && player.PlayerStatus == PlayerStatus.Play)
                {
                    Card card = await Task.Run(() => DoTurn(player.PlayerId, gameStat.GameId, deck));
                    player.Cards.Add(card);
                }
            }

            return gameStat;
        }

        private async Task<Card> DoTurn(long playerId, long gameId, Deck deck)
        {
            Card card = await Task.Run(() => GiveCard(deck));
            Game game = await _gameRepository.FindById(gameId);

            game.TurnNumber += 1;
            await _turnRepository.Create(
                new Turn
                {
                    GameId = game.Id,
                    PlayerId = playerId,
                    LearCard = card.LearCard,
                    NumberCard = card.NumberCard,
                });

            await _gameRepository.Update(game);

            return new Card { LearCard = card.LearCard, NumberCard = card.NumberCard };
        }

        private async Task<int> CountPoint(PlayerInGame player)
        {
            player.Point = 0;
            foreach (var card in player.Cards)
            {
                player.Point += await Task.Run(() => GetCardPoint(card));
            }

            return player.Point;
        }

        private async Task CheckPoint(List<PlayerInGame> playersInGame)
        {
            foreach (var player in playersInGame)
            {
                player.Point = await CountPoint(player);
                if (player.Point > _maxPoint)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }

            return;
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

        private async Task<GameStat> InitializationGameStat(long gameId)
        {
            Game game = await _gameRepository.FindById(gameId);
            GameStat gameStat = new GameStat();
            gameStat.Players = await _data.PlayersInGame(game);
            gameStat.GameId = gameId;

            await CheckPoint(gameStat.Players);

            return gameStat;
        }

        public async Task<GameStat> ContinuePlay(long gameId, long choose)
        {
            Game game = await _gameRepository.FindById(gameId);
            GameStat gameStat = await InitializationGameStat(gameId);
            int continueGame = 1;
            int stopGame = 2;

            if (choose == continueGame)
            {
                gameStat = await TakeCard(gameStat);
            }
            if (choose == stopGame)
            {
                gameStat.Players[await _data.SearchUser(gameStat.Players)].PlayerStatus = PlayerStatus.Wait;
                gameStat = await DropCard(gameStat);
            }

            await _gameRepository.Update(game);

            return gameStat;
        }

        private async Task<GameStat> TakeCard(GameStat gameStat)
        {
            Deck deck = _data.GetDeck(gameStat.Players);
            await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.Dealer);
            await CheckPoint(gameStat.Players);

            return gameStat;
        }

        public async Task<GameStat> DropCard(GameStat gameStat)
        {
            Deck deck = _data.GetDeck(gameStat.Players);
            int dealer = await _data.SearchDealer(gameStat.Players);

            while (await CountPoint(gameStat.Players[dealer]) <= 17)
            {
                await DoRoundWithoutPlayerType(gameStat, deck, PlayerType.Bot, PlayerType.User);
            }
            if (await CountPoint(gameStat.Players[dealer]) > _maxPoint)
            {
                gameStat.Players[dealer].PlayerStatus = PlayerStatus.Lose;
            }

            return gameStat;
        }

        public async Task<GameStat> GetGameResult(long gameId)
        {
            GameStat gameStat = await InitializationGameStat(gameId);
            Game game = (await _gameRepository.FindById(gameId));
            game.GameStatus = GameStatus.Done;
            await _gameRepository.Update(game);
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose)
                {
                    gameStat = DealerLose(gameStat);
                    return gameStat;
                }
            }

            int dealerIndex = await _data.SearchDealer(gameStat.Players);
            gameStat.Players[dealerIndex].PlayerStatus = PlayerStatus.Won;
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStat.Players[dealerIndex].Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStat.Players[dealerIndex].PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point > gameStat.Players[dealerIndex].Point)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                    gameStat.Players[dealerIndex].PlayerStatus = PlayerStatus.Lose;
                }
                if (player.PlayerStatus != PlayerStatus.Lose && player.Point <= gameStat.Players[dealerIndex].Point && player.PlayerType != PlayerType.Dealer)
                {
                    player.PlayerStatus = PlayerStatus.Lose;
                }
            }
            _data.SaveWinner(gameStat);

            return gameStat;
        }

        private GameStat DealerLose(GameStat gameStat)
        {
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerStatus != PlayerStatus.Lose)
                {
                    player.PlayerStatus = PlayerStatus.Won;
                }
            }

            return gameStat;
        }
    }
}
