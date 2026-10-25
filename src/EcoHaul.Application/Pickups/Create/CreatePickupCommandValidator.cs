using FluentValidation;

namespace EcoHaul.Application.Pickups.Create;

public sealed class CreatePickupCommandValidator : AbstractValidator<CreatePickupCommand>
{
    public CreatePickupCommandValidator()
    {
        RuleFor(command => command.CustomerId).NotEmpty();
        RuleFor(command => command.SiteName).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Material).IsInEnum();
        RuleFor(command => command.WeightKg).GreaterThan(0);
        RuleFor(command => command.Notes).MaximumLength(1000);
    }
}
