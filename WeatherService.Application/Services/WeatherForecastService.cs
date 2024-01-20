using WeatherService.Domain.Entities;
using WeatherService.Domain.Interfaces;
using WeatherService.Infrastructure.Interfaces;

namespace WeatherService.Application.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IGeocodingService _geocodingService;
        private readonly INationalWeatherService _nationalWeatherService;
        public WeatherForecastService(IGeocodingService geocodingService, INationalWeatherService nationalWeatherService)
        {
            _geocodingService = geocodingService;
            _nationalWeatherService = nationalWeatherService;
        }

        public async Task<WeatherForecast> GetByAddress(Address address)
        {
            Coordinates coordinates = await _geocodingService.GetCoordinates(address);

            return await _nationalWeatherService.GetWeatherForecast(coordinates);
        }
    }
}
