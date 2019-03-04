using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Entities.Enam;
using DataBaseControl.Repository;
using DataBaseControl.Repository.Interface;
using System.Collections.Generic;
using System.Linq;


namespace Logick
{
    public class DataControl
    {
        private BlackJackContext _db;
        private IPlayerRepository _player;
        private ITurnRepository _turn;

        public DataControl()
        {
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
            _turn = new TurnReposytory(_db);
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

        public List<GameStats> GetAllTurns (Game game)
        {
            var l = _turn.GetAllTurns(game);
            List<GameStats> k = new List<GameStats>();

            foreach(var p in game.Players)
            {
                k.Add(new GameStats { PlayerId = p.Id, PlayerName = _player.FindById(p.Id).Name, Cards = PlayerCard(p.Id, l)});
            }

            return k;
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
                    deck.Cards.Remove(card);
                }
            }

            return deck;
        }
    }
}
