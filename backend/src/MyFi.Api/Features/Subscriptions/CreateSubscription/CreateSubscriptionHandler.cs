using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Subscriptions;

public sealed class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionCommand, Result<SubscriptionResponse>>
{
    private readonly IRepository _repository;

    public CreateSubscriptionHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SubscriptionResponse>> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
    {
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

        var subscription = Subscription.Create(
            request.UserId,
            request.CategoryId,
            request.Name,
            request.Amount,
            request.BillingCycle,
            request.RenewalDate,
            request.IsActive,
            request.ReminderDaysBefore);

        await _repository.AddAsync(subscription, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<SubscriptionResponse>.Success(subscription.ToResponse(categoryName));
    }
}
