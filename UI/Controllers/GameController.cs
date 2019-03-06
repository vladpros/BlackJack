using BlackJackDataBaseAccess.Entities;
using BlackJack.BusinessLogic;
using System.Web.Mvc;

namespace BlackJack.UI.Controllers
{
    public class GameController : Controller
    {
        private DataControl _dataControl;
        private GameControl _gameControl;

        public GameController()
        {
            _dataControl = new DataControl();
            _gameControl = new GameControl();
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
            return player.PlayerType == BlackJackDataBaseAccess.Entities.Enum.PlayerType.User && player.PlayerStatus != BlackJackDataBaseAccess.Entities.Enum.PlayerStatus.Play || player.PlayerType == BlackJackDataBaseAccess.Entities.Enum.PlayerType.Dealer && player.PlayerStatus == BlackJackDataBaseAccess.Entities.Enum.PlayerStatus.Lose;
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