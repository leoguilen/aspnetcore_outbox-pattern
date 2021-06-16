using System.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Sqlite;
using VehicleReservations.Command.Infrastructure.Data.Factories;

namespace VehicleReservations.Command.FunctionalTest.Fixtures.DataFixture
{
    internal class SqLiteConnectionFactory : IConnectionFactory
    {
        private readonly OrmLiteConnectionFactory _databaseFactory =
            new(":memory:", SqliteOrmLiteDialectProvider.Instance);

        public IDbConnection GetNewConnection() =>
            _databaseFactory.OpenDbConnection();
    }
}
