using System.Web.Mvc;
using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;

namespace BlackJack.UI.Controllers
{
    public class GameController : Controller
    {
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<ActionResult> Start()
        {
            ViewBag.Player = await _gameService.GetUserOrdered();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Start(string player, int botsNumber)
        {
            await _gameService.ChekPlayer(player);

            return RedirectToAction("GameShow", new { gameId = await _gameService.StartGame(player, botsNumber) });
        }

        public async Task<ActionResult> GameShow(long? gameId)
        {
            if (gameId == null)
            {
                return RedirectToAction("Start");
            }
         
            return View(await _gameService.DoFirstTwoRound((long)gameId));
        }

        [HttpPost]
        public async Task<ActionResult> GameShow(long? gameId, long? number)
        {
            if (gameId == null || number == null)
            {
                return RedirectToAction("Start");
            }

            var v = await _gameService.ContinuePlay((long)gameId, (long)number);           

            return View(await _gameService.ContinuePlay((long)gameId, (long)number));
        }

        public async Task<ActionResult> GameResult(long? gameId)
        {
            if (gameId == null)
            {
                return RedirectToAction("Start");
            }


            return View(await _gameService.GetGameResult((long)gameId));
        }


    }
}