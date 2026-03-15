using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryResponse>>
{
    private readonly IRepository _repository;

    public CreateCategoryHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var normalizedName = Category.NormalizeName(request.Name);
        var normalizedNameUpper = normalizedName.ToUpperInvariant();

        var nameInUse = await _repository.Query<Category>()
            .AnyAsync(
                category => category.UserId == request.UserId && category.Name.ToUpper() == normalizedNameUpper,
                cancellationToken);

        if (nameInUse)
        {
            return Result<CategoryResponse>.Failure(CategoryErrors.NameInUse());
        }

        var category = Category.Create(request.UserId, request.Name, request.Color, request.Icon);

        await _repository.AddAsync(category, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<CategoryResponse>.Success(category.ToResponse());
    }
}
