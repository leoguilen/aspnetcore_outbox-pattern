using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OutboxMessage.Itg.Infra.CrossCutting.IoC.DependencyInjection;
using RabbitMQ.Client;

namespace OutboxMessage.Itg.Publisher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddIoc(hostContext.Configuration)
                        .AddHostedService<Worker>()
                        .AddHealthChecks()
                            .AddSqlServer(
                                connectionString: hostContext.Configuration["Database:ConnectionString"],
                                healthQuery: "SELECT 1",
                                failureStatus: HealthStatus.Unhealthy,
                                timeout: TimeSpan.FromSeconds(1))
                            .AddRabbitMQ(
                                connectionFactoryFactory: provider => provider.GetService<IConnectionFactory>(),
                                failureStatus: HealthStatus.Unhealthy,
                                timeout: TimeSpan.FromSeconds(1));
                })
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>())
                .UseConsoleLifetime();
    }
}
