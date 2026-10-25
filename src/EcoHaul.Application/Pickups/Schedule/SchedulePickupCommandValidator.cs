using FluentValidation;

namespace EcoHaul.Application.Pickups.Schedule;

public sealed class SchedulePickupCommandValidator : AbstractValidator<SchedulePickupCommand>
{
    public SchedulePickupCommandValidator()
    {
        RuleFor(command => command.PickupId).NotEmpty();
        RuleFor(command => command.CustomerId).NotEmpty();
    }
}
