namespace OutboxMessage.Itg.Infra.Data.Repositories.Statements
{
    internal static class SqlStatements
    {
        public const string SelectReadyToSendMessagesStmt = @"
            SELECT Id, Application, Event, CorrelationId, Payload, State, EmitedOn, ModifiedOn
            FROM OutboxMessage WHERE State = 1";

        public const string UpdateStateMessageStmt = @"
            UPDATE OutboxMessage SET State = 2 WHERE Id = @id";
    }
}
