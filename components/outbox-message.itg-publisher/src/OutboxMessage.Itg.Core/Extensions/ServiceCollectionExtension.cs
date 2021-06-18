using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Configurations;

namespace OutboxMessage.Itg.Core.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>());
    }
}
