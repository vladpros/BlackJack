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
            
            return RedirectToAction("GameShow", _gameControl.StartGame(_dataControl.SearchPlayerWithName(player), botsNumber));
        }

        public ActionResult GameShow(Game game)
        {

            return View();
        }
    }
}