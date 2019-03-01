using DataBaseControl;
using DataBaseControl.Repository;
using System.Web.Mvc;
using System.Linq;
using DataBaseControl.Repository.Interface;

namespace UI.Controllers
{
    public class LeaderBoardController : Controller
    {

        private BlackJackContext _db;
        private IPlayerRepository _player;

        public LeaderBoardController()
        {
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
        }

        public ActionResult Index()
        {
            ViewBag.Player = _player.GetAllUser().OrderByDescending(x => x.Point);
            return View();
        }
    }
}