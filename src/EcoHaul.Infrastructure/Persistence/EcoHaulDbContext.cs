using EcoHaul.Domain;
using EcoHaul.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Infrastructure.Persistence;

public sealed class EcoHaulDbContext : DbContext, IEcoHaulDbContext
{
    public EcoHaulDbContext(DbContextOptions<EcoHaulDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pickup> Pickups => Set<Pickup>();

    public DbSet<LoadPhoto> LoadPhotos => Set<LoadPhoto>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcoHaulDbContext).Assembly);
    }
}
