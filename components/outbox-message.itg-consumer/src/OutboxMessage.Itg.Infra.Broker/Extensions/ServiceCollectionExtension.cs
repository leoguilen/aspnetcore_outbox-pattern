using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Broker.Configurations;
using OutboxMessage.Itg.Infra.Broker.Helpers;
using OutboxMessage.Itg.Infra.Broker.RabbitMQ;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Infra.Broker.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfraBroker(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton(configuration.GetSection("Rabbit").Get<RabbitConfiguration>())
                .AddSingleton<IConnectionFactory>(provider =>
                {
                    var rabbitConfig = provider.GetService<RabbitConfiguration>();

                    return new ConnectionFactory
                    {
                        HostName = rabbitConfig.HostName,
                        Port = rabbitConfig.Port,
                        UserName = rabbitConfig.UserName,
                        Password = rabbitConfig.Password,
                        VirtualHost = rabbitConfig.VirtualHost,
                        DispatchConsumersAsync = true,
                    };
                })
                .AddSingleton<IMessageParser, MessageParser>()
                .AddSingleton<IRabbitConnection, RabbitConnection>()
                .AddSingleton<IConsumer, RabbitConsumer>();
    }
}
