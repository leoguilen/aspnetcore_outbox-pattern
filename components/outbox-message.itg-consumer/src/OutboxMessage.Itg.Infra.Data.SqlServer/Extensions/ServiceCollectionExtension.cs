using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Data.SqlServer.Configurations;
using OutboxMessage.Itg.Infra.Data.SqlServer.Factories;
using OutboxMessage.Itg.Infra.Data.SqlServer.Providers;
using OutboxMessage.Itg.Infra.Data.SqlServer.Repositories;

namespace OutboxMessage.Itg.Infra.Data.SqlServer.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfraDataSqlServer(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("SqlServer").Get<DatabaseSettings>())
                .AddSingleton<IConnectionProvider, ConnectionProvider>()
                .AddSingleton<IConnectionFactory, ConnectionFactory>()
                .AddSingleton<IOutboxMessageRepository, OutboxMessageRepository>();
    }
}
