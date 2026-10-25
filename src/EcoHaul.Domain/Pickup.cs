using System.ComponentModel.DataAnnotations;

namespace EcoHaul.Domain;

public sealed class Pickup
{
    public Guid Id { get; init; }

    public Guid CustomerId { get; init; }

    [Required]
    [MaxLength(200)]
    public string SiteName { get; set; } = string.Empty;

    public Material Material { get; set; }

    public decimal WeightKg { get; set; }

    public DateOnly RequestedOn { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }

    public PickupStatus Status { get; set; }

    public static Pickup Create(
        Guid customerId,
        string siteName,
        Material material,
        decimal weightKg,
        DateOnly requestedOn,
        string? notes)
    {
        var pickup = new Pickup
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            Status = PickupStatus.Draft
        };
        pickup.ApplyDetails(siteName, material, weightKg, requestedOn, notes);
        return pickup;
    }

    public void Replace(
        string siteName,
        Material material,
        decimal weightKg,
        DateOnly requestedOn,
        string? notes)
    {
        EnsureDraft();
        ApplyDetails(siteName, material, weightKg, requestedOn, notes);
    }

    public void Patch(
        string? siteName,
        Material? material,
        decimal? weightKg,
        DateOnly? requestedOn,
        string? notes,
        bool updateNotes)
    {
        EnsureDraft();
        ApplyDetails(
            siteName ?? SiteName,
            material ?? Material,
            weightKg ?? WeightKg,
            requestedOn ?? RequestedOn,
            updateNotes ? notes : Notes);
    }

    public void Schedule()
    {
        if (Status != PickupStatus.Draft)
        {
            throw new PickupConflictException("Only a draft pickup can be scheduled.");
        }

        Status = PickupStatus.Scheduled;
    }

    public void Collect()
    {
        if (Status != PickupStatus.Scheduled)
        {
            throw new PickupConflictException("Only a scheduled pickup can be collected.");
        }

        Status = PickupStatus.Collected;
    }

    private void EnsureDraft()
    {
        if (Status != PickupStatus.Draft)
        {
            throw new PickupConflictException("Only a draft pickup can be edited.");
        }
    }

    private void ApplyDetails(
        string siteName,
        Material material,
        decimal weightKg,
        DateOnly requestedOn,
        string? notes)
    {
        var trimmedSite = siteName.Trim();
        if (trimmedSite.Length == 0)
        {
            throw new PickupValidationException("Site name is required.");
        }

        if (weightKg <= 0)
        {
            throw new PickupValidationException("Weight must be greater than zero.");
        }

        SiteName = trimmedSite;
        Material = material;
        WeightKg = weightKg;
        RequestedOn = requestedOn;
        Notes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }
}
