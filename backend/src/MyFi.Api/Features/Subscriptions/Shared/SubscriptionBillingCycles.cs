namespace MyFi.Api.Features.Subscriptions;

public static class SubscriptionBillingCycles
{
    public const string Monthly = "monthly";
    public const string Quarterly = "quarterly";
    public const string Yearly = "yearly";

    public static readonly string[] All =
    [
        Monthly,
        Quarterly,
        Yearly
    ];

    public static bool IsAllowed(string? billingCycle)
    {
        if (string.IsNullOrWhiteSpace(billingCycle))
        {
            return false;
        }

        var normalized = Subscription.NormalizeBillingCycle(billingCycle);
        return All.Contains(normalized, StringComparer.Ordinal);
    }
}
