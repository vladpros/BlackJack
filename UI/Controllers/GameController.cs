using BlackJack.DataBaseAccess.Entities;
using BlackJack.BusinessLogic;
using System.Web.Mvc;
using Logick.Interfases;

namespace BlackJack.UI.Controllers
{
    public class GameController : Controller
    {
        private IDataService _dataControl;
        private IGameService _gameControl;

        public GameController(IDataService dataService, IGameService gameService)
        {
            _dataControl = dataService;
            _gameControl = gameService;
        }

        public ActionResult Start()
        {
            ViewBag.Player = _dataControl.GetUserOrdered();

            return View();
        }

        [HttpPost]
        public ActionResult Start(string player, int botsNumber)
        {
            _dataControl.PlayerChecked(player);

            return RedirectToAction("GameShow", new { gameId = _gameControl.StartGame(_dataControl.SearchPlayerWithName(player), botsNumber) });
        }

        public ActionResult GameShow(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }

            ViewBag.Game = _gameControl.DoFirstTwoRound((long)gameId);

            return View();
        }

        [HttpPost]
        public ActionResult GameShow(long? gameId, long? number)
        {
            if (IsNull(gameId) || IsNull(gameId))
            {
                return RedirectToAction("Start");
            }

            var v = _gameControl.ContinuePlay((long)gameId, (long)number);
            ViewBag.Game = v;
            foreach(var player in v)
            {
                if(IsEndGame(player))
                {
                    _gameControl.DropCard(v);
                    return RedirectToAction("GameResult", new { gameId = player.GameId});
                }
            }
            return View();
        }

        public ActionResult GameResult(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }

            ViewBag.Game = _gameControl.GetGameResult((long)gameId);

            return View();
        }

        private bool IsEndGame (GameStats player)
        {
            return player.PlayerType == BlackJack.DataBaseAccess.Entities.Enum.PlayerType.User && player.PlayerStatus != BlackJack.DataBaseAccess.Entities.Enum.PlayerStatus.Play || player.PlayerType == BlackJack.DataBaseAccess.Entities.Enum.PlayerType.Dealer && player.PlayerStatus == BlackJack.DataBaseAccess.Entities.Enum.PlayerStatus.Lose;
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