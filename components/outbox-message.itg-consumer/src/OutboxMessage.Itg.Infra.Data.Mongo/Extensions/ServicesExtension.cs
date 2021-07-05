using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Data.Mongo.Repositories;

namespace OutboxMessage.Itg.Infra.Data.Mongo.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServicesExtension
    {
        public static IServiceCollection AddInfraDataMongoDb(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .ConfigureMongoConnection(configuration)
                .AddSingleton<IReservationRepository, ReservationRepository>();
    }
}
