using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.ApplicationServices.Extensions;
using VehicleReservations.Command.Core.Extensions;
using VehicleReservations.Command.Core.Services.Extensions;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Extensions;
using VehicleReservations.Command.Infrastructure.Data.Extensions;

namespace VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddCore(configuration)
                .AddLogger()
                .AddInfraData(configuration)
                .AddCoreService()
                .AddAppServices();
        }
    }
}
