using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace VehicleReservations.Command.Api.Extensions
{
    internal static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vehicle Reservations Command API",
                    Description = "Vehicle Reservations Command API (ASP.NET 5.0)",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Leonardo Guilen",
                    },
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                opt.IncludeXmlComments(xmlPath);
            });

        public static IApplicationBuilder ConfigureSwagger(
            this IApplicationBuilder app,
            IConfiguration config)
        {
            var useSwagger = config.GetValue<bool>("AppSettings:UseSwagger");

            return !useSwagger
                ? app
                : app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint($"v1/swagger.json", "v1");
                    opt.DocumentTitle = "Vehicle Reservations Command API";
                    opt.DocExpansion(DocExpansion.List);
                });
        }
    }
}
