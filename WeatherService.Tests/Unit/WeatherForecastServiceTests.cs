using Moq;
using WeatherService.Application.Services;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Interfaces;

namespace WeatherService.Tests.Unit
{
    public class WeatherForecastServiceTests
    {
        private readonly Mock<IGeocodingService> _mockGeocodingService;
        private readonly Mock<INationalWeatherService> _mockNationalWeatherService;
        private readonly WeatherForecastService _weatherForecastService;

        public WeatherForecastServiceTests()
        {
            _mockGeocodingService = new Mock<IGeocodingService>();
            _mockNationalWeatherService = new Mock<INationalWeatherService>();
            _weatherForecastService = new WeatherForecastService(_mockGeocodingService.Object, _mockNationalWeatherService.Object);
        }

        [Fact]
        public async Task GetForecastByAddress_ValidAddress_ReturnsForecast()
        {
            // Arrange
            var validAddress = new Address() { StreetAddress = "Valid Address" };

            var coordinates = new Coordinates { Latitude = "40.7128", Longitude = "-74.0060" };
            var forecast = new WeatherForecast { };

            _mockGeocodingService.Setup(s => s.GetCoordinates(validAddress))
                .ReturnsAsync(coordinates);

            _mockNationalWeatherService.Setup(s => s.GetWeatherForecast(coordinates))
                .ReturnsAsync(forecast);

            // Act
            var result = await _weatherForecastService.GetByAddress(validAddress);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetForecastByAddress_InvalidAddress_ThrowsException()
        {
            // Arrange
            var invalidAddress = new Address() { StreetAddress = "Invalid Address" };

            _mockGeocodingService.Setup(s => s.GetCoordinates(invalidAddress))
                .ThrowsAsync(new Exception("Invalid address"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _weatherForecastService.GetByAddress(invalidAddress));
        }
    }
}