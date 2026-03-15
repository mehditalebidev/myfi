using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Subscriptions;

public static class SubscriptionErrors
{
    public static Error NotFound()
    {
        return new Error(
            "subscription_not_found",
            "Subscription was not found.",
            "The requested subscription does not exist.",
            StatusCodes.Status404NotFound);
    }

    public static Error CategoryNotFound()
    {
        return new Error(
            "category_not_found",
            "Category was not found.",
            "The selected category does not exist.",
            StatusCodes.Status404NotFound);
    }
}
