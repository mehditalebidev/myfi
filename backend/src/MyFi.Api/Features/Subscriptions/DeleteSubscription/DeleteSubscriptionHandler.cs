using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Subscriptions;

public sealed class DeleteSubscriptionHandler : IRequestHandler<DeleteSubscriptionCommand, Result>
{
    private readonly IRepository _repository;

    public DeleteSubscriptionHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var subscription = await _repository.Query<Subscription>()
            .SingleOrDefaultAsync(
                candidate => candidate.Id == request.Id && candidate.UserId == request.UserId,
                cancellationToken);

        if (subscription is null)
        {
            return Result.Failure(SubscriptionErrors.NotFound());
        }

        _repository.Remove(subscription);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
