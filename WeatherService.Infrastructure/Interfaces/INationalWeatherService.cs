using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Interfaces
{
    public interface INationalWeatherService
    {
        Task<WeatherForecast> GetWeatherForecast(Coordinates coordinates);
    }
}
