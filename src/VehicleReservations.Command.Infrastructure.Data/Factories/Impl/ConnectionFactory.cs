using System;
using System.Data;
using VehicleReservations.Command.Infrastructure.Data.Configurations;
using VehicleReservations.Command.Infrastructure.Data.Providers;

namespace VehicleReservations.Command.Infrastructure.Data.Factories
{
    internal class ConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionProvider _connectionProvider;
        private readonly string _connectionString;
        private readonly string _providerName;

        public ConnectionFactory(IConnectionProvider connectionProvider, DatabaseSettings dbSettings)
        {
            if (dbSettings is null)
            {
                throw new ArgumentNullException(nameof(dbSettings));
            }

            _connectionString = dbSettings.ConnectionString;
            _providerName = dbSettings.ProviderName;
            _connectionProvider = connectionProvider;
        }

        public IDbConnection GetNewConnection() =>
            _connectionProvider.CreateConnection(_providerName, _connectionString);
    }
}
