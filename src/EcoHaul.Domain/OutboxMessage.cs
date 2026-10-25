using System.ComponentModel.DataAnnotations;

namespace EcoHaul.Domain;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }

    public OutboxMessageType Type { get; set; }

    [Required]
    public string Payload { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? SentAt { get; set; }
}
