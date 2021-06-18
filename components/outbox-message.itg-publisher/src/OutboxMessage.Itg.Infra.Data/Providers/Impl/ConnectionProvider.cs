using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace OutboxMessage.Itg.Infra.Data.Providers
{
    internal class ConnectionProvider : IConnectionProvider
    {
        private readonly Dictionary<string, IDbConnection> _providersDic = new(1)
        {
            { "sqlserver", new SqlConnection() },
        };

        public IDbConnection CreateConnection(
            string providerName,
            string connectionString)
        {
            var provider = _providersDic[providerName.ToLowerInvariant()];
            provider.ConnectionString = connectionString;

            return provider;
        }
    }
}
