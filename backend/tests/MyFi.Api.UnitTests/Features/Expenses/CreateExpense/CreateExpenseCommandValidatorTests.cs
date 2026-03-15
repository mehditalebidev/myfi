using MyFi.Api.Features.Expenses;

namespace MyFi.Api.UnitTests.Features.Expenses.CreateExpense;

public sealed class CreateExpenseCommandValidatorTests
{
    private readonly CreateExpenseCommandValidator _validator = new();

    [Fact]
    public void Validate_ReturnsErrors_WhenRequiredFieldsAreInvalid()
    {
        var result = _validator.Validate(new CreateExpenseCommand
        {
            Title = "   ",
            Amount = 0m,
            ExpenseDate = default
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.Amount));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.ExpenseDate));
    }

    [Fact]
    public void Validate_ReturnsErrors_WhenOptionalTextFieldsExceedLimits()
    {
        var result = _validator.Validate(new CreateExpenseCommand
        {
            Title = new string('a', 201),
            Amount = 84.50m,
            ExpenseDate = new DateOnly(2026, 3, 12),
            PaymentMethod = new string('b', 51),
            Note = new string('c', 501)
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.PaymentMethod));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateExpenseCommand.Note));
    }

    [Fact]
    public void Validate_Succeeds_WhenCommandIsValid()
    {
        var result = _validator.Validate(new CreateExpenseCommand
        {
            Title = "Supermarket",
            Amount = 84.50m,
            ExpenseDate = new DateOnly(2026, 3, 12)
        });

        Assert.True(result.IsValid);
    }
}
