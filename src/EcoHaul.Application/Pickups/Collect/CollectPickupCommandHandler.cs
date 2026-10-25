using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Application.Messaging;
using EcoHaul.Application.Persistence;
using EcoHaul.Domain;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Collect;

internal sealed class CollectPickupCommandHandler(IEcoHaulDbContext db)
    : ICommandHandler<CollectPickupCommand>
{
    public async Task<Result> Handle(CollectPickupCommand command, CancellationToken cancellationToken)
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

        Result collected = PickupExecution.Run(() => found.Value.Collect());

        if (collected.IsFailure)
        {
            return collected;
        }

        db.OutboxMessages.Add(PickupChangedOutbox.Create(found.Value));
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
