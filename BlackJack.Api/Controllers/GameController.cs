using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;
using System.Web.Http;
using System;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.BusinessLogic.ViewModel.Enum;

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
            
            try
            {
                GetNameGameView names = await _gameService.GetOrderedUsers();
                return Ok(names);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            
        }

        [HttpGet]
        public async Task<IHttpActionResult> StartGame(string name, int botsNumber)
        {
            
            try
            {
                await _gameService.CheсkAndRegisterPlayer(name);
                long gameId = await _gameService.StartGame(name, botsNumber);
                return Ok(gameId);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
            
        }

        [HttpGet]
        public async Task<IHttpActionResult> Show(long gameId, PlayerChoose? playerChoose)
        {
            try
            {
                ShowGameView gameStatistics = new ShowGameView();
                
                if (await _gameService.IsDoneGame(gameId))
                {
                    return BadRequest("Game is done");
                }
                if (playerChoose == null && await _gameService.IsNewGame(gameId))
                {
                    gameStatistics.ShowGameViewItems = await _gameService.DoFirstTwoRounds(gameId);
                    return Ok(gameStatistics);
                }
                if (playerChoose == null && !(await _gameService.IsNewGame(gameId)))
                {
                    gameStatistics.ShowGameViewItems = await _gameService.LoadGame(gameId);
                    return Ok(gameStatistics);
                }
                
                gameStatistics.ShowGameViewItems = await _gameService.ContinuePlaying(gameId, (PlayerChoose)playerChoose);
                return Ok(gameStatistics);
            }
            catch (Exception exeption)
            {
                return InternalServerError(exeption);
            }           
        }
    }
}
