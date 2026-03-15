using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Subscriptions.ListSubscriptions;

public sealed class ListSubscriptionsHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsCurrentUsersSubscriptions_WithFilteringSortingAndPaging()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var bills = Category.Create(userId, "Bills", null, null);
        dbContext.Categories.AddRange(bills, Category.Create(Guid.NewGuid(), "Ignored", null, null));
        dbContext.Subscriptions.AddRange(
            Subscription.Create(userId, bills.Id, "Netflix", 15.99m, "monthly", new DateOnly(2026, 3, 28), true, 3),
            Subscription.Create(userId, bills.Id, "Spotify", 12.99m, "monthly", new DateOnly(2026, 3, 15), true, 5),
            Subscription.Create(userId, null, "Archive", 50m, "yearly", new DateOnly(2026, 7, 1), false, 30),
            Subscription.Create(Guid.NewGuid(), bills.Id, "Foreign", 999m, "monthly", new DateOnly(2026, 3, 10), true, 1));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new ListSubscriptionsHandler(repository);

        var result = await handler.Handle(new ListSubscriptionsQuery
        {
            UserId = userId,
            Search = "net",
            IsActive = true,
            SortBy = "amount",
            SortDir = "desc",
            Page = 1,
            PageSize = 1
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.TotalCount);
        Assert.Single(result.Value.Items);
        Assert.Equal("Netflix", result.Value.Items[0].Name);
        Assert.Equal("Bills", result.Value.Items[0].CategoryName);
    }

    [Fact]
    public async Task Handle_DefaultsToRenewalDateAscending()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        dbContext.Subscriptions.AddRange(
            Subscription.Create(userId, null, "Later", 10m, "monthly", new DateOnly(2026, 3, 20), true, 3),
            Subscription.Create(userId, null, "Sooner", 10m, "monthly", new DateOnly(2026, 3, 10), true, 3));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new ListSubscriptionsHandler(repository);

        var result = await handler.Handle(new ListSubscriptionsQuery { UserId = userId }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { "Sooner", "Later" }, result.Value.Items.Select(item => item.Name).ToArray());
    }
}
