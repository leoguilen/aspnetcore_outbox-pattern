using System;
using System.Data;
using ServiceStack.OrmLite;
using VehicleReservations.Command.Infrastructure.Data.Factories;

namespace VehicleReservations.Command.FunctionalTest.Fixtures.DataFixture
{
    internal class SqLiteDatabaseFixture
    {
        public SqLiteDatabaseFixture(IConnectionFactory connectionFactory)
        {
            Connection = connectionFactory.GetNewConnection();
            InitDatabase();
        }

        public IDbConnection Connection { get; }

        public bool ExistsReserveBy(Guid vehicleId, Guid customerId) =>
            Connection.Exists<dynamic>(
                SqlStatements.SelectByVehicleAndCustomerIdStmt,
                new { vehicleId, customerId });

        public bool IsCanceledReserve(Guid reserveId) =>
            Connection.Exists<dynamic>(
                SqlStatements.CheckIsCanceledReserve,
                new { reserveId });

        public bool IsRenewedReserve(Guid reserveId, DateTime expireDate) =>
            Connection.Exists<dynamic>(
                SqlStatements.CheckIsRenewedReserve,
                new { reserveId, expireDate });

        public void InsertReserve(object obj) =>
            Connection.ExecuteSql(SqlStatements.InsertReserveStmt, obj);

        public void DropDatabase() =>
            Connection.ExecuteSql(SqlStatements.DropDatabaseStmt);

        private void InitDatabase() =>
            Connection.ExecuteSql(SqlStatements.CreateDatabaseStmt);
    }
}
