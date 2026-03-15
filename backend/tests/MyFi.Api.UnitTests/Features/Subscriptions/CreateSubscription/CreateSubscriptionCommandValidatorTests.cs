using FluentValidation.TestHelper;
using MyFi.Api.Features.Subscriptions;

namespace MyFi.Api.UnitTests.Features.Subscriptions.CreateSubscription;

public sealed class CreateSubscriptionCommandValidatorTests
{
    private readonly CreateSubscriptionCommandValidator _validator = new();

    [Fact]
    public void Validate_ReturnsErrors_ForInvalidFields()
    {
        var result = _validator.TestValidate(new CreateSubscriptionCommand
        {
            Name = "   ",
            Amount = 0m,
            BillingCycle = "weekly",
            RenewalDate = default,
            ReminderDaysBefore = -1
        });

        result.ShouldHaveValidationErrorFor(command => command.Name);
        result.ShouldHaveValidationErrorFor(command => command.Amount);
        result.ShouldHaveValidationErrorFor(command => command.BillingCycle);
        result.ShouldHaveValidationErrorFor(command => command.RenewalDate);
        result.ShouldHaveValidationErrorFor(command => command.ReminderDaysBefore);
    }

    [Fact]
    public void Validate_Succeeds_ForValidCommand()
    {
        var result = _validator.TestValidate(new CreateSubscriptionCommand
        {
            Name = "Netflix",
            Amount = 15.99m,
            BillingCycle = "monthly",
            RenewalDate = new DateOnly(2026, 3, 28),
            ReminderDaysBefore = 3
        });

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_ReturnsErrors_ForLengthAndRangeLimits()
    {
        var result = _validator.TestValidate(new CreateSubscriptionCommand
        {
            Name = new string('a', 151),
            Amount = 15.99m,
            BillingCycle = new string('b', 21),
            RenewalDate = new DateOnly(2026, 3, 28),
            ReminderDaysBefore = 366
        });

        result.ShouldHaveValidationErrorFor(command => command.Name);
        result.ShouldHaveValidationErrorFor(command => command.BillingCycle);
        result.ShouldHaveValidationErrorFor(command => command.ReminderDaysBefore);
    }
}
