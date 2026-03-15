using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Expenses;
using MyFi.Api.Features.Subscriptions;

namespace MyFi.Api.Features.Dashboard;

public sealed class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, Result<DashboardSummaryResponse>>
{
    private const int DashboardListLimit = 5;

    private readonly IRepository _repository;

    public GetDashboardSummaryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<DashboardSummaryResponse>> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateOnly(now.Year, now.Month, 1);
        var nextMonthStart = monthStart.AddMonths(1);

        var expenses = _repository.Query<Expense>()
            .AsNoTracking()
            .Where(expense => expense.UserId == request.UserId);

        var monthlySpend = await expenses
            .Where(expense => expense.ExpenseDate >= monthStart && expense.ExpenseDate < nextMonthStart)
            .SumAsync(expense => (decimal?)expense.Amount, cancellationToken) ?? 0m;

        var monthlySpendRows = await expenses
            .Where(expense => expense.ExpenseDate >= monthStart && expense.ExpenseDate < nextMonthStart)
            .GroupBy(expense => expense.CategoryId)
            .Select(group => new
            {
                CategoryId = group.Key,
                Amount = group.Sum(item => item.Amount)
            })
            .OrderByDescending(row => row.Amount)
            .ToListAsync(cancellationToken);

        var categoryIds = monthlySpendRows
            .Where(row => row.CategoryId.HasValue)
            .Select(row => row.CategoryId!.Value)
            .Distinct()
            .ToList();

        var categoryNames = await _repository.Query<Category>()
            .AsNoTracking()
            .Where(category => category.UserId == request.UserId && categoryIds.Contains(category.Id))
            .ToDictionaryAsync(category => category.Id, category => category.Name, cancellationToken);

        var spendByCategory = monthlySpendRows
            .Select(row => new DashboardCategorySpendResponse(
                row.CategoryId,
                row.CategoryId.HasValue && categoryNames.TryGetValue(row.CategoryId.Value, out var categoryName)
                    ? categoryName
                    : "Uncategorized",
                row.Amount))
            .OrderByDescending(item => item.Amount)
            .ThenBy(item => item.CategoryName)
            .ToList();

        var recentExpenses = await (
            from expense in expenses
                .OrderByDescending(item => item.ExpenseDate)
                .ThenByDescending(item => item.CreatedAt)
                .Take(DashboardListLimit)
            join category in _repository.Query<Category>().AsNoTracking()
                on expense.CategoryId equals (Guid?)category.Id into categoryGroup
            from category in categoryGroup.DefaultIfEmpty()
            select new ExpenseResponse(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.ExpenseDate,
                expense.CategoryId,
                category != null ? category.Name : null,
                expense.PaymentMethod,
                expense.Note,
                expense.IsRecurring,
                expense.CreatedAt,
                expense.UpdatedAt))
            .ToListAsync(cancellationToken);

        var subscriptions = _repository.Query<Subscription>()
            .AsNoTracking()
            .Where(subscription => subscription.UserId == request.UserId && subscription.IsActive);

        var activeSubscriptionCosts = await subscriptions
            .Select(subscription => new
            {
                subscription.Amount,
                subscription.BillingCycle
            })
            .ToListAsync(cancellationToken);

        var monthlySubscriptionCost = Math.Round(
            activeSubscriptionCosts.Sum(item => ToMonthlyAmount(item.Amount, item.BillingCycle)),
            2,
            MidpointRounding.AwayFromZero);

        var upcomingRenewals = await (
            from subscription in subscriptions
                .OrderBy(item => item.RenewalDate)
                .ThenBy(item => item.CreatedAt)
                .Take(DashboardListLimit)
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

        return Result<DashboardSummaryResponse>.Success(
            new DashboardSummaryResponse(
                now.Month,
                now.Year,
                monthlySpend,
                monthlySubscriptionCost,
                spendByCategory,
                recentExpenses,
                upcomingRenewals));
    }

    private static decimal ToMonthlyAmount(decimal amount, string billingCycle)
    {
        return billingCycle.Trim().ToLowerInvariant() switch
        {
            SubscriptionBillingCycles.Quarterly => amount / 3m,
            SubscriptionBillingCycles.Yearly => amount / 12m,
            _ => amount
        };
    }
}
