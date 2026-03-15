using FluentValidation;

namespace MyFi.Api.Features.Subscriptions;

public sealed class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
{
    public CreateSubscriptionCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Must(name => !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name is required.")
            .MaximumLength(150)
            .WithMessage("Name must be 150 characters or fewer.");

        RuleFor(command => command.Amount)
            .GreaterThan(0m)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(command => command.BillingCycle)
            .NotEmpty()
            .WithMessage("BillingCycle is required.")
            .Must(billingCycle => !string.IsNullOrWhiteSpace(billingCycle))
            .WithMessage("BillingCycle is required.")
            .MaximumLength(20)
            .WithMessage("BillingCycle must be 20 characters or fewer.")
            .Must(SubscriptionBillingCycles.IsAllowed)
            .WithMessage("BillingCycle must be one of: monthly, quarterly, yearly.");

        RuleFor(command => command.RenewalDate)
            .Must(renewalDate => renewalDate != default)
            .WithMessage("RenewalDate is required.");

        RuleFor(command => command.ReminderDaysBefore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ReminderDaysBefore must be greater than or equal to 0.")
            .LessThanOrEqualTo(365)
            .WithMessage("ReminderDaysBefore must be 365 or fewer.");
    }
}
