namespace MyFi.Api.Features.Subscriptions;

public sealed record SubscriptionResponse(
    Guid Id,
    string Name,
    decimal Amount,
    string BillingCycle,
    DateOnly RenewalDate,
    Guid? CategoryId,
    string? CategoryName,
    bool IsActive,
    int ReminderDaysBefore,
    DateTime CreatedAt,
    DateTime UpdatedAt);
