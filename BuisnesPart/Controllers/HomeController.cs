using DataBaseControl;
using DataBaseControl.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuisnesPart.Controllers
{
    public class HomeController : Controller
    {
        private BlackJackContext _db = new BlackJackContext();

        public ActionResult Index()
        {
            GameRepository _game = new GameRepository(_db);
            ViewBag.Game = _game.GetAllGameWithPlayer(new DataBaseControl.Entities.Player { Id = 2 });

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
             
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}