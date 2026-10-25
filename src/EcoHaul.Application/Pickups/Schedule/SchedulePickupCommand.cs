using EcoHaul.Application.Abstractions.Messaging;

namespace EcoHaul.Application.Pickups.Schedule;

public sealed record SchedulePickupCommand(Guid PickupId, Guid CustomerId) : ICommand;
