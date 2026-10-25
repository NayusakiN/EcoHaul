using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;
using EcoHaul.Application.Persistence;
using EcoHaul.SharedKernel;

namespace EcoHaul.Application.Pickups.Create;

internal sealed class CreatePickupCommandHandler(IEcoHaulDbContext db)
    : ICommandHandler<CreatePickupCommand, PickupResponse>
{
    public async Task<Result<PickupResponse>> Handle(CreatePickupCommand command, CancellationToken cancellationToken)
    {
        Result<Pickup> created = PickupExecution.Run(() => Pickup.Create(
            command.CustomerId,
            command.SiteName,
            command.Material,
            command.WeightKg,
            command.RequestedOn,
            command.Notes));

        if (created.IsFailure)
        {
            return Result.Failure<PickupResponse>(created.Error);
        }

        db.Pickups.Add(created.Value);
        await db.SaveChangesAsync(cancellationToken);

        return PickupResponse.From(created.Value);
    }
}
