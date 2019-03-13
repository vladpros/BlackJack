using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackJack.BusinessLogic;
using Logick.Interfases;
using Microsoft.AspNetCore.Mvc;

namespace Angular.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SampleDataController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private IDataService _dataService;

        //public SampleDataController(IDataService dataService)
        //{
        //    _dataService = dataService;
        //}

        [HttpGet("WeatherForecasts")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        //[HttpGet("[action]")]
        //public async Task<string> GetName()
        //{
        //    string[] k = { "ergerg", "ergergerg", " ergerg" };
        //    return (await _dataService.GetUserOrdered()).ToString();
        //}
    }
}
