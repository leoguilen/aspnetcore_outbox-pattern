using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Services;
using OutboxMessage.Itg.Core.Services.Services;

namespace OutboxMessage.Itg.Core.Services.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services) =>
            services
                .AddSingleton<IOutboxMessageItgService, OutboxMessageItgService>();
    }
}
