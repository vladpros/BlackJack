using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Repository;
using DataBaseControl.Repository.Interface;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class GameController : Controller
    {
        private BlackJackContext _db;
        private IPlayerRepository _player;

        public GameController()
        {
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
        }

        public ActionResult Start()
        {
            ViewBag.Player = _player.GetAllUser().OrderByDescending(x => x.Name);
            return View();
        }

        [HttpPost]
        public ActionResult Start(string player, int botsNumber)
        {
            if(!_player.IsAPlayer(new Player { Name = player }))
            {
                _player.RegisterNewPlayer(new Player { Name = player });
            }

            return Redirect($"~/Home/Index");
        }
    }
}