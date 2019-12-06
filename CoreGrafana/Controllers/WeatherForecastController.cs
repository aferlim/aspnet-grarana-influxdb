using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreGrafana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("hotfound")]
        public async Task<IActionResult> NotFoundAC([FromQuery]int v)
        {
            await Task.Delay(TimeSpan.FromSeconds(2 * v), HttpContext.RequestAborted);

            switch (v)
            {
                case 1:
                    return NotFound();
                case 2:
                    return BadRequest();
                case 3:
                    return Unauthorized();
                case 4:
                    return StatusCode(500);
                default:
                    return Ok();
            }
        }

        [HttpGet("badfound")]
        public IActionResult BadAC()
        {

            return BadRequest();
        }

    }
}
