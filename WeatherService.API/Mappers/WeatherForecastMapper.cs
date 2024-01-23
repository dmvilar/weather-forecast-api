using WeatherService.API.ViewModels;
using WeatherService.Domain.Entities;

namespace WeatherService.API.Mappers
{
    public class WeatherForecastMapper
    {
        public static List<WeatherForecastViewModel> ToWeatherForecastViewModel(WeatherForecast weatherForecast)
        {
            var maxTemperaturesDict = weatherForecast.MaxTemperatures
                .ToDictionary(t => t.ValidTime.Split('T')[0], t => t.Value);

            var minTemperaturesDict = weatherForecast.MinTemperatures
                .ToDictionary(t => t.ValidTime.Split('T')[0], t => t.Value);

            var allDates = new HashSet<string>(maxTemperaturesDict.Keys.Concat(minTemperaturesDict.Keys));

            var viewModel = allDates
                .OrderBy(date => date)
                .Take(7)
                .Select(date =>
                {
                    maxTemperaturesDict.TryGetValue(date, out double maxTempValue);
                    minTemperaturesDict.TryGetValue(date, out double minTempValue);

                    return new WeatherForecastViewModel
                    {
                        Date = date,
                        MaxTemperature = maxTemperaturesDict.ContainsKey(date) ? Math.Round(maxTempValue).ToString() : "-",
                        MinTemperature = minTemperaturesDict.ContainsKey(date) ? Math.Round(minTempValue).ToString() : "-"
                    };
                })
                .ToList();

            return viewModel;
        }
    }
}
