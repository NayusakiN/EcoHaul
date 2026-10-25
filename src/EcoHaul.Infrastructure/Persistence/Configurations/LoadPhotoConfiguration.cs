using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoHaul.Infrastructure.Persistence.Configurations;

public sealed class LoadPhotoConfiguration : IEntityTypeConfiguration<LoadPhoto>
{
    public void Configure(EntityTypeBuilder<LoadPhoto> builder)
    {
        builder.HasKey(photo => photo.Id);
        builder.HasIndex(photo => photo.BlobName).IsUnique();
        builder.HasIndex(photo => photo.PickupId);
        builder.HasOne<Pickup>()
            .WithMany()
            .HasForeignKey(photo => photo.PickupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
