using MyFi.Api.Common.Persistence;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Subscriptions;
using MyFi.Api.UnitTests.Support;

namespace MyFi.Api.UnitTests.Features.Subscriptions.CreateSubscription;

public sealed class CreateSubscriptionHandlerTests
{
    [Fact]
    public async Task Handle_CreatesSubscription_WhenCategoryBelongsToUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        var userId = Guid.NewGuid();
        var category = Category.Create(userId, "Entertainment", null, null);
        dbContext.Categories.Add(category);
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateSubscriptionHandler(repository);

        var result = await handler.Handle(new CreateSubscriptionCommand
        {
            UserId = userId,
            CategoryId = category.Id,
            Name = "  Netflix  ",
            Amount = 15.99m,
            BillingCycle = "MONTHLY",
            RenewalDate = new DateOnly(2026, 3, 28),
            IsActive = true,
            ReminderDaysBefore = 3
        }, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Netflix", result.Value.Name);
        Assert.Equal("monthly", result.Value.BillingCycle);
        Assert.Equal("Entertainment", result.Value.CategoryName);
    }

    [Fact]
    public async Task Handle_ReturnsNotFound_WhenCategoryBelongsToDifferentUser()
    {
        await using var dbContext = InMemoryDbContextFactory.Create();
        dbContext.Categories.Add(Category.Create(Guid.NewGuid(), "Entertainment", null, null));
        await dbContext.SaveChangesAsync();

        var repository = new Repository(dbContext);
        var handler = new CreateSubscriptionHandler(repository);

        var result = await handler.Handle(new CreateSubscriptionCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = dbContext.Categories.Single().Id,
            Name = "Netflix",
            Amount = 15.99m,
            BillingCycle = "monthly",
            RenewalDate = new DateOnly(2026, 3, 28),
            IsActive = true,
            ReminderDaysBefore = 3
        }, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("category_not_found", result.Error!.Code);
    }
}
