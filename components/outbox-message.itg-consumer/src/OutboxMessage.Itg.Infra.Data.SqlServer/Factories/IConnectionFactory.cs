using System.Data;

namespace OutboxMessage.Itg.Infra.Data.SqlServer.Factories
{
    public interface IConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
