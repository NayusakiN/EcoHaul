using EcoHaul.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcoHaul.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(message => message.Id);
        builder.Property(message => message.Type)
            .HasConversion<string>()
            .HasMaxLength(100);
        builder.Property(message => message.Payload).HasColumnType("nvarchar(max)");
        builder.HasIndex(message => new { message.SentAt, message.CreatedAt })
            .HasFilter("[SentAt] IS NULL");
    }
}
