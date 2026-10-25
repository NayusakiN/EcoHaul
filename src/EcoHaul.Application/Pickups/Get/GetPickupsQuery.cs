using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;

namespace EcoHaul.Application.Pickups.Get;

public sealed record GetPickupsQuery(
    Guid CustomerId,
    PickupStatus? Status = null,
    DateOnly? RequestedFrom = null,
    DateOnly? RequestedTo = null,
    int Offset = 0,
    int Limit = PickupPage.DefaultLimit) : IQuery<PickupPage>;
