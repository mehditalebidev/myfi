using System.Net;
using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Categories;

public static class CategoryErrors
{
    public static Error NameInUse()
    {
        return new Error(
            "category_name_in_use",
            "Category name is already in use.",
            "A category with that name already exists for this user.",
            (int)HttpStatusCode.Conflict);
    }

    public static Error NotFound()
    {
        return new Error(
            "category_not_found",
            "Category was not found.",
            "The requested category does not exist.",
            (int)HttpStatusCode.NotFound);
    }
}
