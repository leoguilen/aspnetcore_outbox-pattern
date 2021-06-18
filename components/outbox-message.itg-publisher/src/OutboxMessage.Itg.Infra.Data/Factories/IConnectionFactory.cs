using System.Data;

namespace OutboxMessage.Itg.Infra.Data.Factories
{
    public interface IConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
