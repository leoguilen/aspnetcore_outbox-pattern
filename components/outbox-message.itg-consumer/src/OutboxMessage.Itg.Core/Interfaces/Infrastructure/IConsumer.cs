using System.Threading.Tasks;

namespace OutboxMessage.Itg.Core.Interfaces.Infrastructure
{
    public interface IConsumer
    {
        Task ConsumeAsync();
    }
}
