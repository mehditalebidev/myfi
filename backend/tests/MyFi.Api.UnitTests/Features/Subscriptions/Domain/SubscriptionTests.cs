using MyFi.Api.Features.Subscriptions;

namespace MyFi.Api.UnitTests.Features.Subscriptions.Domain;

public sealed class SubscriptionTests
{
    [Fact]
    public void Create_TrimsAndNormalizesFields()
    {
        var subscription = Subscription.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "  Netflix  ",
            15.99m,
            "  MONTHLY  ",
            new DateOnly(2026, 3, 28),
            true,
            3);

        Assert.Equal("Netflix", subscription.Name);
        Assert.Equal("monthly", subscription.BillingCycle);
    }

    [Fact]
    public void Update_RefreshesUpdatedAtAndFields()
    {
        var subscription = Subscription.Create(
            Guid.NewGuid(),
            null,
            "Netflix",
            15.99m,
            "monthly",
            new DateOnly(2026, 3, 28),
            true,
            3);

        var updatedAtBefore = subscription.UpdatedAt;
        var categoryId = Guid.NewGuid();

        subscription.Update(
            categoryId,
            "  Spotify Duo  ",
            19.99m,
            "YEARLY",
            new DateOnly(2026, 5, 1),
            false,
            10);

        Assert.Equal(categoryId, subscription.CategoryId);
        Assert.Equal("Spotify Duo", subscription.Name);
        Assert.Equal(19.99m, subscription.Amount);
        Assert.Equal("yearly", subscription.BillingCycle);
        Assert.Equal(new DateOnly(2026, 5, 1), subscription.RenewalDate);
        Assert.False(subscription.IsActive);
        Assert.Equal(10, subscription.ReminderDaysBefore);
        Assert.True(subscription.UpdatedAt >= updatedAtBefore);
    }
}
