using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GithubActions.Contract.v1;
using GithubActions.Web.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GithubActions.Web.Server.v1.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        private static readonly string[] Cities = new[]
        {
            "london", "paris"
        };

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the weather for a city (Only London & Paris are supported)
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(WeatherForecastResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new ErrorResponse
                {
                    Message = $"City '{city}' is null/whitespace"
                });
            }
            if (!Cities.Contains(city.ToLower()))
            {
                return NotFound(new ErrorResponse
                {
                    Message = $"City '{city}' not found"
                });
            }

            var rng = new Random();
            var retList = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(new WeatherForecastResponse
            {
                City = city,
                WeatherForecasts = retList
            });
        }
    }
}