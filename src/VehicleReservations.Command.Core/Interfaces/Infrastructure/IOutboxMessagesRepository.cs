using System.Threading.Tasks;

namespace VehicleReservations.Command.Core.Interfaces.Infrastructure
{
    public interface IOutboxMessagesRepository
    {
        Task AddAsync(object message);
    }
}
