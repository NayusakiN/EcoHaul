using System.ComponentModel.DataAnnotations;

namespace EcoHaul.Domain;

public sealed class LoadPhoto
{
    public Guid Id { get; init; }

    public Guid PickupId { get; init; }

    [Required]
    [MaxLength(256)]
    public string BlobName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;

    [MaxLength(64)]
    public string? ETag { get; set; }

    public long Size { get; set; }
}
