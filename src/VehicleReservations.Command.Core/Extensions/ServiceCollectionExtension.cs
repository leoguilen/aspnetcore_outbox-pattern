using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Core.Configurations;
using VehicleReservations.Command.Core.Notifications;

namespace VehicleReservations.Command.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>())
                .AddScoped<INotification, Notification>();
    }
}
