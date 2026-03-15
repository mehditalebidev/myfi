namespace MyFi.Api.Features.Dashboard;

public sealed record DashboardCategorySpendResponse(
    Guid? CategoryId,
    string CategoryName,
    decimal Amount);
