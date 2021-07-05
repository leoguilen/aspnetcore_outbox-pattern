using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace OutboxMessage.Itg.Infra.Data.Mongo.Extensions
{
    internal static class DatabaseExtensions
    {
        private static string _databaseName;

        public static IServiceCollection ConfigureMongoConnection(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton<IMongoClient>(provider =>
                {
                    var connectionString = GetConnectionString(configuration);
                    _databaseName = MongoUrl.Create(connectionString).DatabaseName;
                    return new MongoClient(connectionString);
                })
                .AddSingleton(provider =>
                {
                    var mongoClient = provider.GetRequiredService<IMongoClient>();
                    return mongoClient.GetDatabase(_databaseName);
                });

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDb:ConnectionString"];
            return connectionString;
        }
    }
}
