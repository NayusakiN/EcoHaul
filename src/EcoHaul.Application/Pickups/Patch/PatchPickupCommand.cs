using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Patch;

public sealed record PatchPickupCommand(
    Guid PickupId,
    Guid CustomerId,
    Optional<string?> SiteName,
    Optional<Material?> Material,
    Optional<decimal?> WeightKg,
    Optional<DateOnly?> RequestedOn,
    Optional<string?> Notes) : ICommand;
