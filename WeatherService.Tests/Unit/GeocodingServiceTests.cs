using Moq;
using Moq.Protected;
using System.Net;
using Newtonsoft.Json;
using WeatherService.Infrastructure.Services;
using WeatherService.Domain.Entities;

public class GeocodingServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly GeocodingService _geocodingService;
    private readonly HttpClient _httpClient;

    public GeocodingServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        _geocodingService = new GeocodingService(_httpClient);
    }

    [Fact]
    public async Task GetCoordinates_ValidAddress_ReturnsCoordinates()
    {
        // Arrange
        var validAddress = new Address { StreetAddress = "123 Main St" };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(
                new
                {
                    result = new
                    {
                        addressMatches = new[]
                        {
                            new
                            {
                                coordinates = new
                                {
                                    x = "102.0",
                                    y = "35.0"
                                }
                            }
                        }
                    }
                })),
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("geocoding.geo.census.gov")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockResponse);

        // Act
        var result = await _geocodingService.GetCoordinates(validAddress);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("35.0", result.Latitude);
        Assert.Equal("102.0", result.Longitude);
    }

    [Fact]
    public async Task GetCoordinates_ApiThrowsException()
    {
        // Arrange
        var invalidAddress = new Address { StreetAddress = "Invalid Address" };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("geocoding.geo.census.gov")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _geocodingService.GetCoordinates(invalidAddress));
    }
}
