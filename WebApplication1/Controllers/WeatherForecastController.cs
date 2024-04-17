using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, Summaries.Length).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("get-sample")]
        public IActionResult GetSample(string summary)
        {
            var randomisedForecast = Enumerable.Range(1, Summaries.Length)
                .Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            .ToArray();


            var response = randomisedForecast.FirstOrDefault(x => x.Summary == summary);


            if (response is not null)
            {
                return Ok(response);
            }

            return NotFound($"Oops, sorry but no forecast with {summary} was found");

        }

        [HttpPost]
        public IActionResult CreateNewSummary(string summary)
        {
            if (Summaries is null)
            {
                return BadRequest("Something did not right");
            }

            var isInTheBox = Summaries.Select(x => x.ToLower()).Contains(summary.ToLower());

            if (isInTheBox)
            {
                return BadRequest("Sorry, but this item already exist");
            }

            var tempArray = new string[Summaries.Length + 1];

            for (int i = 0; i < Summaries.Length; i++)
            {
                tempArray[i] = Summaries[i];
            }


            tempArray[tempArray.Length - 1] = summary;



            return Ok(tempArray);

        }

        [HttpDelete]
        public IActionResult DeleteFromList(string summary)
        {
            if (Summaries is null)
            {
                return BadRequest("Something did not right");
            }

            var isInTheBox = Summaries.Select(x => x.ToLower()).Contains(summary.ToLower());

            if (!isInTheBox)
            {
                return BadRequest("Sorry, but this item does not exist");
            }

            var tempList = Summaries.ToList();

            var isActionSucceed = tempList.Remove(summary);

            if(!isActionSucceed) 
            {
                return BadRequest("Sorry, something unexpected happened");
            }


            return Ok(new
            {
                message = "Done",
                tempList
            });

        }
    }
}
