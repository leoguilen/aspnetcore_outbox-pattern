using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OutboxMessage.Itg.Infra.CrossCutting.IoC.DependencyInjection;

namespace OutboxMessage.Itg.Consumerr
{
    internal static class Startup
    {
        public static ServiceProvider SetupApplication()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            return new ServiceCollection()
                .AddIoc(configuration)
                .BuildServiceProvider();
        }
    }
}
