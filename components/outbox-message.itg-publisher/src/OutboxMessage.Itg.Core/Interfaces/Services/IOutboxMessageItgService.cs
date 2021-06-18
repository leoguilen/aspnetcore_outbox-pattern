using System.Threading.Tasks;

namespace OutboxMessage.Itg.Core.Interfaces.Services
{
    public interface IOutboxMessageItgService
    {
        Task ExecuteAsync();
    }
}
