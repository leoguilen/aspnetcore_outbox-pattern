namespace VehicleReservations.Command.Infrastructure.Data.Repositories.Statements
{
    internal static class SqlStatements
    {
        public const string SelectReserve = @"
            SELECT 
            CASE
                WHEN COUNT(*) >= 1 THEN 'true'
                ELSE 'false'
            END
            FROM VehicleReservation WHERE ReserveId = @ReserveId";

        public const string SelectVehicleAvailable = @"
            SELECT
            CASE
                WHEN COUNT(*) >= 1 THEN 'false'
                ELSE 'true'
            END
            FROM VehicleReservation WHERE VehicleId = @VehicleId";

        public const string SelectReserveExpireDate = @"
            SELECT ReservationExpiresOn FROM VehicleReservation WHERE ReserveId = @ReserveId";

        public const string UpdateReserveExpireDate = @"
            UPDATE VehicleReservation 
            SET ReservationExpiresOn = @ReservationExpiresOn 
            WHERE ReserveId = @ReserveId";

        public const string UpdateReserveStatus = @"
            UPDATE VehicleReservation 
            SET Status = 2
            WHERE ReserveId = @ReserveId";

        public const string InsertReserve = @"
            INSERT INTO VehicleReservation 
            (
                ReserveId,
                VehicleId,
                CustomerId,
                ReservedAt,
                ReservationExpiresOn,
                Value,
                Status
            )
            VALUES
            (
                @ReserveId,
                @VehicleId,
                @CustomerId,
                @ReservedAt,
                @ReservationExpiresOn,
                @Value,
                @Status
            )";

        public const string InsertOutboxMessage = @"
            INSERT INTO OutboxMessage 
            (
                Application,
                Event,
                CorrelationId,
                Payload,
                State,
                EmitedOn,
                ModifiedOn
            )
            VALUES
            (
                @Application,
                @Event,
                @CorrelationId,
                @Payload,
                @State,
                @EmitedOn,
                @ModifiedOn
            )";
    }
}
