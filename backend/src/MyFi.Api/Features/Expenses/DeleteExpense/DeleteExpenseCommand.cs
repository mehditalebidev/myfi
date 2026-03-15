using MediatR;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Expenses;

public sealed record DeleteExpenseCommand(Guid Id, Guid UserId) : IRequest<Result>;
