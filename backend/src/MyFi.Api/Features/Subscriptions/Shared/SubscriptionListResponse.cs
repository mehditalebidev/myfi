namespace MyFi.Api.Features.Subscriptions;

public sealed record SubscriptionListResponse(
    IReadOnlyList<SubscriptionResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);
