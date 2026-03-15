using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Dashboard;
using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class DashboardEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private const string ApiBasePath = "/api/v1";

    private readonly IntegrationTestFixture _fixture;

    public DashboardEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DashboardSummary_RequiresAuthentication()
    {
        var client = _fixture.CreateClient();

        var response = await client.GetAsync($"{ApiBasePath}/dashboard/summary");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task DashboardSummary_ReturnsCurrentUsersAggregates()
    {
        var ownerClient = await CreateAuthenticatedClientAsync("dashboard-owner");
        var otherClient = await CreateAuthenticatedClientAsync("dashboard-other");

        var now = DateTime.UtcNow;
        var currentMonthDate = new DateOnly(now.Year, now.Month, 15);
        var previousMonthDate = DateOnly.FromDateTime(now.AddMonths(-1));

        var category = await CreateCategoryAsync(ownerClient, "Groceries");
        await CreateExpenseAsync(ownerClient, "Market", category.Id, 90m, currentMonthDate);
        await CreateExpenseAsync(ownerClient, "Coffee", null, 10m, new DateOnly(now.Year, now.Month, 16));
        await CreateExpenseAsync(ownerClient, "Old", null, 500m, previousMonthDate);

        await CreateSubscriptionAsync(ownerClient, "Netflix", null, 15m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 20), true);
        await CreateSubscriptionAsync(ownerClient, "Cloud", null, 24m, SubscriptionBillingCycles.Yearly, new DateOnly(now.Year, now.Month, 10), true);
        await CreateSubscriptionAsync(ownerClient, "Inactive", null, 80m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 8), false);

        await CreateExpenseAsync(otherClient, "Foreign", null, 999m, currentMonthDate);
        await CreateSubscriptionAsync(otherClient, "Foreign sub", null, 999m, SubscriptionBillingCycles.Monthly, new DateOnly(now.Year, now.Month, 1), true);

        var response = await ownerClient.GetAsync($"{ApiBasePath}/dashboard/summary");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var summary = await response.Content.ReadFromJsonAsync<DashboardSummaryResponse>();

        Assert.NotNull(summary);
        Assert.Equal(now.Month, summary.Month);
        Assert.Equal(now.Year, summary.Year);
        Assert.Equal(100m, summary.MonthlySpend);
        Assert.Equal(17m, summary.MonthlySubscriptionCost);

        Assert.Equal(2, summary.SpendByCategory.Count);
        Assert.Equal("Groceries", summary.SpendByCategory[0].CategoryName);
        Assert.Equal(90m, summary.SpendByCategory[0].Amount);
        Assert.Equal("Uncategorized", summary.SpendByCategory[1].CategoryName);
        Assert.Equal(10m, summary.SpendByCategory[1].Amount);

        Assert.Equal(new[] { "Coffee", "Market", "Old" }, summary.RecentExpenses.Select(expense => expense.Title).ToArray());
        Assert.Equal(new[] { "Cloud", "Netflix" }, summary.UpcomingRenewals.Select(subscription => subscription.Name).ToArray());
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync(string prefix)
    {
        var client = _fixture.CreateClient();
        var user = TestUserData.NewUser(prefix);

        var signupResponse = await client.PostAsJsonAsync($"{ApiBasePath}/auth/signup", new
        {
            email = user.Email,
            displayName = user.DisplayName,
            password = user.Password
        });

        Assert.Equal(HttpStatusCode.OK, signupResponse.StatusCode);

        var authResponse = await signupResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(authResponse);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.AccessToken);
        return client;
    }

    private static async Task<CategoryResponse> CreateCategoryAsync(HttpClient client, string name)
    {
        var response = await client.PostAsJsonAsync($"{ApiBasePath}/categories", new
        {
            name
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var category = await response.Content.ReadFromJsonAsync<CategoryResponse>();

        Assert.NotNull(category);
        return category;
    }

    private static async Task<ExpenseResponse> CreateExpenseAsync(
        HttpClient client,
        string title,
        Guid? categoryId,
        decimal amount,
        DateOnly expenseDate)
    {
        var response = await client.PostAsJsonAsync($"{ApiBasePath}/expenses", new
        {
            title,
            amount,
            expenseDate,
            categoryId
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var expense = await response.Content.ReadFromJsonAsync<ExpenseResponse>();

        Assert.NotNull(expense);
        return expense;
    }

    private static async Task<SubscriptionResponse> CreateSubscriptionAsync(
        HttpClient client,
        string name,
        Guid? categoryId,
        decimal amount,
        string billingCycle,
        DateOnly renewalDate,
        bool isActive)
    {
        var response = await client.PostAsJsonAsync($"{ApiBasePath}/subscriptions", new
        {
            name,
            amount,
            billingCycle,
            renewalDate,
            categoryId,
            isActive,
            reminderDaysBefore = 3
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();

        Assert.NotNull(subscription);
        return subscription;
    }
}
