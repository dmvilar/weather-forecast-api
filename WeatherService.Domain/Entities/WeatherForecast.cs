using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.Domain.Entities
{
    public class WeatherForecast
    {
        public IEnumerable<Temperature> MaxTemperatures { get; set; }
        public IEnumerable<Temperature> MinTemperatures { get; set; }
    }

    public class Temperature
    {
        [JsonProperty("validTime")]
        public string ValidTime { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
