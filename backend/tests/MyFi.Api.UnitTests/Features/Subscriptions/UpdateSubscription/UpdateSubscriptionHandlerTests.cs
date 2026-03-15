using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Subscriptions.UpdateSubscription;

public sealed class UpdateSubscriptionHandlerTests
{
    [Fact]
    public async Task Handle_UpdatesOwnedSubscription()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Bills", null, null);
        var subscription = Subscription.Create(
            userId,
            null,
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
        var handler = new UpdateSubscriptionHandler(repository);

        var result = await handler.Handle(new UpdateSubscriptionCommand
        {
            Id = subscription.Id,
            UserId = userId,
            CategoryId = category.Id,
            Name = "  Netflix Premium  ",
            Amount = 21.99m,
            BillingCycle = "YEARLY",
            RenewalDate = new DateOnly(2026, 5, 1),
            IsActive = false,
            ReminderDaysBefore = 14
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Netflix Premium", result.Value.Name);
        Assert.Equal("yearly", result.Value.BillingCycle);
        Assert.False(result.Value.IsActive);
        Assert.Equal(14, result.Value.ReminderDaysBefore);
        Assert.Equal("Bills", result.Value.CategoryName);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenSubscriptionIsMissingOrForeign()
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
        var handler = new UpdateSubscriptionHandler(repository);

        var result = await handler.Handle(new UpdateSubscriptionCommand
        {
            Id = subscription.Id,
            UserId = Guid.NewGuid(),
            Name = "Netflix",
            Amount = 15.99m,
            BillingCycle = "monthly",
            RenewalDate = new DateOnly(2026, 3, 28),
            ReminderDaysBefore = 3
        }, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("subscription_not_found", result.Error!.Code);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenCategoryBelongsToDifferentUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var foreignCategory = Category.Create(Guid.NewGuid(), "Bills", null, null);
        var subscription = Subscription.Create(
            userId,
            null,
            "Netflix",
            15.99m,
            "monthly",
            new DateOnly(2026, 3, 28),
            true,
            3);

        dbContext.Categories.Add(foreignCategory);
        dbContext.Subscriptions.Add(subscription);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new UpdateSubscriptionHandler(repository);

        var result = await handler.Handle(new UpdateSubscriptionCommand
        {
            Id = subscription.Id,
            UserId = userId,
            CategoryId = foreignCategory.Id,
            Name = "Netflix",
            Amount = 15.99m,
            BillingCycle = "monthly",
            RenewalDate = new DateOnly(2026, 3, 28),
            ReminderDaysBefore = 3
        }, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("category_not_found", result.Error!.Code);
    }
}
