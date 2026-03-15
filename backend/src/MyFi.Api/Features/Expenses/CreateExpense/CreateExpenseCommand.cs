using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Expenses;

public sealed record CreateExpenseCommand : IRequest<Result<ExpenseResponse>>
{
    [JsonIgnore]
    public Guid UserId { get; init; }

    public Guid? CategoryId { get; init; }

    public string Title { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public DateOnly ExpenseDate { get; init; }

    public string? PaymentMethod { get; init; }

    public string? Note { get; init; }

    public bool IsRecurring { get; init; }
}
