using EcoHaul.Domain;
using EcoHaul.Application.Pickups;
using EcoHaul.Application.Pickups.Collect;
using EcoHaul.Application.Pickups.Create;
using EcoHaul.Application.Pickups.Get;
using EcoHaul.Application.Pickups.GetById;
using EcoHaul.Application.Pickups.Patch;
using EcoHaul.Application.Pickups.Replace;
using EcoHaul.Application.Pickups.Schedule;
using EcoHaul.SharedKernel;
using EcoHaul.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Api.Tests;

public class PickupTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly DateOnly RequestedOn = new(2026, 10, 20);

    [Fact]
    public async Task Create_starts_as_draft()
    {
        await using var db = CreateDb();
        var handler = new CreatePickupCommandHandler(db);

        Result<PickupResponse> result = await handler.Handle(SampleCreate(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(PickupStatus.Draft, result.Value.Status);
        Assert.Equal("North office", result.Value.SiteName);
    }

    [Fact]
    public async Task Patch_changes_only_the_sent_field()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);

        Result patched = await new PatchPickupCommandHandler(db).Handle(
            new PatchPickupCommand(
                created.Value.Id,
                CustomerId,
                default,
                default,
                Optional<decimal?>.Specified(80),
                default,
                default),
            CancellationToken.None);

        var pickup = await db.Pickups.SingleAsync(pickup => pickup.Id == created.Value.Id);
        Assert.True(patched.IsSuccess);
        Assert.Equal(80, pickup.WeightKg);
        Assert.Equal("North office", pickup.SiteName);
        Assert.Equal(Material.Paper, pickup.Material);
        Assert.Equal("cardboard", pickup.Notes);
    }

    [Fact]
    public async Task Patch_clears_notes_when_null_is_sent()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);

        Result patched = await new PatchPickupCommandHandler(db).Handle(
            new PatchPickupCommand(
                created.Value.Id,
                CustomerId,
                default,
                default,
                default,
                default,
                Optional<string?>.Specified(null)),
            CancellationToken.None);

        var pickup = await db.Pickups.SingleAsync(pickup => pickup.Id == created.Value.Id);
        Assert.True(patched.IsSuccess);
        Assert.Null(pickup.Notes);
        Assert.Equal(25, pickup.WeightKg);
    }

    [Fact]
    public async Task Put_replaces_the_draft()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);

        Result replaced = await new ReplacePickupCommandHandler(db).Handle(
            new ReplacePickupCommand(created.Value.Id, CustomerId, "South shop", Material.Glass, 40, RequestedOn, null),
            CancellationToken.None);

        var pickup = await db.Pickups.SingleAsync(pickup => pickup.Id == created.Value.Id);
        Assert.True(replaced.IsSuccess);
        Assert.Equal("South shop", pickup.SiteName);
        Assert.Equal(Material.Glass, pickup.Material);
        Assert.Null(pickup.Notes);
    }

    [Fact]
    public async Task Schedule_then_collect()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);

        await new SchedulePickupCommandHandler(db).Handle(
            new SchedulePickupCommand(created.Value.Id, CustomerId),
            CancellationToken.None);
        await new CollectPickupCommandHandler(db).Handle(
            new CollectPickupCommand(created.Value.Id, CustomerId),
            CancellationToken.None);

        var pickup = await db.Pickups.SingleAsync(pickup => pickup.Id == created.Value.Id);
        Assert.Equal(PickupStatus.Collected, pickup.Status);
    }

    [Fact]
    public async Task Edit_after_schedule_conflicts()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);
        await new SchedulePickupCommandHandler(db).Handle(
            new SchedulePickupCommand(created.Value.Id, CustomerId),
            CancellationToken.None);

        Result patched = await new PatchPickupCommandHandler(db).Handle(
            new PatchPickupCommand(
                created.Value.Id,
                CustomerId,
                default,
                default,
                Optional<decimal?>.Specified(10),
                default,
                default),
            CancellationToken.None);

        Assert.True(patched.IsFailure);
        Assert.Equal(ErrorType.Conflict, patched.Error.Type);
    }

    [Fact]
    public async Task Another_customer_does_not_see_the_pickup()
    {
        await using var db = CreateDb();
        Result<PickupResponse> created = await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);
        var otherCustomerId = Guid.NewGuid();

        Result<PickupPage> listed = await new GetPickupsQueryHandler(db).Handle(
            new GetPickupsQuery(otherCustomerId),
            CancellationToken.None);
        Result<PickupResponse> fetched = await new GetPickupQueryHandler(db).Handle(
            new GetPickupQuery(created.Value.Id, otherCustomerId),
            CancellationToken.None);
        var found = await db.Pickups.FirstOrDefaultAsync(
            pickup => pickup.Id == created.Value.Id && pickup.CustomerId == otherCustomerId);

        Assert.Empty(listed.Value.Items);
        Assert.Equal(0, listed.Value.Total);
        Assert.Equal(ErrorType.NotFound, fetched.Error.Type);
        Assert.Null(found);
    }

    [Fact]
    public async Task List_filters_and_pages()
    {
        await using var db = CreateDb();
        await new CreatePickupCommandHandler(db).Handle(SampleCreate(), CancellationToken.None);
        await new CreatePickupCommandHandler(db).Handle(
            SampleCreate() with { SiteName = "South shop", RequestedOn = new DateOnly(2026, 11, 1) },
            CancellationToken.None);

        Result<PickupPage> page = await new GetPickupsQueryHandler(db).Handle(
            new GetPickupsQuery(CustomerId, Limit: 1),
            CancellationToken.None);
        Result<PickupPage> filtered = await new GetPickupsQueryHandler(db).Handle(
            new GetPickupsQuery(CustomerId, RequestedFrom: new DateOnly(2026, 11, 1)),
            CancellationToken.None);
        Result<PickupPage> invalid = await new GetPickupsQueryHandler(db).Handle(
            new GetPickupsQuery(CustomerId, Limit: 0),
            CancellationToken.None);

        Assert.Single(page.Value.Items);
        Assert.Equal(2, page.Value.Total);
        Assert.Equal("North office", page.Value.Items[0].SiteName);
        Assert.Single(filtered.Value.Items);
        Assert.Equal("South shop", filtered.Value.Items[0].SiteName);
        Assert.Equal(ErrorType.Validation, invalid.Error.Type);
    }

    [Fact]
    public async Task Missing_site_name_is_rejected()
    {
        await using var db = CreateDb();
        var handler = new CreatePickupCommandHandler(db);

        Result<PickupResponse> result = await handler.Handle(
            SampleCreate() with { SiteName = "  " },
            CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Problem, result.Error.Type);
    }

    private static EcoHaulDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EcoHaulDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new EcoHaulDbContext(options);
    }

    private static CreatePickupCommand SampleCreate() =>
        new(CustomerId, "North office", Material.Paper, 25, RequestedOn, "cardboard");
}
