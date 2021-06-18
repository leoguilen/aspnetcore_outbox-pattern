using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using VehicleReservations.Command.Core.Notifications;

namespace VehicleReservations.Command.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class Error
    {
        public Error()
        {
            // needed for deserialization
        }

        private Error(InnerError innerError)
        {
            Errors = ImmutableList.Create(innerError);
        }

        public IEnumerable<InnerError> Errors { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int StatusCode
        {
            get
            {
                var status = Errors.FirstOrDefault()?.Status;

                if (string.IsNullOrWhiteSpace(status))
                {
                    return (int)HttpStatusCode.InternalServerError;
                }

                return int.TryParse(status, out var result)
                    ? result
                    : (int)HttpStatusCode.InternalServerError;
            }
        }

        public static Error FromValidation(ValidationException vex) => new(
            InnerError.FromValidation(vex, HttpStatusCode.UnprocessableEntity));

        public static Error FromDefault(Exception ex) => ex switch
        {
            OperationCanceledException => new(
                new()
                {
                    Title = "Operation was canceled",
                    Status = "499",
                    Detail = ex.Message,
                }),
            _ => new(InnerError.FromDefault(ex, HttpStatusCode.InternalServerError)),
        };

        public static Error FromNotification(INotification notification) => new(
            new()
            {
                Title = "Domain error",
                Status = notification.GetErrorStatus(),
                Detail = notification.GetSummary(),
            });
    }
}
