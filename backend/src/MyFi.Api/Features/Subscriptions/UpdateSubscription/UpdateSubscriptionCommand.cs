using MediatR;
using MyFi.Api.Common.Results;
using System.Text.Json.Serialization;

namespace MyFi.Api.Features.Subscriptions;

public sealed record UpdateSubscriptionCommand : IRequest<Result<SubscriptionResponse>>
{
    [JsonIgnore]
    public Guid Id { get; init; }

    [JsonIgnore]
    public Guid UserId { get; init; }

    public Guid? CategoryId { get; init; }

    public string Name { get; init; } = string.Empty;

    public decimal Amount { get; init; }

    public string BillingCycle { get; init; } = string.Empty;

    public DateOnly RenewalDate { get; init; }

    public bool IsActive { get; init; } = true;

    public int ReminderDaysBefore { get; init; } = 3;
}
