using EcoHaul.Domain;
using EcoHaul.Application.Persistence;
using EcoHaul.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Application.Pickups;

internal static class PickupExecution
{
    public static async Task<Result<Pickup>> Find(
        IEcoHaulDbContext db,
        Guid pickupId,
        Guid customerId,
        CancellationToken cancellationToken)
    {
        Pickup? pickup = await db.Pickups.FirstOrDefaultAsync(
            item => item.Id == pickupId && item.CustomerId == customerId,
            cancellationToken);

        if (pickup is null)
        {
            return Result.Failure<Pickup>(PickupErrors.NotFound(pickupId));
        }

        return pickup;
    }

    public static Result Run(Action action)
    {
        try
        {
            action();
            return Result.Success();
        }
        catch (PickupValidationException exception)
        {
            return Result.Failure(PickupErrors.Invalid(exception.Message));
        }
        catch (PickupConflictException exception)
        {
            return Result.Failure(PickupErrors.Conflict(exception.Message));
        }
    }

    public static Result<T> Run<T>(Func<T> action)
    {
        try
        {
            return action();
        }
        catch (PickupValidationException exception)
        {
            return Result.Failure<T>(PickupErrors.Invalid(exception.Message));
        }
        catch (PickupConflictException exception)
        {
            return Result.Failure<T>(PickupErrors.Conflict(exception.Message));
        }
    }
}
