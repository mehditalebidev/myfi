using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Subscriptions.GetSubscription;

public sealed class GetSubscriptionHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsOwnedSubscription()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Entertainment", null, null);
        var subscription = Subscription.Create(
            userId,
            category.Id,
            "Netflix",
            15.99m,
            "monthly",
            new DateOnly(2026, 3, 28),
            true,
            3);

        dbContext.Categories.Add(category);
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new GetSubscriptionHandler(repository);

        var result = await handler.Handle(new GetSubscriptionQuery(subscription.Id, userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Entertainment", result.Value.CategoryName);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_ForForeignSubscription()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var subscription = Subscription.Create(
            Guid.NewGuid(),
            null,
            "Netflix",
            15.99m,
            "monthly",
            new DateOnly(2026, 3, 28),
            true,
            3);
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new GetSubscriptionHandler(repository);

        var result = await handler.Handle(new GetSubscriptionQuery(subscription.Id, Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("subscription_not_found", result.Error!.Code);
    }
}
