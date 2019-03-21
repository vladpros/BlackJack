using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.BusinessLogic.Utils;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DataBaseControl.Util;

namespace BlackJack.Api2.Controllers
{

    public class GameController : ApiController
    {
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<List<string>> GetName()
        {
            return await _gameService.GetUserOrdered(); ;
        }

        [HttpGet]
        public async Task<long> StartGame(string name, int botsNumber)
        {
            await _gameService.ChekPlayer(name);

            return await _gameService.StartGame(name, botsNumber);
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerInGameViewModel>> ShowGame(long? gameId, long? choos)
        {
            long gameIdLong = (long)gameId;
            if(gameId == null)
            {
                return new List<PlayerInGameViewModel>();
            }
            if ((await _gameService.GetGame(gameIdLong)).GameStatus == GameStatus.Done)
            {
                return new List<PlayerInGameViewModel>();
            }
            if (choos == null && await _gameService.IsNewGame(gameIdLong))
            {
               return await _gameService.DoFirstTwoRound(gameIdLong);
            }
            if (choos == null && !(await _gameService.IsNewGame(gameIdLong)))
            {
                return await _gameService.LoadGame(gameIdLong);
            }
            var gameResult = await _gameService.ContinuePlay(gameIdLong, (long)choos);

            return gameResult;
        }
    }
}
