using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.Core.Enums;
using VehicleReservations.Command.Core.Interfaces.Services;
using VehicleReservations.Command.Core.Models;

namespace VehicleReservations.Command.ApplicationServices.Features.CreateReserve
{
    internal class CreateReserveCommandHandler : IRequestHandler<CreateReserveCommand>
    {
        private readonly IVehiclesReserveService _vehiclesReserveService;

        public CreateReserveCommandHandler(IVehiclesReserveService vehiclesReserveService)
        {
            _vehiclesReserveService = vehiclesReserveService;
        }

        public async Task<Unit> Handle(
            CreateReserveCommand request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var newVehicleReservation = new VehicleReservation()
            {
                VehicleId = request.VehicleId,
                CustomerId = request.CustomerId,
                ReservedAt = request.ReservedAt,
                ReservationExpiresOn = request.ReservationExpiresOn,
                Value = request.Value,
                Status = (ReserveStatus)request.Status,
            };

            await _vehiclesReserveService
                .CreateFromAsync(newVehicleReservation);

            return Unit.Value;
        }
    }
}
