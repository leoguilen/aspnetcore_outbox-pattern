using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Data.SqlServer.Factories;
using OutboxMessage.Itg.Infra.Data.SqlServer.Repositories.Statements;

namespace OutboxMessage.Itg.Infra.Data.SqlServer.Repositories
{
    internal class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly IDbConnection _connection;

        public OutboxMessageRepository(IConnectionFactory factory) =>
            _connection = factory.GetNewConnection();

        public async Task UpdateMessageStateToCompletedAsync(Guid reserveId) =>
            await _connection.ExecuteAsync(
                sql: SqlStatements.UpdateStateMessageStmt,
                param: new { reserveId });
    }
}
