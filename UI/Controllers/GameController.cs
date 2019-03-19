using System.Web.Mvc;
using BlackJack.BusinessLogic.Service.Interface;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic.ViewModel;
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

            return RedirectToAction("GameShow", new { gameId = await _gameService.StartGame(await _gameService.SearchPlayerWithName(player), botsNumber) });
        }

        public async Task<ActionResult> GameShow(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }
         
            return View(await _gameService.DoFirstTwoRound((long)gameId));
        }

        [HttpPost]
        public async Task<ActionResult> GameShow(long? gameId, long? number)
        {
            if (IsNull(gameId) || IsNull(number))
            {
                return RedirectToAction("Start");
            }

            var v = await _gameService.ContinuePlay((long)gameId, (long)number);           

            return View(v);
        }

        public async Task<ActionResult> GameResult(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }

            return View(await _gameService.GetGameResult((long)gameId));
        }

        private bool IsNull(long? number)
        {
            if (number == null)
            {

                return true;
            }

            return false;
        }
    }
}