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

namespace BlackJack.Api.Controllers
{
    public class GameController : ApiController
    {
        private IGameService _gameService;

        public GameController()
        {
            INinjectModule[] registrations = { new NinjectRegistrationService(), new NinjectRegistrationRepository() };
            var kernel = new StandardKernel(registrations);

            _gameService = kernel.Get<IGameService>();
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
            if(gameId == null)
            {
                return new List<PlayerInGameViewModel>();
            }
            if ((await _gameService.GetGame((long)gameId)).GameStatus == GameStatus.Done)
            {
                return new List<PlayerInGameViewModel>();
            }
            if (choos == null)
            {
               return await _gameService.DoFirstTwoRound((long)gameId);
            }

            var gameResult = await _gameService.ContinuePlay((long)gameId, (long)choos);

            return gameResult;
        }
    }
}
