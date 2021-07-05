using System.Threading.Tasks;
using OutboxMessage.Itg.Core.Models;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface IReservationRepository
    {
        Task PersistAsync(VehicleReservation reservation);
    }
}
