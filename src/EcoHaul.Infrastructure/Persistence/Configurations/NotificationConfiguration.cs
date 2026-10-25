using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoHaul.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(notification => notification.Id);
        builder.Property(notification => notification.State)
            .HasConversion<string>()
            .HasMaxLength(32);
        builder.HasIndex(notification => notification.PickupId);
        builder.HasOne<Pickup>()
            .WithMany()
            .HasForeignKey(notification => notification.PickupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
