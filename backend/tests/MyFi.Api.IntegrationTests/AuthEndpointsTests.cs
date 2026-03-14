using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class AuthEndpointsTests : IClassFixture<IntegrationTestFixture>
{
    private readonly IntegrationTestFixture _fixture;

    public AuthEndpointsTests(IntegrationTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Signup_ReturnsAccessToken_ForNewUser()
    {
        var client = _fixture.CreateClient();
        var email = $"signup-{Guid.NewGuid():N}@example.com";

        var signupResponse = await client.PostAsJsonAsync("/api/auth/signup", new
        {
            email,
            displayName = "Signup User",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.OK, signupResponse.StatusCode);

        var signupPayload = await signupResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(signupPayload);
        Assert.False(string.IsNullOrWhiteSpace(signupPayload.AccessToken));
        Assert.Equal(email, signupPayload.User.Email);
        Assert.Equal("Signup User", signupPayload.User.DisplayName);
    }

    [Fact]
    public async Task Login_And_GetMe_Work_WithSeededUser()
    {
        var client = _fixture.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = IntegrationTestSeedData.SeededUserEmail,
            password = IntegrationTestSeedData.SeededUserPassword
        });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(loginPayload);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginPayload.AccessToken);

        var meResponse = await client.GetAsync("/api/users/me");

        Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

        var mePayload = await meResponse.Content.ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(mePayload);
        Assert.Equal(IntegrationTestSeedData.SeededUserEmail, mePayload.Email);
        Assert.Equal(IntegrationTestSeedData.SeededUserDisplayName, mePayload.DisplayName);
    }

    [Fact]
    public async Task Signup_ReturnsConflict_WhenEmailAlreadyExists()
    {
        var client = _fixture.CreateClient();

        var duplicateResponse = await client.PostAsJsonAsync("/api/auth/signup", new
        {
            email = IntegrationTestSeedData.SeededUserEmail,
            displayName = "Second User",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);

        Assert.Equal("application/problem+json", duplicateResponse.Content.Headers.ContentType?.MediaType);

        var problem = await duplicateResponse.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal("Email is already in use.", problem.Title);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        var client = _fixture.CreateClient();

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email = IntegrationTestSeedData.SeededUserEmail,
            password = "WrongPass123!"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);

        Assert.Equal("application/problem+json", loginResponse.Content.Headers.ContentType?.MediaType);
    }
}
