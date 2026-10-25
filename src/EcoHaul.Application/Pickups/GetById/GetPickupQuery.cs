using EcoHaul.Application.Abstractions.Messaging;

namespace EcoHaul.Application.Pickups.GetById;

public sealed record GetPickupQuery(Guid PickupId, Guid CustomerId) : IQuery<PickupResponse>;
