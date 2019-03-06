using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Entities.Enum;
using DataBaseControl.Repository;
using DataBaseControl.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Logick
{
    public class DataControl
    {
        private BlackJackContext _db;
        private IPlayerRepository _player;
        private ITurnRepository _turn;
        private Random _random;
        private IGameResultRepository _winner;

        public DataControl()
        {
            _random = new Random();
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
            _turn = new TurnReposytory(_db);
            _winner = new GameResultRepository(_db);
        }

        public List<Player> GetUserOrdered()
        {
            return _player.GetAllUser().OrderByDescending(x => x.Name).ToList();
        }

        private bool RegisterNewPlayer(Player player)
        {
            if (player.Name != null)
            {
                player.PlayerType = PlayerType.User;
                _player.Create(player);

                return true;
            }

            return false;
        }

        public Player SearchPlayerWithName(string name)
        {
            return _player.SearchPlayerWithName(name);
        }

        public void PlayerChecked (string name)
        {
            if (_player.SearchPlayerWithName(name) == null)
            {
                RegisterNewPlayer(new Player { Name = name });
            }
        }

        public List<GameStats> GetGameStats (Game game)
        {
            var turns = _turn.GetAllTurns(game);
            List<long> players = turns.Select(p => p.PlayerId).Distinct().ToList();
            List<GameStats> gameStats = new List<GameStats>();

            foreach (var playerId in players)
            {
                Player player = _player.FindById(playerId);
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
                if(_player.FindById(gameStats[i].PlayerId).PlayerType == PlayerType.Dealer)
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
                if (_player.FindById(gameStats[i].PlayerId).PlayerType == PlayerType.User)
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
                PlayerName = _player.FindById(player).Name
            });

            List<Player> bots = _player.GetAllBots();

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

            List<Player> dealer = _player.GetAllDealer();

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
                    _winner.Create(new GameWinner
                    {
                        PlayerId = player.PlayerId,
                        GameId = player.GameId
                    });
                }
            }
        }
    }
}
