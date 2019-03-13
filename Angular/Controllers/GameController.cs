using Microsoft.AspNetCore.Mvc;
using Logick.Interfases;
using BlackJack.DataBaseAccess.Entities.Enum;
using Logick.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlackJack.DataBaseAccess.Entities;

namespace Angular.Controllers
{

    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private IDataService _dataService;

        public GameController(IDataService dataService)
        {
            _dataService = dataService;
        }


    }
}
