using System;

namespace VehicleReservations.Command.Core.Holders
{
    public interface IRequestContextHolder
    {
        public string ApplicationName { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
