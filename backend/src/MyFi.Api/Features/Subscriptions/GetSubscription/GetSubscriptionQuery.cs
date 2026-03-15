using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Subscriptions;

public sealed record GetSubscriptionQuery(Guid Id, Guid UserId) : IRequest<Result<SubscriptionResponse>>;
