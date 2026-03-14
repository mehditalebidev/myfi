using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Security;
using MyFi.Api.Features.Users;
using Testcontainers.PostgreSql;

namespace MyFi.Api.IntegrationTests;

public sealed class IntegrationTestFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _databaseContainer = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("myfi_integration_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    public TestWebApplicationFactory Factory { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();

        Factory = new TestWebApplicationFactory(_databaseContainer.GetConnectionString());

        _ = Factory.CreateClient();

        await SeedDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        if (Factory is not null)
        {
            await Factory.DisposeAsync();
        }

        await _databaseContainer.DisposeAsync();
    }

    public HttpClient CreateClient()
    {
        if (Factory is null)
        {
            throw new InvalidOperationException("The integration test fixture has not been initialized.");
        }

        return Factory.CreateClient();
    }

    private async Task SeedDatabaseAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyFiDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        var existingUser = await dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == IntegrationTestSeedData.SeededUserEmail);

        if (existingUser is not null)
        {
            return;
        }

        var user = User.Create(
            IntegrationTestSeedData.SeededUserEmail,
            IntegrationTestSeedData.SeededUserDisplayName);

        user.SetPasswordHash(passwordService.HashPassword(user, IntegrationTestSeedData.SeededUserPassword));

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }
}
