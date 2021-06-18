using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Extensions;
using OutboxMessage.Itg.Core.Services.Extensions;
using OutboxMessage.Itg.Infra.Broker.Extensions;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Extensions;
using OutboxMessage.Itg.Infra.Data.Extensions;

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
                .AddInfraData(configuration)
                .AddInfraBroker(configuration)
                .AddCoreService();
    }
}
