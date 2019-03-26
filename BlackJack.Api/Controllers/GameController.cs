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
                NameView names = await _gameService.GetOrderedUsers();
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
        public async Task<IHttpActionResult> ShowGame(long? gameId, PlayerChoose? choos)
        {
            try
            {
                ShowGameView gameStatistics = new ShowGameView();
                
                if (!gameId.HasValue)
                {
                    return BadRequest("Game not found");
                }
                if (await _gameService.IsDoneGame(gameId.Value))
                {
                    return BadRequest("Game is done");
                }
                if (choos == null && await _gameService.IsNewGame(gameId.Value))
                {
                    gameStatistics.ShowGameViewItems = await _gameService.DoFirstTwoRounds(gameId.Value);
                    return Ok(gameStatistics);
                }
                if (choos == null && !(await _gameService.IsNewGame(gameId.Value)))
                {
                    gameStatistics.ShowGameViewItems = await _gameService.LoadGame(gameId.Value);
                    return Ok(gameStatistics);
                }
                
                gameStatistics.ShowGameViewItems = await _gameService.ContinuePlaying(gameId.Value, (PlayerChoose)choos);
                return Ok(gameStatistics);
            }
            catch (Exception exeption)
            {
                return InternalServerError(exeption);
            }           
        }
    }
}
