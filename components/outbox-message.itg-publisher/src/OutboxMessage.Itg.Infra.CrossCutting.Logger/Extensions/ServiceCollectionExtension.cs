using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.CrossCutting.Logger.Logging;

namespace OutboxMessage.Itg.Infra.CrossCutting.Logger.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLogger(this IServiceCollection services) =>
            services
                .AddSingleton<ILogWriter, SerilogLogWriter>();
    }
}
