using Newtonsoft.Json.Linq;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Interfaces;

namespace WeatherService.Infrastructure.Services
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<Coordinates> GetCoordinates(Address address)
        {
            try
            {
                var url = $"https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={Uri.EscapeDataString(address.StreetAddress)}&benchmark=Public_AR_Current&format=json";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                var coordinatesJson = json["result"]["addressMatches"][0]["coordinates"];

                var coordinates = new Coordinates
                {
                    Latitude = coordinatesJson["y"].ToString().Replace(",", "."),
                    Longitude = coordinatesJson["x"].ToString().Replace(',', '.')
                };

                return coordinates;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve coordinates from geocoding API", ex);
            }
        }
    }
}
