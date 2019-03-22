using System.Web.Mvc;
using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities.Enums;

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
            await _gameService.CheсkPlayer(player);

            return RedirectToAction("GameShow", new { gameId = await _gameService.StartGame(player, botsNumber) });
        }

        public async Task<ActionResult> GameShow(long? gameId)
        {
            if (gameId == null)
            {
                return RedirectToAction("Start");
            }
         
            return View(await _gameService.DoFirstTwoRounds((long)gameId));
        }

        [HttpPost]
        public async Task<ActionResult> GameShow(long? gameId, long? number)
        {
            if (gameId == null || number == null)
            {
                return RedirectToAction("Start");
            }

            var gameStat = await _gameService.ContinuePlaying((long)gameId, (long)number);
            foreach (var player in gameStat)
            {
                if (IsEndGame(player))
                {
                    return RedirectToAction("GameResult", new { gameId });
                }
            }

            return View(gameStat);
        }

        [HttpGet]
        public async Task<ActionResult> GameResult(long? gameId)
        {
            if (gameId == null)
            {
                return RedirectToAction("Start");
            }


            return View(await _gameService.ContinuePlaying((long)gameId, 2));
        }

        private bool IsEndGame(PlayerInGameView player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play || player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }

    }
}