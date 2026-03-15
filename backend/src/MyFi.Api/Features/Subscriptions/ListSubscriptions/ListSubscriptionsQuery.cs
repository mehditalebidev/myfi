using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Subscriptions;

public sealed record ListSubscriptionsQuery : IRequest<Result<SubscriptionListResponse>>
{
    [JsonIgnore]
    public Guid UserId { get; init; }

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? Search { get; init; }

    public bool? IsActive { get; init; }

    public string? SortBy { get; init; }

    public string? SortDir { get; init; }
}
