namespace MyFi.Api.Features.Expenses;

public sealed record ExpenseResponse(
    Guid Id,
    string Title,
    decimal Amount,
    DateOnly ExpenseDate,
    Guid? CategoryId,
    string? CategoryName,
    string? PaymentMethod,
    string? Note,
    bool IsRecurring,
    DateTime CreatedAt,
    DateTime UpdatedAt);
