using BlackJackDataBaseAccess;
using BlackJackDataBaseAccess.Entities;
using BlackJackDataBaseAccess.Entities.Enum;
using BlackJackDataBaseAccess.Repository;
using BlackJackDataBaseAccess.Repository.Interface;
using DataBaseControl.Repository.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BlackJack.BusinessLogic
{
    public class DataControl
    {
        private BlackJackContext _db;
        private IPlayerRepository _playerRepository;
        private ITurnRepository _turnRepository;
        private Random _random;
        private IGameResultRepository _winnerRepository;

        public DataControl()
        {
            _random = new Random();
            _db = new BlackJackContext();
            _playerRepository = new DapPlayerRepository();
            _turnRepository = new DapTurnRepository();
            _winnerRepository = new DapGameResultRepository();
        }

        public List<Player> GetUserOrdered()
        {
            return _playerRepository.GetAllUser().OrderByDescending(x => x.Name).ToList();
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

        public Player SearchPlayerWithName(string name)
        {
            return _playerRepository.SearchPlayerWithName(name);
        }

        public void PlayerChecked (string name)
        {
            if (_playerRepository.SearchPlayerWithName(name) == null)
            {
                RegisterNewPlayer(new Player { Name = name });
            }
        }

        public List<GameStats> GetGameStats (Game game)
        {
            var turns = _turnRepository.GetAllTurns(game);
            List<long> players = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<GameStats> gameStats = new List<GameStats>();

            foreach (var playerId in players)
            {
                Player player = _playerRepository.FindById(playerId);
                gameStats.Add(new GameStats
                {
                    PlayerId = player.Id,
                    PlayerName = player.Name,
                    GameId = game.Id,
                    Cards = PlayerCard(player.Id, turns),
                    PlayerType = player.PlayerType
                });
            }
           
            return gameStats;
        }

        private List<Card> PlayerCard (long playerId, List<Turn> turns)
        {
            return turns.Where(p => p.PlayerId == playerId).Select(k => new Card { LearCard = k.LearCard, NumberCard = k.NumberCard }).ToList();
        }

        public Deck GetDeck (List<GameStats> gameStats)
        {
            Deck deck = new Deck();

            foreach(var player in gameStats)
            {
                foreach(var card in player.Cards)
                {
                    int index = SearchCardInDeck(deck, card);
                    deck.Cards.RemoveAt(index);
                }
            }

            return deck;
        }

        public int SearchDealer (List<GameStats> gameStats)
        {
            for(int i = 0; i<gameStats.Count; i++)
            {
                if(_playerRepository.FindById(gameStats[i].PlayerId).PlayerType == PlayerType.Dealer)
                {
                    return i;
                }
            }

            return -1;
        }

        public int SearchUser(List<GameStats> gameStats)
        {
            for (int i = 0; i < gameStats.Count; i++)
            {
                if (_playerRepository.FindById(gameStats[i].PlayerId).PlayerType == PlayerType.User)
                {
                    return i;
                }
            }

            return -1;
        }

        public List<GameStats> GenPlayers(long player, int botsNumber)
        {
            int rand;

            List<GameStats> players = new List<GameStats>();
            players.Add(new GameStats
            {
                PlayerId = player,
                PlayerName = _playerRepository.FindById(player).Name
            });

            List<Player> bots = _playerRepository.GetAllBots();

            for (int i = 0; i < botsNumber; i++)
            {
                rand = _random.Next(0, bots.Count);
                players.Add(new GameStats
                {
                    PlayerId = bots[rand].Id,
                    PlayerName = bots[rand].Name
                });
                bots.RemoveAt(rand);
            }

            List<Player> dealer = _playerRepository.GetAllDealer();

            rand = _random.Next(0, dealer.Count);
            players.Add(new GameStats
            {
                PlayerId = dealer[rand].Id,
                PlayerName = dealer[rand].Name,
            });

            return players;
        }

        private int SearchCardInDeck(Deck deck, Card card)
        {
            for(int i = 0; i<deck.NumberCard; i++)
            {
                if(deck.Cards[i].LearCard == card.LearCard && deck.Cards[i].NumberCard == card.NumberCard)
                {
                    return i;
                }
            }

            return -1;
        }

        public void SaveWinner(List<GameStats> gameStats)
        {
            foreach (var player in gameStats)
            {
                if (player.PlayerStatus == PlayerStatus.Won)
                {
                    _winnerRepository.Create(new GameResult
                    {
                        PlayerId = player.PlayerId,
                        GameId = player.GameId
                    });
                }
            }
        }
    }
}
