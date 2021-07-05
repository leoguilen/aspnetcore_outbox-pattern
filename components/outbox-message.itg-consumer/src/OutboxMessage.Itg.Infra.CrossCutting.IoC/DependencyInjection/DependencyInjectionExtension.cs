using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Extensions;
using OutboxMessage.Itg.Core.Services.Extensions;
using OutboxMessage.Itg.Infra.Broker.Extensions;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Extensions;
using OutboxMessage.Itg.Infra.Data.Mongo.Extensions;
using OutboxMessage.Itg.Infra.Data.SqlServer.Extensions;

namespace OutboxMessage.Itg.Infra.CrossCutting.IoC.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddIoc(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddCore(configuration)
                .AddLogger()
                .AddInfraDataSqlServer(configuration)
                .AddInfraDataMongoDb(configuration)
                .AddInfraBroker(configuration)
                .AddCoreService();
    }
}
