using DataBaseControl.Entities;
using Logick;
using System.Web.Mvc;
using UI.Models;

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
            TempData["game"] = _gameControl.StartGame(_dataControl.SearchPlayerWithName(player), botsNumber);

            return RedirectToAction("GameShow");
        }

        public ActionResult GameShow(GameViewModel gameModel)
        {
            Game game = (Game)TempData["game"];  
            ViewBag.Turns = _gameControl.DoFirstRound(game);

            return View();
        }
    }
}