using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Subscriptions;

namespace MyFi.Api.Features.Dashboard;

public sealed record DashboardSummaryResponse(
    int Month,
    int Year,
    decimal MonthlySpend,
    decimal MonthlySubscriptionCost,
    IReadOnlyList<DashboardCategorySpendResponse> SpendByCategory,
    IReadOnlyList<ExpenseResponse> RecentExpenses,
    IReadOnlyList<SubscriptionResponse> UpcomingRenewals);
