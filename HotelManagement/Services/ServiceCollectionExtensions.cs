using HotelManagement.Models;
using Microsoft.Extensions.Options;

namespace HotelManagement.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpClient<RoomTypeService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        // Register other services...

        services.AddHttpClient<RentRoomService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        return services;
    }

}