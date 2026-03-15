namespace MyFi.Api.Features.Expenses;

public sealed record ExpenseListResponse(
    IReadOnlyList<ExpenseResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
