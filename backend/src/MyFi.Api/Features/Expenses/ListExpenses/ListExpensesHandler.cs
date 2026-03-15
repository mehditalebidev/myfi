using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Expenses;

public sealed class ListExpensesHandler : IRequestHandler<ListExpensesQuery, Result<ExpenseListResponse>>
{
    private readonly IRepository _repository;

    public ListExpensesHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExpenseListResponse>> Handle(ListExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = _repository.Query<Expense>()
            .AsNoTracking()
            .Where(expense => expense.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var normalizedSearch = request.Search.Trim().ToUpperInvariant();

            expenses = expenses.Where(expense =>
                expense.Title.ToUpper().Contains(normalizedSearch)
                || (expense.Note != null && expense.Note.ToUpper().Contains(normalizedSearch)));
        }

        if (request.CategoryId.HasValue)
        {
            expenses = expenses.Where(expense => expense.CategoryId == request.CategoryId.Value);
        }

        if (request.DateFrom.HasValue)
        {
            expenses = expenses.Where(expense => expense.ExpenseDate >= request.DateFrom.Value);
        }

        if (request.DateTo.HasValue)
        {
            expenses = expenses.Where(expense => expense.ExpenseDate <= request.DateTo.Value);
        }

        expenses = ApplySorting(expenses, request.SortBy, request.SortDir);

        var totalCount = await expenses.CountAsync(cancellationToken);
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)request.PageSize);
        var skip = (request.Page - 1) * request.PageSize;

        var items = await (
            from expense in expenses.Skip(skip).Take(request.PageSize)
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

        return Result<ExpenseListResponse>.Success(
            new ExpenseListResponse(items, request.Page, request.PageSize, totalCount, totalPages));
    }

    private static IQueryable<Expense> ApplySorting(IQueryable<Expense> expenses, string? sortBy, string? sortDir)
    {
        var normalizedSortBy = sortBy?.Trim().ToLowerInvariant();
        var normalizedSortDir = sortDir?.Trim().ToLowerInvariant() ?? "desc";
        var ascending = normalizedSortDir == "asc";

        return (normalizedSortBy, ascending) switch
        {
            ("amount", true) => expenses.OrderBy(expense => expense.Amount)
                .ThenBy(expense => expense.ExpenseDate)
                .ThenBy(expense => expense.CreatedAt),
            ("amount", false) => expenses.OrderByDescending(expense => expense.Amount)
                .ThenByDescending(expense => expense.ExpenseDate)
                .ThenByDescending(expense => expense.CreatedAt),
            (_, true) => expenses.OrderBy(expense => expense.ExpenseDate)
                .ThenBy(expense => expense.CreatedAt),
            _ => expenses.OrderByDescending(expense => expense.ExpenseDate)
                .ThenByDescending(expense => expense.CreatedAt)
        };
    }
}
