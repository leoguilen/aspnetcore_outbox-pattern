using System.Threading.Tasks;
using OutboxMessage.Itg.Core.Models;

namespace OutboxMessage.Itg.Core.Interfaces.Services
{
    public interface IOutboxMessageItgService
    {
        Task ExecuteAsync(VehicleReservation message);
    }
}
