using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeatherService.Domain.Entities;
using WeatherService.Infrastructure.Interfaces;

namespace WeatherService.Infrastructure.Services
{
    public class NationalWeatherService : INationalWeatherService
    {
        private readonly HttpClient _httpClient;

        public NationalWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyWeatherApp (myweatherapp.com, contact@myweatherapp.com)");
        }

        public async Task<WeatherForecast> GetWeatherForecast(Coordinates coordinates)
        {
            try
            {
                var url = $"https://api.weather.gov/points/{coordinates.Latitude},{coordinates.Longitude}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);
                var forecastUrl = json["properties"]["forecastGridData"];
                
                response = await _httpClient.GetAsync((string)forecastUrl);
                response.EnsureSuccessStatusCode();
                
                var forecastContent = await response.Content.ReadAsStringAsync();
                var forecastJson = JObject.Parse(forecastContent);

                var maxTemperaturesList = forecastJson["properties"]["maxTemperature"]["values"].ToString();
                var minTemperaturesList = forecastJson["properties"]["minTemperature"]["values"].ToString();

                var forecast = new WeatherForecast
                {
                    MaxTemperatures = JsonConvert.DeserializeObject<IEnumerable<Temperature>>(maxTemperaturesList),
                    MinTemperatures = JsonConvert.DeserializeObject<IEnumerable<Temperature>>(minTemperaturesList)
                };

                return forecast;
            }
            catch (Exception ex)
            {
                throw new Exception("Could not retrieve forecast from National Weather API", ex);
            }
        }
    }
}
