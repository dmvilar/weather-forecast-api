using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService.API.Mappers;
using WeatherService.Domain.Entities;

namespace WeatherService.Tests.Unit
{
    public class WeatherForecastMapperTests
    {
        [Fact]
        public void ToWeatherForecastViewModel_MapsCorrectly()
        {
            // Arrange
            var weatherForecast = new WeatherForecast
            {
                MaxTemperatures = new List<Temperature>
                {
                new Temperature { ValidTime = "2024-01-22T05:00:00+00:00/PT9H", Value = 20.0 },
                },
                MinTemperatures = new List<Temperature>
                {
                new Temperature { ValidTime = "2024-01-22T05:00:00+00:00/PT9H", Value = 10.0 },
                }
            };

            // Act
            var result = WeatherForecastMapper.ToWeatherForecastViewModel(weatherForecast);

            // Assert
            Assert.NotNull(result);

            var firstDay = result.First();
            Assert.Equal("2024-01-22", firstDay.Date);
            Assert.Equal("20", firstDay.MaxTemperature);
            Assert.Equal("10", firstDay.MinTemperature);
        }
    }
}
