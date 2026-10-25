using EcoHaul.Application.Abstractions.Messaging;

namespace EcoHaul.Application.Pickups.Collect;

public sealed record CollectPickupCommand(Guid PickupId, Guid CustomerId) : ICommand;
