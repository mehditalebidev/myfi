using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class CategoriesEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private const string ApiBasePath = "/api/v1";

    private readonly IntegrationTestFixture _fixture;

    public CategoriesEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Categories_RequireAuthentication()
    {
        var client = _fixture.CreateClient();

        var response = await client.GetAsync($"{ApiBasePath}/categories");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Create_And_List_ReturnCurrentUsersCategoriesOrderedByName()
    {
        var primaryClient = await CreateAuthenticatedClientAsync("categories-primary");
        var secondaryClient = await CreateAuthenticatedClientAsync("categories-secondary");

        await CreateCategoryAsync(primaryClient, "Utilities");
        await CreateCategoryAsync(primaryClient, "Food");
        await CreateCategoryAsync(secondaryClient, "Ignored");

        var response = await primaryClient.GetAsync($"{ApiBasePath}/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>();

        Assert.NotNull(categories);
        Assert.Equal(new[] { "Food", "Utilities" }, categories.Select(category => category.Name).ToArray());
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenNameAlreadyExistsForUser()
    {
        var client = await CreateAuthenticatedClientAsync("categories-duplicate");

        await CreateCategoryAsync(client, "Groceries");

        var duplicateResponse = await client.PostAsJsonAsync($"{ApiBasePath}/categories", new
        {
            name = "groceries"
        });

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
        Assert.Equal("application/problem+json", duplicateResponse.Content.Headers.ContentType?.MediaType);

        var problem = await duplicateResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal("Category name is already in use.", problem.Title);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCategoryBelongsToDifferentUser()
    {
        var ownerClient = await CreateAuthenticatedClientAsync("categories-owner");
        var otherClient = await CreateAuthenticatedClientAsync("categories-other");
        var createdCategory = await CreateCategoryAsync(ownerClient, "Travel");

        var response = await otherClient.PutAsJsonAsync($"{ApiBasePath}/categories/{createdCategory.Id}", new
        {
            name = "Updated Travel"
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Update_And_Delete_WorkForOwningUser()
    {
        var client = await CreateAuthenticatedClientAsync("categories-owning-user");
        var createdCategory = await CreateCategoryAsync(client, "Bills");

        var updateResponse = await client.PutAsJsonAsync($"{ApiBasePath}/categories/{createdCategory.Id}", new
        {
            name = "Monthly Bills",
            color = "#22c55e",
            icon = "receipt"
        });

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        var updatedCategory = await updateResponse.Content.ReadFromJsonAsync<CategoryResponse>();

        Assert.NotNull(updatedCategory);
        Assert.Equal("Monthly Bills", updatedCategory.Name);
        Assert.Equal("#22c55e", updatedCategory.Color);

        var deleteResponse = await client.DeleteAsync($"{ApiBasePath}/categories/{createdCategory.Id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var listResponse = await client.GetAsync($"{ApiBasePath}/categories");
        var categories = await listResponse.Content.ReadFromJsonAsync<List<CategoryResponse>>();

        Assert.NotNull(categories);
        Assert.DoesNotContain(categories, category => category.Id == createdCategory.Id);
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
}
