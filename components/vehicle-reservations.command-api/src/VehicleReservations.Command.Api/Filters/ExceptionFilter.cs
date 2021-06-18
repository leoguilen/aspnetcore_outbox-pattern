using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VehicleReservations.Command.Api.Models;
using VehicleReservations.Command.Core.Interfaces.Infrastructure;

namespace VehicleReservations.Command.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogWriter _logWriter;

        public ExceptionFilter(ILogWriter logWriter) => _logWriter = logWriter;

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var res = ResolveResponse(ex);

            LogException(ex, res);

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(res);
            context.HttpContext.Response.StatusCode = res.StatusCode;
        }

        private static Error ResolveResponse(Exception ex) => ex switch
        {
            ValidationException vex => Error.FromValidation(vex),
            _ => Error.FromDefault(ex)
        };

        private void LogException(Exception ex, Error error)
        {
            var message = error?.Errors?.FirstOrDefault()?.Detail ?? ex.Message;
            _logWriter.Error(message, StatusCodes.Status500InternalServerError, ex: ex);
        }
    }
}
