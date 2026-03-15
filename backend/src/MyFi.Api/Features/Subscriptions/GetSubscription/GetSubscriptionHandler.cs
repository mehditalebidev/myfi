using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Subscriptions;

public sealed class GetSubscriptionHandler : IRequestHandler<GetSubscriptionQuery, Result<SubscriptionResponse>>
{
    private readonly IRepository _repository;

    public GetSubscriptionHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SubscriptionResponse>> Handle(GetSubscriptionQuery request, CancellationToken cancellationToken)
    {
        var subscription = await (
            from currentSubscription in _repository.Query<Subscription>().AsNoTracking()
            where currentSubscription.Id == request.Id && currentSubscription.UserId == request.UserId
            join category in _repository.Query<Category>().AsNoTracking()
                on currentSubscription.CategoryId equals (Guid?)category.Id into categoryGroup
            from category in categoryGroup.DefaultIfEmpty()
            select new SubscriptionResponse(
                currentSubscription.Id,
                currentSubscription.Name,
                currentSubscription.Amount,
                currentSubscription.BillingCycle,
                currentSubscription.RenewalDate,
                currentSubscription.CategoryId,
                category != null ? category.Name : null,
                currentSubscription.IsActive,
                currentSubscription.ReminderDaysBefore,
                currentSubscription.CreatedAt,
                currentSubscription.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        return subscription is null
            ? Result<SubscriptionResponse>.Failure(SubscriptionErrors.NotFound())
            : Result<SubscriptionResponse>.Success(subscription);
    }
}
