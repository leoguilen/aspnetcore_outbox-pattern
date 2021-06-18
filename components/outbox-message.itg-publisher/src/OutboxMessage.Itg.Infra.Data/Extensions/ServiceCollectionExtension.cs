using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Data.Configurations;
using OutboxMessage.Itg.Infra.Data.Factories;
using OutboxMessage.Itg.Infra.Data.Providers;
using OutboxMessage.Itg.Infra.Data.Repositories;

namespace OutboxMessage.Itg.Infra.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfraData(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("Database").Get<DatabaseSettings>())
                .AddSingleton<IConnectionProvider, ConnectionProvider>()
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IOutboxMessageRepository, OutboxMessageRepository>();
    }
}
