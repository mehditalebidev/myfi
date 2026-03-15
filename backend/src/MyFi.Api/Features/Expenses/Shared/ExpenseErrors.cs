using MyFi.Api.Common.Results;

namespace MyFi.Api.Features.Expenses;

public static class ExpenseErrors
{
    public static Error NotFound()
    {
        return new Error(
            "expense_not_found",
            "Expense was not found.",
            "The requested expense does not exist.",
            StatusCodes.Status404NotFound);
    }

    public static Error CategoryNotFound()
    {
        return new Error(
            "category_not_found",
            "Category was not found.",
            "The selected category does not exist.",
            StatusCodes.Status404NotFound);
    }
}
