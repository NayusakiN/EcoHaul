using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcoHaul.Infrastructure.Persistence;

public sealed class EcoHaulDbInitializer
{
    public static readonly Guid SampleCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static async Task ApplyAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<EcoHaulDbContext>();

        await db.Database.MigrateAsync(cancellationToken);

        if (await db.Pickups.AnyAsync(cancellationToken))
        {
            return;
        }

        var northOffice = Pickup.Create(
            SampleCustomerId, "North office", Material.Paper, 120, new DateOnly(2026, 10, 20), "cardboard");
        var southShop = Pickup.Create(
            SampleCustomerId, "South shop", Material.Plastic, 80, new DateOnly(2026, 10, 21), "bottles");
        var warehouse = Pickup.Create(
            SampleCustomerId, "Warehouse", Material.Glass, 200, new DateOnly(2026, 10, 18), "jars");
        var depot = Pickup.Create(
            SampleCustomerId, "Depot", Material.Electronics, 45, new DateOnly(2026, 10, 22), "cables");
        var plantGate = Pickup.Create(
            SampleCustomerId, "Plant gate", Material.Paper, 300, new DateOnly(2026, 10, 15), "archive");

        warehouse.Schedule();
        depot.Schedule();
        plantGate.Schedule();
        plantGate.Collect();

        db.Pickups.AddRange(northOffice, southShop, warehouse, depot, plantGate);
        await db.SaveChangesAsync(cancellationToken);
    }
}
