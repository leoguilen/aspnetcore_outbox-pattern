using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OutboxMessage.Itg.Infra.Data.Mongo.Models
{
    internal class ReservationDocument
    {
        [BsonElement("RESERVE_UUID")]
        [BsonRepresentation(BsonType.String)]
        public Guid ReserveId { get; init; }

        [BsonElement("VEHICLE_UUID")]
        [BsonRepresentation(BsonType.String)]
        public Guid VehicleId { get; init; }

        [BsonElement("CUSTOMER_UUID")]
        [BsonRepresentation(BsonType.String)]
        public Guid CustomerId { get; init; }

        [BsonElement("RESERVED_AT")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime, Kind = DateTimeKind.Local)]
        public DateTime ReservedAt { get; init; }

        [BsonElement("RESERVED_EXPIRES_ON")]
        [BsonDateTimeOptions(Representation = BsonType.DateTime, Kind = DateTimeKind.Local)]
        public DateTime ReservationExpiresOn { get; init; }

        [BsonElement("VALUE")]
        [BsonRepresentation(BsonType.Double)]
        public decimal Value { get; init; }

        [BsonElement("STATUS")]
        public int Status { get; init; }
    }
}
