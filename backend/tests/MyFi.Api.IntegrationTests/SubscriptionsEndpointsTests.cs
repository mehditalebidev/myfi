using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class SubscriptionsEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private const string ApiBasePath = "/api/v1";

    private readonly IntegrationTestFixture _fixture;

    public SubscriptionsEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Subscriptions_RequireAuthentication()
    {
        var client = _fixture.CreateClient();

        var response = await client.GetAsync($"{ApiBasePath}/subscriptions");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_And_Get_ReturnOwnedSubscription()
    {
        var client = await CreateAuthenticatedClientAsync("subscriptions-create-get");
        var category = await CreateCategoryAsync(client, "Entertainment");
        var createdSubscription = await CreateSubscriptionAsync(client, "Netflix", category.Id, 15.99m, new DateOnly(2026, 3, 28));

        var response = await client.GetAsync($"{ApiBasePath}/subscriptions/{createdSubscription.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();

        Assert.NotNull(subscription);
        Assert.Equal("Netflix", subscription.Name);
        Assert.Equal(category.Id, subscription.CategoryId);
        Assert.Equal("Entertainment", subscription.CategoryName);
    }

    [Fact]
    public async Task List_AppliesFiltersSortingAndPaging()
    {
        var client = await CreateAuthenticatedClientAsync("subscriptions-list");
        var category = await CreateCategoryAsync(client, "Bills");

        await CreateSubscriptionAsync(client, "Netflix", category.Id, 15.99m, new DateOnly(2026, 3, 28), isActive: true);
        await CreateSubscriptionAsync(client, "Gym", null, 29.99m, new DateOnly(2026, 3, 20), isActive: false);
        await CreateSubscriptionAsync(client, "Spotify", category.Id, 12.99m, new DateOnly(2026, 3, 15), isActive: true);

        var response = await client.GetAsync(
            $"{ApiBasePath}/subscriptions?page=1&pageSize=1&search=net&isActive=true&sortBy=amount&sortDir=desc");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscriptions = await response.Content.ReadFromJsonAsync<SubscriptionListResponse>();

        Assert.NotNull(subscriptions);
        Assert.Equal(1, subscriptions.TotalCount);
        Assert.Single(subscriptions.Items);
        Assert.Equal("Netflix", subscriptions.Items[0].Name);
    }

    [Fact]
    public async Task Create_ReturnsNotFound_WhenCategoryBelongsToDifferentUser()
    {
        var ownerClient = await CreateAuthenticatedClientAsync("subscriptions-category-owner");
        var otherClient = await CreateAuthenticatedClientAsync("subscriptions-category-other");
        var category = await CreateCategoryAsync(ownerClient, "Bills");

        var response = await otherClient.PostAsJsonAsync($"{ApiBasePath}/subscriptions", new
        {
            name = "Netflix",
            amount = 15.99m,
            billingCycle = "monthly",
            renewalDate = new DateOnly(2026, 3, 28),
            categoryId = category.Id,
            isActive = true,
            reminderDaysBefore = 3
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal("Category was not found.", problem.Title);
    }

    [Fact]
    public async Task Update_And_Delete_WorkForOwningUser()
    {
        var client = await CreateAuthenticatedClientAsync("subscriptions-update-delete");
        var category = await CreateCategoryAsync(client, "Entertainment");
        var createdSubscription = await CreateSubscriptionAsync(client, "Netflix", null, 15.99m, new DateOnly(2026, 3, 28));

        var updateResponse = await client.PutAsJsonAsync($"{ApiBasePath}/subscriptions/{createdSubscription.Id}", new
        {
            name = "Netflix Premium",
            amount = 21.99m,
            billingCycle = "yearly",
            renewalDate = new DateOnly(2026, 5, 1),
            categoryId = category.Id,
            isActive = false,
            reminderDaysBefore = 10
        });

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedSubscription = await updateResponse.Content.ReadFromJsonAsync<SubscriptionResponse>();

        Assert.NotNull(updatedSubscription);
        Assert.Equal("Entertainment", updatedSubscription.CategoryName);
        Assert.False(updatedSubscription.IsActive);
        Assert.Equal(10, updatedSubscription.ReminderDaysBefore);

        var deleteResponse = await client.DeleteAsync($"{ApiBasePath}/subscriptions/{createdSubscription.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync($"{ApiBasePath}/subscriptions/{createdSubscription.Id}");

        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
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

    private static async Task<SubscriptionResponse> CreateSubscriptionAsync(
        HttpClient client,
        string name,
        Guid? categoryId,
        decimal amount,
        DateOnly renewalDate,
        string billingCycle = "monthly",
        bool isActive = true,
        int reminderDaysBefore = 3)
    {
        var response = await client.PostAsJsonAsync($"{ApiBasePath}/subscriptions", new
        {
            name,
            amount,
            billingCycle,
            renewalDate,
            categoryId,
            isActive,
            reminderDaysBefore
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var subscription = await response.Content.ReadFromJsonAsync<SubscriptionResponse>();

        Assert.NotNull(subscription);
        return subscription;
    }
}
