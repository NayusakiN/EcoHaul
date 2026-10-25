using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups;

public static class PickupErrors
{
    public static Error NotFound(Guid pickupId) => Error.NotFound(
        "Pickups.NotFound",
        $"The pickup with Id = '{pickupId}' was not found.");

    public static Error Conflict(string description) => Error.Conflict(
        "Pickups.Conflict",
        description);

    public static Error Invalid(string description) => Error.Problem(
        "Pickups.Invalid",
        description);
}
