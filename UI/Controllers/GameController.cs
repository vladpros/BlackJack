using System.Web.Mvc;
using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogick.ViewModel.Enum;
using System;
using System.Collections.Generic;

namespace BlackJack.UI.Controllers
{
    public class GameController : Controller
    {
        private IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<ActionResult> Start()
        {
            try
            {
                ViewBag.Player = await _gameService.GetOrderedUsers();
                return View();
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Start(string player, int botsNumber)
        {
            long gameId;
            try
            {
                await _gameService.CheсkAndRegisterPlayer(player);
                gameId = await _gameService.StartGame(player, botsNumber);
                return RedirectToAction("GameShow", new { gameId });
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        public async Task<ActionResult> GameShow(long? gameId)
        {
            IEnumerable<PlayerInGameView> gameStatistics;
            try
            {
                if (gameId == null)
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
                gameStatistics = await _gameService.DoFirstTwoRounds((long)gameId);
                return View(gameStatistics);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        [HttpPost]
        public async Task<ActionResult> GameShow(long? gameId, long? number)
        {
            IEnumerable<PlayerInGameView> gameStatistics;
            try
            {
                if (gameId == null || number == null)
                {
                    return View("~/Views/Shared/Error.cshtml");
                }

                gameStatistics = await _gameService.ContinuePlaying((long)gameId, (PlayerChoos)number);

                foreach (var player in gameStatistics)
                {
                    if (player.PlayerStatus == PlayerStatus.Won)
                    {
                        return RedirectToAction("GameResult", new { gameId });
                    }
                }
                return View(gameStatistics);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }


        }

        [HttpGet]
        public async Task<ActionResult> GameResult(long? gameId)
        {
            IEnumerable<PlayerInGameView> gameStatistics;
            try
            {
                if (gameId == null)
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
                gameStatistics = await _gameService.LoadGame((long)gameId);
                return View(gameStatistics);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

    }
}