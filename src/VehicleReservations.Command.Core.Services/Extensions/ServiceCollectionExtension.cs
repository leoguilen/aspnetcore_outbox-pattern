using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Core.Interfaces.Services;
using VehicleReservations.Command.Core.Services.Services;

namespace VehicleReservations.Command.Core.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services) =>
            services
                .AddScoped<IVehiclesReserveService, VehiclesReserveService>();
    }
}
