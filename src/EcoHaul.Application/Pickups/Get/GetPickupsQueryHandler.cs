using EcoHaul.Application.Abstractions.Messaging;
using EcoHaul.Domain;
using EcoHaul.Application.Persistence;
using EcoHaul.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Application.Pickups.Get;

internal sealed class GetPickupsQueryHandler(IEcoHaulDbContext db)
    : IQueryHandler<GetPickupsQuery, PickupPage>
{
    public async Task<Result<PickupPage>> Handle(GetPickupsQuery query, CancellationToken cancellationToken)
    {
        ValidationError? invalid = Validate(query);
        if (invalid is not null)
        {
            return Result.Failure<PickupPage>(invalid);
        }

        IQueryable<Pickup> pickups = db.Pickups
            .AsNoTracking()
            .Where(pickup => pickup.CustomerId == query.CustomerId);

        if (query.Status is not null)
        {
            pickups = pickups.Where(pickup => pickup.Status == query.Status);
        }

        if (query.RequestedFrom is not null)
        {
            pickups = pickups.Where(pickup => pickup.RequestedOn >= query.RequestedFrom);
        }

        if (query.RequestedTo is not null)
        {
            pickups = pickups.Where(pickup => pickup.RequestedOn <= query.RequestedTo);
        }

        int total = await pickups.CountAsync(cancellationToken);
        List<PickupResponse> items = await pickups
            .OrderBy(pickup => pickup.RequestedOn)
            .ThenBy(pickup => pickup.Id)
            .Skip(query.Offset)
            .Take(query.Limit)
            .Select(pickup => new PickupResponse(
                pickup.Id,
                pickup.CustomerId,
                pickup.SiteName,
                pickup.Material,
                pickup.WeightKg,
                pickup.RequestedOn,
                pickup.Notes,
                pickup.Status))
            .ToListAsync(cancellationToken);

        return new PickupPage(items, query.Offset, query.Limit, total);
    }

    private static ValidationError? Validate(GetPickupsQuery query)
    {
        List<ValidationErrorDetail> errors = [];

        if (query.Offset < 0)
        {
            errors.Add(new ValidationErrorDetail(
                nameof(GetPickupsQuery.Offset),
                "Invalid",
                "Offset must be zero or greater."));
        }

        if (query.Limit < 1 || query.Limit > PickupPage.MaxLimit)
        {
            errors.Add(new ValidationErrorDetail(
                nameof(GetPickupsQuery.Limit),
                "Invalid",
                $"Limit must be between 1 and {PickupPage.MaxLimit}."));
        }

        if (query.RequestedFrom > query.RequestedTo)
        {
            errors.Add(new ValidationErrorDetail(
                nameof(GetPickupsQuery.RequestedFrom),
                "Invalid",
                "Requested from must be on or before requested to."));
        }

        return errors.Count == 0 ? null : new ValidationError(errors.ToArray());
    }
}
