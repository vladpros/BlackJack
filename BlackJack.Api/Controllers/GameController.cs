using BlackJack.Api.Models;
using Logick.Interfases;
using Logick.Models;
using Logick.Utils;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.Api.Controllers
{
    public class GameController : ApiController
    {
        private IDataService _dataService;
        private IGameService _gameService;

        public GameController()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            _dataService = kernel.Get<IDataService>();
            _gameService = kernel.Get<IGameService>();
        }

        [HttpGet]
        public async Task<List<string>> GetName()
        {
            return await _dataService.GetUserOrdered(); ;
        }

        [HttpGet]
        public async Task<long> StartGame(string name, int botsNumber)
        {
            await _dataService.PlayerChecked(name);

            return await _gameService.StartGame(await _dataService.SearchPlayerWithName(name), botsNumber);
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerViewModel>> ShowGame(long? gameId, long? choos)
        {
            if(gameId == null)
            {
                return new List<PlayerViewModel>();
            }
            if (choos == null)
            {
               return CreatPlayerViewModel(await _gameService.DoFirstTwoRound((long)gameId));
            }

            return CreatPlayerViewModel(await _gameService.ContinuePlay((long)gameId, (long)choos));
        }

        private IEnumerable<PlayerViewModel> CreatPlayerViewModel(GameStat gameStat)
        {
            List<PlayerViewModel> playerViewModels = new List<PlayerViewModel>();
            foreach (var player in gameStat.Players)
            {
                PlayerViewModel playerTemp = new PlayerViewModel();
                playerTemp.Cards = player.Cards;
                playerTemp.PlayerName = player.PlayerName;
                playerTemp.PlayerStatus = player.PlayerStatus;
                playerTemp.Point = player.Point;
                playerTemp.GameId = gameStat.GameId;
                playerViewModels.Add(playerTemp);
            }

            return playerViewModels;
        }
    }
}
