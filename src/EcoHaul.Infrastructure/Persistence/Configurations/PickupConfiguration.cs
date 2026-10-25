using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoHaul.Infrastructure.Persistence.Configurations;

public sealed class PickupConfiguration : IEntityTypeConfiguration<Pickup>
{
    public void Configure(EntityTypeBuilder<Pickup> builder)
    {
        builder.HasKey(pickup => pickup.Id);
        builder.Property(pickup => pickup.Material)
            .HasConversion<string>()
            .HasMaxLength(32);
        builder.Property(pickup => pickup.Status)
            .HasConversion<string>()
            .HasMaxLength(32);
        builder.Property(pickup => pickup.WeightKg).HasPrecision(10, 2);
        builder.HasIndex(pickup => new { pickup.CustomerId, pickup.RequestedOn });
    }
}
