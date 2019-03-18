using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic..Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.BusinessLogic.Utils;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.Api.Controllers
{
    public class GameController : ApiController
    {
        private IGameService _gameService;

        public GameController()
        {
            NinjectModule registrations = new NinjectRegistration();
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
            await _gameService.PlayerChecked(name);

            return await _gameService.StartGame(await _gameService.SearchPlayerWithName(name), botsNumber);
        }

        [HttpGet]
        public async Task<IEnumerable<PlayerInGame>> ShowGame(long? gameId, long? choos)
        {
            if(gameId == null)
            {
                return new List<PlayerInGame>();
            }
            if ((await _gameService.GetGame((long)gameId)).GameStatus == GameStatus.Done)
            {
                return new List<PlayerInGame>();
            }
            if (choos == null)
            {
               return await _gameService.DoFirstTwoRound((long)gameId);
            }


            var gameResult = await _gameService.ContinuePlay((long)gameId, (long)choos);

            foreach (var player in gameResult)
            {
                if (IsEndGame(player))
                {
                    await _gameService.DropCard(gameResult);

                    return await _gameService.GetGameResult((long)gameId);
                }
            }

            return gameResult;
        }

        private bool IsEndGame(PlayerInGame player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play || player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }
    }
}
