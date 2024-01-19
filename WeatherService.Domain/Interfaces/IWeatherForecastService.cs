using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.Domain.Entities;

namespace WeatherService.Domain.Interfaces
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast> GetByAddress(Address address);
    }
}
