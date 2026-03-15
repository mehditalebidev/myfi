using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;
using MyFi.Api.Features.Categories;

namespace MyFi.Api.Features.Expenses;

public sealed class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, Result<ExpenseResponse>>
{
    private readonly IRepository _repository;

    public CreateExpenseHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ExpenseResponse>> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var categoryName = await GetCategoryNameAsync(request.UserId, request.CategoryId, cancellationToken);

        if (request.CategoryId.HasValue && categoryName is null)
        {
            return Result<ExpenseResponse>.Failure(ExpenseErrors.CategoryNotFound());
        }

        var expense = Expense.Create(
            request.UserId,
            request.CategoryId,
            request.Title,
            request.Amount,
            request.ExpenseDate,
            request.PaymentMethod,
            request.Note,
            request.IsRecurring);

        await _repository.AddAsync(expense, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<ExpenseResponse>.Success(expense.ToResponse(categoryName));
    }

    private Task<string?> GetCategoryNameAsync(Guid userId, Guid? categoryId, CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue)
        {
            return Task.FromResult<string?>(null);
        }

        return _repository.Query<Category>()
            .Where(category => category.Id == categoryId.Value && category.UserId == userId)
            .Select(category => category.Name)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
