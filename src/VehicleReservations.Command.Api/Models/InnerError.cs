#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace VehicleReservations.Command.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class InnerError
    {
        public InnerError()
        {
            // needed for deserialization
        }

        private InnerError(string title, string detail, HttpStatusCode statusCode)
        {
            Title = title;
            Detail = detail;
            Status = ((int)statusCode).ToString();
        }

        public string Title { get; set; } = string.Empty;

        public string Detail { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string? Code { get; set; }

        public static InnerError FromValidation(ValidationException vex, HttpStatusCode statusCode) =>
            new("validation error", vex.Message, statusCode);

        public static InnerError FromDefault(Exception ex, HttpStatusCode statusCode) =>
            new("unexpected error", ex.Message, statusCode);
    }
}
