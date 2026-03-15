using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Expenses;

public sealed class GetExpenseHandler : IRequestHandler<GetExpenseQuery, Result<ExpenseResponse>>
{
    private readonly IRepository _repository;

    public GetExpenseHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExpenseResponse>> Handle(GetExpenseQuery request, CancellationToken cancellationToken)
    {
        var expense = await (
            from currentExpense in _repository.Query<Expense>().AsNoTracking()
            where currentExpense.Id == request.Id && currentExpense.UserId == request.UserId
            join category in _repository.Query<Category>().AsNoTracking()
                on currentExpense.CategoryId equals (Guid?)category.Id into categoryGroup
            from category in categoryGroup.DefaultIfEmpty()
            select new ExpenseResponse(
                currentExpense.Id,
                currentExpense.Title,
                currentExpense.Amount,
                currentExpense.ExpenseDate,
                currentExpense.CategoryId,
                category != null ? category.Name : null,
                currentExpense.PaymentMethod,
                currentExpense.Note,
                currentExpense.IsRecurring,
                currentExpense.CreatedAt,
                currentExpense.UpdatedAt))
            .SingleOrDefaultAsync(cancellationToken);

        return expense is null
            ? Result<ExpenseResponse>.Failure(ExpenseErrors.NotFound())
            : Result<ExpenseResponse>.Success(expense);
    }
}
