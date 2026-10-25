using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Application.Persistence;
using EcoHaul.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Application.Pickups.GetById;

internal sealed class GetPickupQueryHandler(IEcoHaulDbContext db)
    : IQueryHandler<GetPickupQuery, PickupResponse>
{
    public async Task<Result<PickupResponse>> Handle(GetPickupQuery query, CancellationToken cancellationToken)
    {
        PickupResponse? pickup = await db.Pickups
            .AsNoTracking()
            .Where(item => item.Id == query.PickupId && item.CustomerId == query.CustomerId)
            .Select(item => new PickupResponse(
                item.Id,
                item.CustomerId,
                item.SiteName,
                item.Material,
                item.WeightKg,
                item.RequestedOn,
                item.Notes,
                item.Status))
            .FirstOrDefaultAsync(cancellationToken);

        if (pickup is null)
        {
            return Result.Failure<PickupResponse>(PickupErrors.NotFound(query.PickupId));
        }

        return pickup;
    }
}
