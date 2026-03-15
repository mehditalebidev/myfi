using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Dashboard;
using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Dashboard.GetSummary;

public sealed class GetDashboardSummaryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsUserScopedSummary_WithExpectedAggregations()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var otherUserId = Guid.NewGuid();

        var now = DateTime.UtcNow;
        var latestExpenseDate = new DateOnly(now.Year, now.Month, 15);
        var currentMonthExpenseDate = new DateOnly(now.Year, now.Month, 9);
        var previousMonthExpenseDate = DateOnly.FromDateTime(now.AddMonths(-1));

        var groceries = Category.Create(userId, "Groceries", null, null);
        dbContext.Categories.Add(groceries);

        dbContext.Expenses.AddRange(
            Expense.Create(userId, groceries.Id, "Groceries run", 120m, latestExpenseDate, null, null, false),
            Expense.Create(userId, null, "Taxi", 30m, currentMonthExpenseDate, null, null, false),
            Expense.Create(userId, null, "Recent 1", 9m, new DateOnly(now.Year, now.Month, 14), null, null, false),
            Expense.Create(userId, null, "Recent 2", 8m, new DateOnly(now.Year, now.Month, 13), null, null, false),
            Expense.Create(userId, null, "Recent 3", 7m, new DateOnly(now.Year, now.Month, 12), null, null, false),
            Expense.Create(userId, null, "Recent 4", 6m, new DateOnly(now.Year, now.Month, 11), null, null, false),
            Expense.Create(userId, null, "Recent 5", 5m, new DateOnly(now.Year, now.Month, 10), null, null, false),
            Expense.Create(userId, null, "Previous month", 999m, previousMonthExpenseDate, null, null, false),
            Expense.Create(otherUserId, groceries.Id, "Foreign", 888m, currentMonthExpenseDate, null, null, false));

        dbContext.Subscriptions.AddRange(
            Subscription.Create(userId, groceries.Id, "Music", 12m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 20), true, 3),
            Subscription.Create(userId, null, "Cloud", 30m, SubscriptionBillingCycles.Quarterly, new DateOnly(now.Year, now.Month, 10), true, 3),
            Subscription.Create(userId, null, "Domain", 120m, SubscriptionBillingCycles.Yearly, new DateOnly(now.Year, now.Month, 25), true, 10),
            Subscription.Create(userId, null, "Inactive", 999m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 1), false, 1),
            Subscription.Create(otherUserId, null, "Foreign subscription", 777m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 5), true, 2));

        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new GetDashboardSummaryHandler(repository);

        var result = await handler.Handle(new GetDashboardSummaryQuery { UserId = userId }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(now.Month, result.Value.Month);
        Assert.Equal(now.Year, result.Value.Year);
        Assert.Equal(185m, result.Value.MonthlySpend);
        Assert.Equal(32m, result.Value.MonthlySubscriptionCost);

        Assert.Equal(2, result.Value.SpendByCategory.Count);
        Assert.Equal("Groceries", result.Value.SpendByCategory[0].CategoryName);
        Assert.Equal(120m, result.Value.SpendByCategory[0].Amount);
        Assert.Equal("Uncategorized", result.Value.SpendByCategory[1].CategoryName);
        Assert.Equal(65m, result.Value.SpendByCategory[1].Amount);

        Assert.Equal(5, result.Value.RecentExpenses.Count);
        Assert.Equal(new[] { "Groceries run", "Recent 1", "Recent 2", "Recent 3", "Recent 4" },
            result.Value.RecentExpenses.Select(expense => expense.Title).ToArray());

        Assert.Equal(3, result.Value.UpcomingRenewals.Count);
        Assert.Equal(new[] { "Cloud", "Music", "Domain" }, result.Value.UpcomingRenewals.Select(subscription => subscription.Name).ToArray());
    }

    [Fact]
    public async Task Handle_ReturnsEmptySummary_WhenUserHasNoData()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();

        var repository = new Repository(dbContext);
        var handler = new GetDashboardSummaryHandler(repository);

        var result = await handler.Handle(new GetDashboardSummaryQuery { UserId = Guid.NewGuid() }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.MonthlySpend);
        Assert.Equal(0m, result.Value.MonthlySubscriptionCost);
        Assert.Empty(result.Value.SpendByCategory);
        Assert.Empty(result.Value.RecentExpenses);
        Assert.Empty(result.Value.UpcomingRenewals);
    }
}
