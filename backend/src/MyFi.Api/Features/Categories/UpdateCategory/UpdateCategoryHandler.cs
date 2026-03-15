using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryResponse>>
{
    private readonly IRepository _repository;

    public UpdateCategoryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.Query<Category>()
            .FirstOrDefaultAsync(
                candidate => candidate.Id == request.Id && candidate.UserId == request.UserId,
                cancellationToken);

        if (category is null)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NotFound());
        }

        var normalizedName = Category.NormalizeName(request.Name);
        var normalizedNameUpper = normalizedName.ToUpperInvariant();

        var nameInUse = await _repository.Query<Category>()
            .AnyAsync(
                candidate => candidate.UserId == request.UserId
                    && candidate.Id != request.Id
                    && candidate.Name.ToUpper() == normalizedNameUpper,
                cancellationToken);

        if (nameInUse)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NameInUse());
        }

        category.Update(request.Name, request.Color, request.Icon);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<CategoryResponse>.Success(category.ToResponse());
    }
}
