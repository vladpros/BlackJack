using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using BlackJack.BusinessLogic.ViewModel;
using System.Collections.Generic;

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
            IEnumerable<string> names;
            try
            {
                names = await _gameService.GetOrderedUsers();
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            return Ok(names);
        }

        [HttpGet]
        public async Task<IHttpActionResult> StartGame(string name, int botsNumber)
        {
            long gameId;
            try
            {
                await _gameService.CheсkPlayer(name);
                gameId = await _gameService.StartGame(name, botsNumber);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            return Ok(gameId);
        }

        [HttpGet]
        public async Task<IHttpActionResult> ShowGame(long? gameId, long? choos)
        {
            IEnumerable<PlayerInGameView> gameStatistics;
            try
            {
                long gameIdLong = (long)gameId;
                if (gameId == null)
                {
                    return BadRequest("Game not found");
                }
                if ((await _gameService.GetGame(gameIdLong)).GameStatus == GameStatus.Done)
                {
                    return BadRequest("Game is done");
                }
                if (choos == null && await _gameService.IsNewGame(gameIdLong))
                {
                    return Ok(await _gameService.DoFirstTwoRounds(gameIdLong));
                }
                if (choos == null && !(await _gameService.IsNewGame(gameIdLong)))
                {
                    return Ok(await _gameService.LoadGame(gameIdLong));
                }
                gameStatistics = await _gameService.ContinuePlaying(gameIdLong, (long)choos);
            }
            catch (Exception exeption)
            {
                return InternalServerError(exeption);
            }
            return Ok(gameStatistics);
        }
    }
}
