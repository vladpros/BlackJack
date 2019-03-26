using System.Web.Mvc;
using BlackJack.BusinessLogic.Service.Interface;
using System.Threading.Tasks;
using BlackJack.BusinessLogic.ViewModel;
using BlackJack.DataAccess.Entities.Enums;
using BlackJack.BusinessLogic.ViewModel.Enum;
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
                GetNameGameView names = await _gameService.GetOrderedUsers();
                return View(names);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Start(string player, int botsNumber)
        {
            
            try
            {
                await _gameService.CheсkAndRegisterPlayer(player);
                long gameId = await _gameService.StartGame(player, botsNumber);
                return RedirectToAction("GameShow", new { gameId });
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        public async Task<ActionResult> GameShow(long gameId)
        {
            
            try
            {
                ShowGameView gameStatistics = new ShowGameView();
                gameStatistics.ShowGameViewItems = await _gameService.DoFirstTwoRounds(gameId);
                return View(gameStatistics.ShowGameViewItems);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Game(long gameId, PlayerChoose number)
        {
           
            try
            {
                ShowGameView gameStatistics = new ShowGameView();
                gameStatistics.ShowGameViewItems = await _gameService.ContinuePlaying((long)gameId, number);

                foreach (var player in gameStatistics.ShowGameViewItems)
                {
                    if (player.PlayerStatus == PlayerStatus.Won)
                    {
                        return View("~/Views/Game/GameResult.cshtml", gameStatistics.ShowGameViewItems);
                    }
                }
                return View(gameStatistics.ShowGameViewItems);
            }
            catch (Exception exeption)
            {
                return View("~/Views/Shared/Error.cshtml", exeption);
            }


        }

    }
}