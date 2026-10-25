using System.ComponentModel.DataAnnotations;

namespace EcoHaul.Domain;

public sealed class Notification
{
    public Guid Id { get; init; }

    public Guid PickupId { get; init; }

    public NotificationState State { get; set; }

    [MaxLength(64)]
    public string? OperationId { get; set; }
}
