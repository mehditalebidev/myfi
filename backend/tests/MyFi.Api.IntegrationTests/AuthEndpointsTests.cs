using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using MyFi.Api.Features.Users;

namespace MyFi.Api.IntegrationTests;

public sealed class AuthEndpointsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Signup_Login_And_GetMe_Work_EndToEnd()
    {
        var signupResponse = await _client.PostAsJsonAsync("/api/auth/signup", new
        {
            email = "mehdi@example.com",
            displayName = "Mehdi",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.OK, signupResponse.StatusCode);

        var signupPayload = await signupResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(signupPayload);
        Assert.False(string.IsNullOrWhiteSpace(signupPayload.AccessToken));
        Assert.Equal("mehdi@example.com", signupPayload.User.Email);
        Assert.Equal("Mehdi", signupPayload.User.DisplayName);

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "mehdi@example.com",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        Assert.NotNull(loginPayload);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginPayload.AccessToken);

        var meResponse = await _client.GetAsync("/api/users/me");

        Assert.Equal(HttpStatusCode.OK, meResponse.StatusCode);

        var mePayload = await meResponse.Content.ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(mePayload);
        Assert.Equal(signupPayload.User.Id, mePayload.Id);
        Assert.Equal("mehdi@example.com", mePayload.Email);
    }

    [Fact]
    public async Task Signup_ReturnsConflict_WhenEmailAlreadyExists()
    {
        await _client.PostAsJsonAsync("/api/auth/signup", new
        {
            email = "duplicate@example.com",
            displayName = "First User",
            password = "Password123!"
        });

        var duplicateResponse = await _client.PostAsJsonAsync("/api/auth/signup", new
        {
            email = "duplicate@example.com",
            displayName = "Second User",
            password = "Password123!"
        });

        Assert.Equal(HttpStatusCode.Conflict, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsWrong()
    {
        await _client.PostAsJsonAsync("/api/auth/signup", new
        {
            email = "login@example.com",
            displayName = "Login User",
            password = "Password123!"
        });

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            email = "login@example.com",
            password = "WrongPass123!"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, loginResponse.StatusCode);
    }
}
