using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Subscriptions;

public sealed class UpdateSubscriptionHandler : IRequestHandler<UpdateSubscriptionCommand, Result<SubscriptionResponse>>
{
    private readonly IRepository _repository;

    public UpdateSubscriptionHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SubscriptionResponse>> Handle(UpdateSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _repository.Query<Subscription>()
            .SingleOrDefaultAsync(
                candidate => candidate.Id == request.Id && candidate.UserId == request.UserId,
                cancellationToken);

        if (subscription is null)
        {
            return Result<SubscriptionResponse>.Failure(SubscriptionErrors.NotFound());
        }

        string? categoryName = null;

        if (request.CategoryId.HasValue)
        {
            var category = await _repository.Query<Category>()
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    candidate => candidate.Id == request.CategoryId.Value && candidate.UserId == request.UserId,
                    cancellationToken);

            if (category is null)
            {
                return Result<SubscriptionResponse>.Failure(SubscriptionErrors.CategoryNotFound());
            }

            categoryName = category.Name;
        }

        subscription.Update(
            request.CategoryId,
            request.Name,
            request.Amount,
            request.BillingCycle,
            request.RenewalDate,
            request.IsActive,
            request.ReminderDaysBefore);

        await _repository.SaveChangesAsync(cancellationToken);

        return Result<SubscriptionResponse>.Success(subscription.ToResponse(categoryName));
    }
}
