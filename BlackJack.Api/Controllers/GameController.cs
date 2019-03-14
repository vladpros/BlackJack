using Logick.Interfases;
using Logick.Utils;
using Ninject;
using Ninject.Modules;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlackJack.Api.Controllers
{
    public class GameController : ApiController
    {
        private IDataService _dataService;
        private IGameService _gameService;

        public GameController()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            _dataService = kernel.Get<IDataService>();
            _gameService = kernel.Get<IGameService>();
        }

        [HttpGet]
        public async Task<List<string>> GetName()
        {
            return await _dataService.GetUserOrdered(); ;
        }

        [HttpGet]
        public async Task<long> StartGame(string name, int botsNumber)
        {
            await _dataService.PlayerChecked(name);

            return await _gameService.StartGame(await _dataService.SearchPlayerWithName(name), botsNumber);
        }
    }
}
