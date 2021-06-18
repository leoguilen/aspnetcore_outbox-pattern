using System;
using OutboxMessage.Itg.Core.Enums;

namespace OutboxMessage.Itg.Core.Models
{
    public record OutboxMessage
    {
        public Guid Id { get; set; }

        public string Application { get; init; }

        public string Event { get; init; }

        public Guid CorrelationId { get; init; }

        public string Payload { get; init; }

        public OutboxMessageState State { get; init; }

        public DateTime EmitedOn { get; init; }

        public DateTime? ModifiedOn { get; init; }
    }
}
