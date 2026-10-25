using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;

namespace EcoHaul.Application.Pickups.Create;

public sealed record CreatePickupCommand(
    Guid CustomerId,
    string SiteName,
    Material Material,
    decimal WeightKg,
    DateOnly RequestedOn,
    string? Notes) : ICommand<PickupResponse>;
