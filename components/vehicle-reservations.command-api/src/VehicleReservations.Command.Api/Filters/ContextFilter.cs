using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using VehicleReservations.Command.Core.Holders;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;

namespace VehicleReservations.Command.Api.Filters
{
    public class ContextFilter : IActionFilter
    {
        private const string CorrelationId = "x-correlation-id";

        private readonly ILogWriter _logWriter;
        private readonly IRequestContextHolder _requestContextHolder;
        private readonly string _applicationName;

        public ContextFilter(
            ILogWriter logWriter,
            IRequestContextHolder requestContextHolder,
            IConfiguration configuration)
        {
            _logWriter = logWriter;
            _requestContextHolder = requestContextHolder;
            _applicationName = configuration["AppSettings:Application"];
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            var controller = (string)httpContext.Request.RouteValues["controller"];
            var operation = (string)httpContext.Request.RouteValues["action"];
            var correlationId = GetCorrelationId(httpContext);
            var traceId = httpContext.TraceIdentifier;

            _logWriter.SetupLogger(controller, operation, correlationId, traceId);
            _requestContextHolder.ApplicationName = _applicationName;
            _requestContextHolder.CorrelationId = correlationId;
        }

        private static Guid GetCorrelationId(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            var correlationId = headers[CorrelationId];
            return Guid.TryParse(correlationId, out var guid) ? guid : Guid.NewGuid();
        }
    }
}
