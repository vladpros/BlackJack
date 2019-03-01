using System.Web.Mvc;
using Logick;

namespace UI.Controllers
{
    public class LeaderBoardController : Controller
    {

        private DataControl _dataControl;

        public LeaderBoardController()
        {
            _dataControl = new DataControl();
        }

        public ActionResult Index()
        {
            ViewBag.Player = _dataControl.GetUserOrdered();
            return View();
        }
    }
}