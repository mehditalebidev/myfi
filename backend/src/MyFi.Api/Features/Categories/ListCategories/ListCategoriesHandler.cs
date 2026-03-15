using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFi.Api.Common.Persistence;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public sealed class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, Result<IReadOnlyList<CategoryResponse>>>
{
    private readonly IRepository _repository;

    public ListCategoriesHandler(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IReadOnlyList<CategoryResponse>>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.Query<Category>()
            .AsNoTracking()
            .Where(category => category.UserId == request.UserId)
            .OrderBy(category => category.Name)
            .Select(category => category.ToResponse())
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<CategoryResponse>>.Success(categories);
    }
}
