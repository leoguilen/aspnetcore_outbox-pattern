using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using VehicleReservations.Command.Api.Extensions;
using VehicleReservations.Command.Infrastructure.CrossCutting.Ioc.DependencyInjection;

namespace OutboxPattern.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddApi()
                .AddIoc(Configuration)
                .AddHealthChecks()
                    .AddSqlServer(
                        connectionString: Configuration["Database:ConnectionString"],
                        healthQuery: "SELECT 1",
                        failureStatus: HealthStatus.Unhealthy,
                        timeout: TimeSpan.FromSeconds(1));

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .ConfigureSwagger(Configuration)
                .UseHttpsRedirection()
                .UseRouting()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
                    {
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                        },
                    });
                })
                .UseResponseCompression();
        }
    }
}
