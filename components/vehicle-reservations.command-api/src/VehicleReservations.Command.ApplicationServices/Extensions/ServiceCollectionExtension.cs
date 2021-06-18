using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.ApplicationServices.Feature;

namespace VehicleReservations.Command.ApplicationServices.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services) =>
            services.AddMediatR(typeof(CancelReserveCommand));
    }
}
