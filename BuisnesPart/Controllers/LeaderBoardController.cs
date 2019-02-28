using DataBaseControl;
using DataBaseControl.Entities;
using DataBaseControl.Repository;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace BuisnesPart.Controllers
{
    public class LeaderBoardController : Controller
    {

        private BlackJackContext _db;
        private PlayerRepository _player;

        public LeaderBoardController()
        {
            _db = new BlackJackContext();
            _player = new PlayerRepository(_db);
        }

        public ActionResult Index()
        {
            ViewBag.Player = _player.GetAllPlayer().OrderByDescending(x => x.Point); 
            return View();
        }
    }
}