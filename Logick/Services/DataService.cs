using BlackJack.DataAccess.Entities;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.DataAccess.Repository.Interface;
using Logick.Interfases;
using Logick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJack.BusinessLogic
{
    public class DataService : IDataService
    {
        private IPlayerRepository _playerRepository;
        private ITurnRepository _turnRepository;
        private Random _random;
        private IGameResultRepository _gameResultRepository;
        private IGameRepository _gameRepository;
        public DataService(IGameResultRepository gameResultRepository, ITurnRepository turnRepository, IPlayerRepository playerRepository, IGameRepository gameRepository)
        {
            _random = new Random();
            _playerRepository = playerRepository;
            _turnRepository = turnRepository;
            _gameResultRepository = gameResultRepository;
            _gameRepository = gameRepository;
        }


        public async Task<List<string>> GetUserOrdered()
        {
            return (await _playerRepository.GetAllUser()).OrderByDescending(x => x.Name).Select(s => s.Name ).ToList();
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
            if ((await _playerRepository.SearchPlayerWithName(name)) == null)
            {
                RegisterNewPlayer(new Player { Name = name });
            }
        }

        public async Task<List<PlayerInGame>> PlayersInGame(Game game)
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

        public Deck GetDeck(List<PlayerInGame> Players)
        {
            Deck deck = new Deck();

            foreach (var player in Players)
            {
                foreach (var card in player.Cards)
                {
                    int index = SearchCardInDeck(deck, card);
                    deck.Cards.RemoveAt(index);
                }
            }

            return deck;
        }

        public async Task<int> SearchDealer(List<PlayerInGame> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if ((await _playerRepository.FindById(players[i].PlayerId)).PlayerType == PlayerType.Dealer)
                {
                    return i;
                }
            }

            return -1;
        }

        public async Task<int> SearchUser(List<PlayerInGame> players)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if ((await _playerRepository.FindById(players[i].PlayerId)).PlayerType == PlayerType.User)
                {
                    return i;
                }
            }

            return -1;
        }

        public async Task<List<PlayerInGame>> GenPlayers(long player, int botsNumber)
        {
            int rand;
            List<PlayerInGame> players = new List<PlayerInGame>();
            PlayerInGame playerTemp = new PlayerInGame();
            playerTemp.PlayerId = player;
            playerTemp.PlayerName = (await _playerRepository.FindById(player)).Name;
            playerTemp.PlayerType = PlayerType.User;
            players.Add(playerTemp);

            List<Player> bots = await _playerRepository.GetAllBots();

            for (int i = 0; i < botsNumber; i++)
            {
                PlayerInGame playerTemp1 = new PlayerInGame();
                rand = _random.Next(0, bots.Count);
                playerTemp1.PlayerId = bots[rand].Id;
                playerTemp1.PlayerName = bots[rand].Name;
                playerTemp1.PlayerType = PlayerType.Bot;
                players.Add(playerTemp1);
                bots.RemoveAt(rand);
            }

            List<Player> dealer = await _playerRepository.GetAllDealer();

            rand = _random.Next(0, dealer.Count);
            PlayerInGame playerTemp3 = new PlayerInGame();
            playerTemp3.PlayerId = dealer[rand].Id;
            playerTemp3.PlayerName = dealer[rand].Name;
            playerTemp3.PlayerType = PlayerType.Dealer;
            players.Add(playerTemp3);

            return players;
        }

        private int SearchCardInDeck(Deck deck, Card card)
        {
            for (int i = 0; i < deck.NumberCard; i++)
            {
                if (deck.Cards[i].CardLear == card.CardLear && deck.Cards[i].CardNumber == card.CardNumber)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SaveWinner(GameStat gameStat)
        {
            foreach (var player in gameStat.Players)
            {
                if (player.PlayerStatus == PlayerStatus.Won)
                {
                    _gameResultRepository.Create(new GameResult
                    {
                        PlayerId = player.PlayerId
                    });
                }
            }
        }

        public async Task<Game> GetGame(long id)
        {
            return await _gameRepository.FindById(id);
        }
    }
}

