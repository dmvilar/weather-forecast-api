using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WeatherService.API.Mappers;
using WeatherService.Domain.Entities;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastService _weatherForecastService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService weatherForecastService)
        {
            _logger = logger;
            _weatherForecastService = weatherForecastService;
        }

        [HttpGet("{address}")]
        public async Task<IActionResult> Get([Required] string address)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var weatherForecast = await _weatherForecastService.GetByAddress(new Address { StreetAddress = address });

                var weatherForecastViewModel = WeatherForecastMapper.ToWeatherForecastViewModel(weatherForecast);

                return Ok(weatherForecastViewModel);
            }
            catch (Exception ex)
            {
                return NotFound(new { ErrorMessage = ex.Message });
            }
        }
    }
}
