using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VehicleReservations.Command.Api.Models;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;
using VehicleReservations.Command.Core.Notifications;

namespace VehicleReservations.Command.Api.Filters
{
    public class RequestFilter : IActionFilter
    {
        private readonly ILogWriter _logWriter;
        private readonly INotification _notification;

        public RequestFilter(ILogWriter logWriter, INotification notification)
        {
            _notification = notification;
            _logWriter = logWriter;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_notification.Any())
            {
                var error = Error.FromNotification(_notification);
                context.Result = new ObjectResult(error)
                {
                    StatusCode = error.StatusCode,
                };

                _logWriter.Error("Business error", error.StatusCode, error);
                return;
            }

            _logWriter.Info(nameof(OnActionExecuted), context.HttpContext.Response.StatusCode);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logWriter.Info(nameof(OnActionExecuting), null);
        }
    }
}
