using System.Web.Mvc;
using Logick.Interfases;
using BlackJack.DataAccess.Entities.Enums;
using Logick.Models;
using BlackJack.UI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackJack.UI.Controllers
{
    public class GameController : Controller
    {
        private IDataService _dataService;
        private IGameService _gameService;

        public GameController(IDataService dataService, IGameService gameService)
        {
            _dataService = dataService;
            _gameService = gameService;
        }

        public async Task<ActionResult> Start()
        {
            ViewBag.Player = await _dataService.GetUserOrdered();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Start(string player, int botsNumber)
        {
            await _dataService.PlayerChecked(player);

            return RedirectToAction("GameShow", new { gameId = await _gameService.StartGame(await _dataService.SearchPlayerWithName(player), botsNumber) });
        }

        public async Task<ActionResult> GameShow(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }
         
            return View(CreatPlayerViewModel(await _gameService.DoFirstTwoRound((long)gameId)));
        }

        [HttpPost]
        public async Task<ActionResult> GameShow(long? gameId, long? number)
        {
            if (IsNull(gameId) || IsNull(number))
            {
                return RedirectToAction("Start");
            }

            var v = await _gameService.ContinuePlay((long)gameId, (long)number);           
            foreach(var player in v.Players)
            {
                if(IsEndGame(player))
                {
                    await _gameService.DropCard(v);
                    return RedirectToAction("GameResult", new { gameId = v.GameId});
                }
            }

            return View(CreatPlayerViewModel(v));
        }

        public async Task<ActionResult> GameResult(long? gameId)
        {
            if (IsNull(gameId))
            {
                return RedirectToAction("Start");
            }

            return View(CreatPlayerViewModel(await _gameService.GetGameResult((long)gameId)));
        }

        private bool IsEndGame (PlayerInGame player)
        {
            return player.PlayerType == PlayerType.User && player.PlayerStatus != PlayerStatus.Play || player.PlayerType == PlayerType.Dealer && player.PlayerStatus == PlayerStatus.Lose;
        }

        private bool IsNull(long? number)
        {
            if (number == null)
            {

                return true;
            }

            return false;
        }

        private IEnumerable<PlayerViewModel> CreatPlayerViewModel(GameStat gameStat)
        {
            List<PlayerViewModel> playerViewModels = new List<PlayerViewModel>();
            foreach(var player in gameStat.Players)
            {
                PlayerViewModel playerTemp = new PlayerViewModel();
                playerTemp.Cards = player.Cards;
                playerTemp.PlayerName = player.PlayerName;
                playerTemp.PlayerStatus = player.PlayerStatus;
                playerTemp.Point = player.Point;
                playerTemp.GameId = gameStat.GameId;
                playerViewModels.Add(playerTemp);
            }

            return playerViewModels;
        }
    }
}