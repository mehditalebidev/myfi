using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IRepository _repository;

    public DeleteCategoryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.Query<Category>()
            .FirstOrDefaultAsync(
                candidate => candidate.Id == request.Id && candidate.UserId == request.UserId,
                cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound());
        }

        _repository.Remove(category);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
