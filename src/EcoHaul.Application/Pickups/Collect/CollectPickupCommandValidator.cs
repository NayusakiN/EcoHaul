using FluentValidation;

namespace EcoHaul.Application.Pickups.Collect;

public sealed class CollectPickupCommandValidator : AbstractValidator<CollectPickupCommand>
{
    public CollectPickupCommandValidator()
    {
        RuleFor(command => command.PickupId).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();
    }
}
