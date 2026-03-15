using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Expenses;

public sealed record GetExpenseQuery(Guid Id, Guid UserId) : IRequest<Result<ExpenseResponse>>;
