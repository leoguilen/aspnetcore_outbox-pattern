using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Infrastructure.Data.Configurations;
using VehicleReservations.Command.Infrastructure.Data.Factories;
using VehicleReservations.Command.Infrastructure.Data.Providers;
using VehicleReservations.Command.Infrastructure.Data.Providers.Impl;
using VehicleReservations.Command.Infrastructure.Data.Repositories;
using VehicleReservations.Command.Infrastructure.Data.UoW;

namespace VehicleReservations.Command.Infrastructure.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfraData(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>())
                .AddSingleton<IConnectionProvider, ConnectionProvider>()
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IUnitOfWork, UnitOfWork>()
                .AddScoped<IReserveRepository, ReserveRepository>()
                .AddScoped<IOutboxMessagesRepository, OutboxMessagesRepository>();
    }
}
