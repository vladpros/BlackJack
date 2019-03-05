using DataBaseControl.Entities;
using Logick;
using System.Web.Mvc;

namespace UI.Controllers
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

        public ActionResult GameShow(long gameId)
        {      

            ViewBag.Game = _gameControl.DoFirstTwoRound(gameId);

            return View();
        }

        [HttpPost]
        public ActionResult GameShow(long gameId, int number)
        {
            var v = _gameControl.ContinuePlay(gameId, number);
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

        public ActionResult GameResult(long gameId)
        {

            ViewBag.Game = _gameControl.GetGameResult(gameId);

            return View();
        }

        private bool IsEndGame (GameStats player)
        {
            return player.PlayerType == DataBaseControl.Entities.Enam.PlayerType.User && player.PlayerStatus != DataBaseControl.Entities.Enam.PlayerStatus.Play || player.PlayerType == DataBaseControl.Entities.Enam.PlayerType.Dealer && player.PlayerStatus == DataBaseControl.Entities.Enam.PlayerStatus.Lose;
        }
    }
}