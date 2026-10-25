using EcoHaul.Domain;

namespace EcoHaul.Application.Messaging;

public sealed record PickupChanged(
    Guid PickupId,
    Guid CustomerId,
    DateOnly RequestedOn,
    PickupStatus Status);
