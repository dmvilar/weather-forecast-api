using System.Net.Sockets;
using WeatherService.Domain.Entities;

namespace WeatherService.Infrastructure.Interfaces
{
    public interface IGeocodingService
    {
        Task<Coordinates> GetCoordinates(Address address);
    }
}
