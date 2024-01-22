using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Services;

namespace WeatherService.Tests.Unit
{
    public class NationalWeatherServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly NationalWeatherService _nationalWeatherService;
        private readonly HttpClient _httpClient;

        public NationalWeatherServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _nationalWeatherService = new NationalWeatherService(_httpClient);
        }

        [Fact]
        public async Task GetWeatherForecast_ValidCoordinates_ReturnsForecast()
        {
            // Arrange
            var mockWeatherForecast = new WeatherForecast { };
            var coordinates = new Coordinates { Latitude = "39.7456", Longitude = "-97.0892" };
            var mockFirstResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        properties = new
                        {
                            forecastGridData = "https://api.weather.gov/gridpoints/LWX/97,71"
                        }
                    })),
            };
            var mockSecondResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        properties = new
                        {
                            maxTemperature = new 
                            {
                                values = new[]
                                {
                                    new {validTime = "", value = "1.11" },
                                    new {validTime = "", value = "1.11" },
                                    new {validTime = "", value = "1.11" }
                                }
                            },
                            minTemperature = new
                            {
                                values = new[]
                                {
                                    new {validTime = "", value = "1.00" },
                                    new {validTime = "", value = "1.00" },
                                    new {validTime = "", value = "1.00" }
                                }
                            },
                        }
                    })),
            };


            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("api.weather.gov")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockFirstResponse);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("api.weather.gov/gridpoints/LWX/")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(mockSecondResponse);

            // Act
            var result = await _nationalWeatherService.GetWeatherForecast(coordinates);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetWeatherForecast_ApiThrowsException()
        {
            // Arrange
            var coordinates = new Coordinates { Latitude = "35.0", Longitude = "102.0" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("api.weather.gov")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _nationalWeatherService.GetWeatherForecast(coordinates));
        }

    }
}
