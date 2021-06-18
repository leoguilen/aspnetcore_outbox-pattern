using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleReservations.Command.ApplicationServices.Feature;
using VehicleReservations.Command.Core.Interfaces.Services;

namespace VehicleReservations.Command.ApplicationServices.Features.RenewReserve
{
    internal class RenewReserveCommandHandler : IRequestHandler<RenewReserveCommand>
    {
        private readonly IVehiclesReserveService _vehiclesReserveService;

        public RenewReserveCommandHandler(IVehiclesReserveService vehiclesReserveService)
        {
            _vehiclesReserveService = vehiclesReserveService;
        }

        public async Task<Unit> Handle(
            RenewReserveCommand request,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _vehiclesReserveService
                .RenewReserveToAsync(request.ReserveId, request.Days);

            return Unit.Value;
        }
    }
}
