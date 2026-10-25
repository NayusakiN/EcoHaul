using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Application.Messaging;
using EcoHaul.Application.Persistence;
using EcoHaul.Domain;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Schedule;

internal sealed class SchedulePickupCommandHandler(IEcoHaulDbContext db)
    : ICommandHandler<SchedulePickupCommand>
{
    public async Task<Result> Handle(SchedulePickupCommand command, CancellationToken cancellationToken)
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

        Result scheduled = PickupExecution.Run(() => found.Value.Schedule());

        if (scheduled.IsFailure)
        {
            return scheduled;
        }

        db.OutboxMessages.Add(PickupChangedOutbox.Create(found.Value));
        await db.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
