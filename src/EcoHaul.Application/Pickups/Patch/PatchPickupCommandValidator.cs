using FluentValidation;

namespace EcoHaul.Application.Pickups.Patch;

public sealed class PatchPickupCommandValidator : AbstractValidator<PatchPickupCommand>
{
    public PatchPickupCommandValidator()
    {
        RuleFor(command => command.PickupId).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();

        RuleFor(command => command.SiteName.Value)
            .NotEmpty()
            .MaximumLength(200)
            .OverridePropertyName(nameof(PatchPickupCommand.SiteName))
            .When(command => command.SiteName.IsSpecified);

        RuleFor(command => command.Material.Value)
            .NotNull()
            .WithMessage("Material is required.")
            .IsInEnum()
            .OverridePropertyName(nameof(PatchPickupCommand.Material))
            .When(command => command.Material.IsSpecified);

        RuleFor(command => command.WeightKg.Value)
            .NotNull()
            .WithMessage("Weight is required.")
            .GreaterThan(0)
            .OverridePropertyName(nameof(PatchPickupCommand.WeightKg))
            .When(command => command.WeightKg.IsSpecified);

        RuleFor(command => command.Notes.Value)
            .MaximumLength(1000)
            .OverridePropertyName(nameof(PatchPickupCommand.Notes))
            .When(command => command.Notes.IsSpecified && command.Notes.Value is not null);
    }
}
