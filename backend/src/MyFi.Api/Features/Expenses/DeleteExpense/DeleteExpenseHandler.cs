using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Expenses;

public sealed class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, Result>
{
    private readonly IRepository _repository;

    public DeleteExpenseHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _repository.Query<Expense>()
            .SingleOrDefaultAsync(
                currentExpense => currentExpense.Id == request.Id && currentExpense.UserId == request.UserId,
                cancellationToken);

        if (expense is null)
        {
            return Result.Failure(ExpenseErrors.NotFound());
        }

        _repository.Remove(expense);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
