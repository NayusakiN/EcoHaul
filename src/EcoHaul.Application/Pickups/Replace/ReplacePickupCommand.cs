using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;

namespace EcoHaul.Application.Pickups.Replace;

public sealed record ReplacePickupCommand(
    Guid PickupId,
    Guid CustomerId,
    string SiteName,
    Material Material,
    decimal WeightKg,
    DateOnly RequestedOn,
    string? Notes) : ICommand;
