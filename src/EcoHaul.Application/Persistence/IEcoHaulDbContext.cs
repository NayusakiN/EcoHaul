using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;

namespace EcoHaul.Application.Persistence;

public interface IEcoHaulDbContext
{
    DbSet<Pickup> Pickups { get; }

    DbSet<LoadPhoto> LoadPhotos { get; }

    DbSet<Notification> Notifications { get; }

    DbSet<OutboxMessage> OutboxMessages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
