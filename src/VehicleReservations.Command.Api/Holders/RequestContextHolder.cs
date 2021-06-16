using System;
using System.Diagnostics.CodeAnalysis;
using VehicleReservations.Command.Core.Holders;

namespace VehicleReservations.Command.Api.Holders
{
    [ExcludeFromCodeCoverage]
    internal class RequestContextHolder : IRequestContextHolder
    {
        public string ApplicationName { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
