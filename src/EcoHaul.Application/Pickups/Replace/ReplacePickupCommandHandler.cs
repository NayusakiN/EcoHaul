using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Application.Persistence;
using EcoHaul.Domain;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Replace;

internal sealed class ReplacePickupCommandHandler(IEcoHaulDbContext db)
    : ICommandHandler<ReplacePickupCommand>
{
    public async Task<Result> Handle(ReplacePickupCommand command, CancellationToken cancellationToken)
    {
        Result<Pickup> found = await PickupExecution.Find(
            db,
            command.PickupId,
            command.CustomerId,
            cancellationToken);

        if (found.IsFailure)
        {
            return Result.Failure(found.Error);
        }

        Result replaced = PickupExecution.Run(() => found.Value.Replace(
            command.SiteName,
            command.Material,
            command.WeightKg,
            command.RequestedOn,
            command.Notes));

        if (replaced.IsFailure)
        {
            return replaced;
        }

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
