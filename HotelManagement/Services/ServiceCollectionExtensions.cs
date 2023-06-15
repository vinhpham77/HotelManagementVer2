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
        
        services.AddHttpClient<CustomerService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        services.AddHttpClient<MenuService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<RentRoomService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<ReOrderService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<RoomService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<ReservationDetailService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<OrderService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<MenuItemService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });
        
        services.AddHttpClient<AccountService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        services.AddHttpClient<StaffService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        services.AddHttpClient<ReceiptService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        services.AddHttpClient<HistoryRentService>(client =>
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(apiSettings.BaseUrl);
        });

        return services;
    }

}