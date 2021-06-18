using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using OutboxMessage.Itg.Core.Interfaces.Infrastructure;
using OutboxMessage.Itg.Infra.Data.Factories;
using OutboxMessage.Itg.Infra.Data.Repositories.Statements;

namespace OutboxMessage.Itg.Infra.Data.Repositories
{
    internal class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly IDbConnection _connection;

        public OutboxMessageRepository(IConnectionFactory factory) =>
            _connection = factory.GetNewConnection();

        public async Task<IAsyncEnumerable<Core.Models.OutboxMessage>> GetAllMessagesAvailableAsync() =>
            (await _connection.QueryAsync<Core.Models.OutboxMessage>(
                sql: SqlStatements.SelectReadyToSendMessagesStmt))
                .ToAsyncEnumerable();

        public async Task UpdateMessageStateToSendToQueueAsync(Guid id) =>
            await _connection.ExecuteAsync(
                sql: SqlStatements.UpdateStateMessageStmt,
                param: new { id });
    }
}
