namespace MyFi.Api.Features.Subscriptions;

public static class SubscriptionMappings
{
    public static SubscriptionResponse ToResponse(this Subscription subscription, string? categoryName = null)
    {
        return new SubscriptionResponse(
            subscription.Id,
            subscription.Name,
            subscription.Amount,
            subscription.BillingCycle,
            subscription.RenewalDate,
            subscription.CategoryId,
            categoryName,
            subscription.IsActive,
            subscription.ReminderDaysBefore,
            subscription.CreatedAt,
            subscription.UpdatedAt);
    }
}
