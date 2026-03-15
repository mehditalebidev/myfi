using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Expenses;

public sealed record ListExpensesQuery : IRequest<Result<ExpenseListResponse>>
{
    [JsonIgnore]
    public Guid UserId { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? Search { get; init; }

    public Guid? CategoryId { get; init; }

    public DateOnly? DateFrom { get; init; }

    public DateOnly? DateTo { get; init; }

    public string? SortBy { get; init; }

    public string? SortDir { get; init; }
}
