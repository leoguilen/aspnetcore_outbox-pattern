using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using VehicleReservations.Command.Api.Filters;
using VehicleReservations.Command.Api.Holders;
using VehicleReservations.Command.Core.Holders;

namespace VehicleReservations.Command.Api.Extensions
{
    internal static class ApiExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services) =>
            services
                .AddSingleton<IRequestContextHolder, RequestContextHolder>()
                .AddControllers(opt =>
                {
                    opt.Filters.Add<ContextFilter>();
                    opt.Filters.Add<RequestFilter>();
                }).Services
                .ConfigAppVersioning()
                .AddSwagger()
                .Configure<GzipCompressionProviderOptions>(gzipCompressionOptions =>
                    gzipCompressionOptions.Level = CompressionLevel.Fastest)
                .AddResponseCompression(compressionOptions =>
                {
                    compressionOptions.EnableForHttps = true;
                    compressionOptions.Providers.Add<GzipCompressionProvider>();
                });

        private static IServiceCollection ConfigAppVersioning(this IServiceCollection services) =>
            services
                .AddApiVersioning(o => o.ReportApiVersions = true)
                .AddVersionedApiExplorer(o =>
                {
                    o.GroupNameFormat = "'v'VVV";
                    o.SubstituteApiVersionInUrl = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                });
    }
}
