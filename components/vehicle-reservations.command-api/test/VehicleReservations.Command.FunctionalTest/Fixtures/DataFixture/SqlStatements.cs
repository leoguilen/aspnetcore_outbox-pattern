namespace VehicleReservations.Command.FunctionalTest.Fixtures.DataFixture
{
    internal static class SqlStatements
    {
        public const string CreateDatabaseStmt = @"
            CREATE TABLE OutboxMessage
            (
            	Application varchar(50) NOT NULL,
            	Event varchar(50) NOT NULL,
            	CorrelationId uniqueidentifier NOT NULL,
            	Payload varchar(255) NOT NULL,
            	State smallint NOT NULL,
            	EmitedOn datetime2 NOT NULL,
            	ModifiedOn datetime2 NULL
            );
            CREATE TABLE VehicleReservation
            (
            	ReserveId uniqueidentifier NOT NULL,
            	VehicleId uniqueidentifier NOT NULL,
            	CustomerId uniqueidentifier NOT NULL,
            	ReservedAt datetime2 NOT NULL,
            	ReservationExpiresOn datetime2 NOT NULL,
            	Value decimal(18, 2) NOT NULL,
            	Status smallint NOT NULL,
            	CONSTRAINT PK_VehicleReservation PRIMARY KEY 
                (
                    ReserveId ASC,
            	    VehicleId ASC,
                    CustomerId ASC
                )
            );";

        public const string DropDatabaseStmt = @"
            DROP TABLE VehicleReservation;
            DROP TABLE OutboxMessage;";

        public const string SeedDatabaseStmt = "";

        public const string SelectByVehicleAndCustomerIdStmt = @"
            SELECT * FROM VehicleReservation 
            WHERE VehicleId = @vehicleId AND CustomerId = @customerId;";

        public const string CheckIsCanceledReserve = @"
            SELECT * FROM VehicleReservation
            WHERE ReserveId = @reserveId AND Status = 2;";

        public const string CheckIsRenewedReserve = @"
            SELECT * FROM VehicleReservation
            WHERE ReserveId = @reserveId AND ReservationExpiresOn = @expireDate;";

        public const string InsertReserveStmt = @"
            INSERT INTO VehicleReservation VALUES
            (
                @ReserveId,
                @VehicleId,
                @CustomerId,
                @ReservedAt,
                @ReservationExpiresOn,
                @Value,
                @Status
            );";
    }
}
