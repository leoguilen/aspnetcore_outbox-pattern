using System.Data;

namespace OutboxMessage.Itg.Infra.Data.Providers
{
    internal interface IConnectionProvider
    {
        IDbConnection CreateConnection(string providerName, string connectionString);
    }
}
