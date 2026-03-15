using FluentValidation;

namespace MyFi.Api.Features.Expenses;

public sealed class UpdateExpenseCommandValidator : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Must(title => !string.IsNullOrWhiteSpace(title))
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .WithMessage("Title must be 200 characters or fewer.");

        RuleFor(command => command.PaymentMethod)
            .MaximumLength(50)
            .WithMessage("PaymentMethod must be 50 characters or fewer.");

        RuleFor(command => command.Note)
            .MaximumLength(500)
            .WithMessage("Note must be 500 characters or fewer.");

        RuleFor(command => command.Amount)
            .GreaterThan(0m)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(command => command.ExpenseDate)
            .Must(expenseDate => expenseDate != default)
            .WithMessage("ExpenseDate is required.");
    }
}
