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

        public GameController()
        {
            NinjectModule registrations = new NinjectRegistration();
            var kernel = new StandardKernel(registrations);
            _dataService = kernel.Get<IDataService>();
        }

        public async Task<List<string>> GetName()
        {
            var l = await _dataService.GetUserOrdered();

            return l;
        }
    }
}
