using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.BusinessLogic.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System;
using System.Web.Http.Results;

namespace BlackJack.Api.Controllers
{

    public class GameController : ApiController
    {
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
           _gameService = gameService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetName()
        {
            var names = await _gameService.GetUserOrdered();

            return Ok(names);
        }

        [HttpGet]
        public async Task<IHttpActionResult> StartGame(string name, int botsNumber)
        {
            long gameId;
            try
            {
                await _gameService.ChekPlayer(name);
                gameId = await _gameService.StartGame(name, botsNumber);
            }
            catch (Exception ex)
            {
                return new ExceptionResult(ex, this);
            }
            return Ok(gameId);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ShowGame(long? gameId, long? choos)
        {
            long gameIdLong = (long)gameId;
            if(gameId == null)
            {
                return NotFound();
            }
            if ((await _gameService.GetGame(gameIdLong)).GameStatus == GameStatus.Done)
            {
                return NotFound();
            }
            if (choos == null && await _gameService.IsNewGame(gameIdLong))
            {
               return Ok(await _gameService.DoFirstTwoRound(gameIdLong));
            }
            if (choos == null && !(await _gameService.IsNewGame(gameIdLong)))
            {
                return Ok(await _gameService.LoadGame(gameIdLong));
            }
            var gameResult = await _gameService.ContinuePlay(gameIdLong, (long)choos);

            return Ok(gameResult);
        }
    }
}
