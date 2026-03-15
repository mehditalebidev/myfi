using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Subscriptions.DeleteSubscription;

public sealed class DeleteSubscriptionHandlerTests
{
    [Fact]
    public async Task Handle_DeletesOwnedSubscription()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var subscription = Subscription.Create(
            userId,
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
        var handler = new DeleteSubscriptionHandler(repository);

        var result = await handler.Handle(new DeleteSubscriptionCommand(subscription.Id, userId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(dbContext.Subscriptions);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenSubscriptionDoesNotExist()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var repository = new Repository(dbContext);
        var handler = new DeleteSubscriptionHandler(repository);

        var result = await handler.Handle(new DeleteSubscriptionCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("subscription_not_found", result.Error!.Code);
    }
}
