namespace MyFi.Api.Features.Categories;

public static class CategoryMappings
{
    public static CategoryResponse ToResponse(this Category category)
    {
        return new CategoryResponse(
            category.Id,
            category.Name,
            category.Color,
            category.Icon,
            category.CreatedAt,
            category.UpdatedAt);
    }
}
