using MyFi.Api.Features.Expenses;

namespace MyFi.Api.UnitTests.Features.Expenses.UpdateExpense;

public sealed class UpdateExpenseCommandValidatorTests
{
    private readonly UpdateExpenseCommandValidator _validator = new();

    [Fact]
    public void Validate_ReturnsErrors_WhenRequiredFieldsAreInvalid()
    {
        var result = _validator.Validate(new UpdateExpenseCommand
        {
            Title = string.Empty,
            Amount = -1m,
            ExpenseDate = default
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.Amount));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.ExpenseDate));
    }

    [Fact]
    public void Validate_Succeeds_WhenCommandIsValid()
    {
        var result = _validator.Validate(new UpdateExpenseCommand
        {
            Title = "Utilities bill",
            Amount = 120m,
            ExpenseDate = new DateOnly(2026, 3, 1)
        });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ReturnsErrors_WhenOptionalTextFieldsExceedLimits()
    {
        var result = _validator.Validate(new UpdateExpenseCommand
        {
            Title = new string('a', 201),
            Amount = 120m,
            ExpenseDate = new DateOnly(2026, 3, 1),
            PaymentMethod = new string('b', 51),
            Note = new string('c', 501)
        });

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.Title));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.PaymentMethod));
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateExpenseCommand.Note));
    }
}
