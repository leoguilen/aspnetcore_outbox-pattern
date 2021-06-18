using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.Core.Interfaces.Services;

namespace VehicleReservations.Command.ApplicationServices.Features.CancelReserve
{
    internal class CancelReserveCommandHandler : IRequestHandler<CancelReserveCommand>
    {
        private readonly IVehiclesReserveService _vehiclesReserveService;

        public CancelReserveCommandHandler(IVehiclesReserveService vehiclesReserveService)
        {
            _vehiclesReserveService = vehiclesReserveService;
        }

        public async Task<Unit> Handle(
            CancelReserveCommand request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _vehiclesReserveService
                .CancelToAsync(request.ReserveId);

            return Unit.Value;
        }
    }
}
