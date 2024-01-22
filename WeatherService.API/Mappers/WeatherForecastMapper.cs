using WeatherService.API.ViewModels;
using WeatherService.Domain.Entities;

namespace WeatherService.API.Mappers
{
    public class WeatherForecastMapper
    {
        public static List<WeatherForecastViewModel> ToWeatherForecastViewModel(WeatherForecast weatherForecast)
        {
            var viewModel = new List<WeatherForecastViewModel>();

            for (var i = 0; i < 7; i++)
            {
                var maxTemperatures = weatherForecast.MaxTemperatures.ToList()[i];
                var minTemperatures = weatherForecast.MinTemperatures.ToList()[i];

                if(maxTemperatures.ValidTime.Split('T')[0] == minTemperatures.ValidTime.Split('T')[0])
                {
                    var date = maxTemperatures.ValidTime.Split('T')[0];

                    viewModel.Add(new WeatherForecastViewModel
                    {
                        Date = date,
                        MinTemperature = Math.Round(minTemperatures.Value).ToString(),
                        MaxTemperature = Math.Round(maxTemperatures.Value).ToString()
                    });
                }
            }

            return viewModel;
        }
    }
}
