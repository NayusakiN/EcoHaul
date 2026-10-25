using EcoHaul.Domain;

namespace EcoHaul.Application.Pickups;

public sealed record PickupResponse(
    Guid Id,
    Guid CustomerId,
    string SiteName,
    Material Material,
    decimal WeightKg,
    DateOnly RequestedOn,
    string? Notes,
    PickupStatus Status)
{
    public static PickupResponse From(Pickup pickup) => new(
        pickup.Id,
        pickup.CustomerId,
        pickup.SiteName,
        pickup.Material,
        pickup.WeightKg,
        pickup.RequestedOn,
        pickup.Notes,
        pickup.Status);
}
