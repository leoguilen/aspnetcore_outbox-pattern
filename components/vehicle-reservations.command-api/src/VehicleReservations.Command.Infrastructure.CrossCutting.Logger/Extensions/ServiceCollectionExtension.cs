using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Logging;

namespace VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLogger(this IServiceCollection services) =>
            services
                .AddSingleton<ILogWriter, SerilogLogWriter>();
    }
}
