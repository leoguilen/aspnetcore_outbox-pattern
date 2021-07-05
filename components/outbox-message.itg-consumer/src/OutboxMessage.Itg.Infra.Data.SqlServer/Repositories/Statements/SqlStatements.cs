namespace OutboxMessage.Itg.Infra.Data.SqlServer.Repositories.Statements
{
    internal static class SqlStatements
    {
        public const string UpdateStateMessageStmt = @"
            UPDATE OutboxMessage SET State = 3 WHERE Id = @id";
    }
}
