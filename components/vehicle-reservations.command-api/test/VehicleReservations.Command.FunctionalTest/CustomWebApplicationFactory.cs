using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxPattern.Api;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.Core.Configurations;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Interfaces.Services;
using VehicleReservations.Command.Core.Notifications;
using VehicleReservations.Command.Core.Services.Services;
using VehicleReservations.Command.FunctionalTest.Fixtures.DataFixture;
using VehicleReservations.Command.Infrastructure.CrossCutting.Logger.Logging;
using VehicleReservations.Command.Infrastructure.Data.Factories;
using VehicleReservations.Command.Infrastructure.Data.Repositories;
using VehicleReservations.Command.Infrastructure.Data.UoW;

namespace VehicleReservations.Command.FunctionalTest
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            builder.ConfigureServices(services =>
            {
                services
                    .AddSingleton(configuration.GetSection("AppSettings").Get<AppSettings>())
                    .AddScoped<Core.Notifications.INotification, Notification>()
                    .AddSingleton<ILogWriter, SerilogLogWriter>()
                    .AddSingleton<IConnectionFactory, SqLiteConnectionFactory>()
                    .AddSingleton<IUnitOfWork, UnitOfWork>()
                    .AddTransient<SqLiteDatabaseFixture>()
                    .AddScoped<IReserveRepository, ReserveRepository>()
                    .AddScoped<IOutboxMessagesRepository, OutboxMessagesRepository>()
                    .AddScoped<IVehiclesReserveService, VehiclesReserveService>()
                    .AddMediatR(typeof(CancelReserveCommand));
            });
        }
    }
}
