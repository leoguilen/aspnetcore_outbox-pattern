using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;

namespace OutboxMessage.Itg.Consumerr
{
    internal static class Program
    {
        private static async Task Main()
        {
            await using var app = Startup.SetupApplication();
            await app.GetRequiredService<IConsumer>().ConsumeAsync();
            await Task.Delay(Timeout.Infinite);
        }
    }
}
