using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Subscriptions;

public sealed class ListSubscriptionsHandler : IRequestHandler<ListSubscriptionsQuery, Result<SubscriptionListResponse>>
{
    private readonly IRepository _repository;

    public ListSubscriptionsHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<SubscriptionListResponse>> Handle(ListSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var subscriptions = _repository.Query<Subscription>()
            .AsNoTracking()
            .Where(subscription => subscription.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var normalizedSearch = request.Search.Trim().ToUpperInvariant();

            subscriptions = subscriptions.Where(subscription =>
                subscription.Name.ToUpper().Contains(normalizedSearch));
        }

        if (request.IsActive.HasValue)
        {
            subscriptions = subscriptions.Where(subscription => subscription.IsActive == request.IsActive.Value);
        }

        subscriptions = ApplySorting(subscriptions, request.SortBy, request.SortDir);

        var totalCount = await subscriptions.CountAsync(cancellationToken);
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var skip = (request.Page - 1) * request.PageSize;

        var items = await (
            from subscription in subscriptions.Skip(skip).Take(request.PageSize)
            join category in _repository.Query<Category>().AsNoTracking()
                on subscription.CategoryId equals (Guid?)category.Id into categoryGroup
            from category in categoryGroup.DefaultIfEmpty()
            select new SubscriptionResponse(
                subscription.Id,
                subscription.Name,
                subscription.Amount,
                subscription.BillingCycle,
                subscription.RenewalDate,
                subscription.CategoryId,
                category != null ? category.Name : null,
                subscription.IsActive,
                subscription.ReminderDaysBefore,
                subscription.CreatedAt,
                subscription.UpdatedAt))
            .ToListAsync(cancellationToken);

        return Result<SubscriptionListResponse>.Success(
            new SubscriptionListResponse(items, request.Page, request.PageSize, totalCount, totalPages));
    }

    private static IQueryable<Subscription> ApplySorting(
        IQueryable<Subscription> subscriptions,
        string? sortBy,
        string? sortDir)
    {
        var normalizedSortBy = sortBy?.Trim().ToLowerInvariant();
        var normalizedSortDir = sortDir?.Trim().ToLowerInvariant() ?? "asc";
        var ascending = normalizedSortDir == "asc";

        return (normalizedSortBy, ascending) switch
        {
            ("amount", true) => subscriptions.OrderBy(subscription => subscription.Amount)
                .ThenBy(subscription => subscription.RenewalDate)
                .ThenBy(subscription => subscription.CreatedAt),
            ("amount", false) => subscriptions.OrderByDescending(subscription => subscription.Amount)
                .ThenBy(subscription => subscription.RenewalDate)
                .ThenBy(subscription => subscription.CreatedAt),
            (_, false) => subscriptions.OrderByDescending(subscription => subscription.RenewalDate)
                .ThenBy(subscription => subscription.CreatedAt),
            _ => subscriptions.OrderBy(subscription => subscription.RenewalDate)
                .ThenBy(subscription => subscription.CreatedAt)
        };
    }
}
