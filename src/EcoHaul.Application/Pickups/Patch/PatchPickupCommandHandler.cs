using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Application.Persistence;
using EcoHaul.Domain;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Patch;

internal sealed class PatchPickupCommandHandler(IEcoHaulDbContext db)
    : ICommandHandler<PatchPickupCommand>
{
    public async Task<Result> Handle(PatchPickupCommand command, CancellationToken cancellationToken)
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

        Result patched = PickupExecution.Run(() => found.Value.Patch(
            command.SiteName.IsSpecified ? command.SiteName.Value : null,
            command.Material.IsSpecified ? command.Material.Value : null,
            command.WeightKg.IsSpecified ? command.WeightKg.Value : null,
            command.RequestedOn.IsSpecified ? command.RequestedOn.Value : null,
            command.Notes.Value,
            command.Notes.IsSpecified));

        if (patched.IsFailure)
        {
            return patched;
        }

        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
