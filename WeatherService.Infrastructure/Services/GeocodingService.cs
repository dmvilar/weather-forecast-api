using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            var url = $"https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={Uri.EscapeDataString(address.StreetAddress)}&benchmark=Public_AR_Current&format=json";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var coordinatesJson = json["result"]["addressMatches"][0]["coordinates"];
            return new Coordinates
            {
                Latitude =  (string)coordinatesJson["y"],
                Longitude = (string)coordinatesJson["x"]
            };
        }
    }
}
