using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class ExpensesEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private const string ApiBasePath = "/api/v1";

    private readonly IntegrationTestFixture _fixture;

    public ExpensesEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Expenses_RequireAuthentication()
    {
        var client = _fixture.CreateClient();

        var response = await client.GetAsync($"{ApiBasePath}/expenses");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_And_Get_ReturnOwnedExpense()
    {
        var client = await CreateAuthenticatedClientAsync("expenses-create-get");
        var category = await CreateCategoryAsync(client, "Groceries");
        var createdExpense = await CreateExpenseAsync(client, "Supermarket", category.Id, 84.50m, new DateOnly(2026, 3, 12));

        var response = await client.GetAsync($"{ApiBasePath}/expenses/{createdExpense.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var expense = await response.Content.ReadFromJsonAsync<ExpenseResponse>();

        Assert.NotNull(expense);
        Assert.Equal("Supermarket", expense.Title);
        Assert.Equal(category.Id, expense.CategoryId);
        Assert.Equal("Groceries", expense.CategoryName);
    }

    [Fact]
    public async Task List_AppliesFiltersSortingAndPaging()
    {
        var client = await CreateAuthenticatedClientAsync("expenses-list");
        var category = await CreateCategoryAsync(client, "Food");

        await CreateExpenseAsync(client, "Dinner", category.Id, 40m, new DateOnly(2026, 3, 5), note: "restaurant");
        await CreateExpenseAsync(client, "Groceries", category.Id, 84.50m, new DateOnly(2026, 3, 12), note: "weekly shop");
        await CreateExpenseAsync(client, "Breakfast", null, 12m, new DateOnly(2026, 3, 2));

        var response = await client.GetAsync(
            $"{ApiBasePath}/expenses?page=1&pageSize=1&search=shop&categoryId={category.Id}&sortBy=amount&sortDir=desc");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var expenses = await response.Content.ReadFromJsonAsync<ExpenseListResponse>();

        Assert.NotNull(expenses);
        Assert.Equal(1, expenses.TotalCount);
        Assert.Single(expenses.Items);
        Assert.Equal("Groceries", expenses.Items[0].Title);
    }

    [Fact]
    public async Task Create_ReturnsNotFound_WhenCategoryBelongsToDifferentUser()
    {
        var ownerClient = await CreateAuthenticatedClientAsync("expenses-category-owner");
        var otherClient = await CreateAuthenticatedClientAsync("expenses-category-other");
        var category = await CreateCategoryAsync(ownerClient, "Travel");

        var response = await otherClient.PostAsJsonAsync($"{ApiBasePath}/expenses", new
        {
            title = "Flight",
            amount = 240.00m,
            expenseDate = new DateOnly(2026, 3, 21),
            categoryId = category.Id
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal("Category was not found.", problem.Title);
    }

    [Fact]
    public async Task Update_And_Delete_WorkForOwningUser()
    {
        var client = await CreateAuthenticatedClientAsync("expenses-update-delete");
        var category = await CreateCategoryAsync(client, "Bills");
        var createdExpense = await CreateExpenseAsync(client, "Electricity", null, 95m, new DateOnly(2026, 3, 8));

        var updateResponse = await client.PutAsJsonAsync($"{ApiBasePath}/expenses/{createdExpense.Id}", new
        {
            title = "Electricity bill",
            amount = 102.40m,
            expenseDate = new DateOnly(2026, 3, 9),
            categoryId = category.Id,
            paymentMethod = "bank transfer",
            note = "paid early",
            isRecurring = true
        });

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedExpense = await updateResponse.Content.ReadFromJsonAsync<ExpenseResponse>();

        Assert.NotNull(updatedExpense);
        Assert.Equal("Bills", updatedExpense.CategoryName);
        Assert.True(updatedExpense.IsRecurring);

        var deleteResponse = await client.DeleteAsync($"{ApiBasePath}/expenses/{createdExpense.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var getResponse = await client.GetAsync($"{ApiBasePath}/expenses/{createdExpense.Id}");

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

    private static async Task<ExpenseResponse> CreateExpenseAsync(
        HttpClient client,
        string title,
        Guid? categoryId,
        decimal amount,
        DateOnly expenseDate,
        string? paymentMethod = null,
        string? note = null,
        bool isRecurring = false)
    {
        var response = await client.PostAsJsonAsync($"{ApiBasePath}/expenses", new
        {
            title,
            amount,
            expenseDate,
            categoryId,
            paymentMethod,
            note,
            isRecurring
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var expense = await response.Content.ReadFromJsonAsync<ExpenseResponse>();

        Assert.NotNull(expense);
        return expense;
    }
}
