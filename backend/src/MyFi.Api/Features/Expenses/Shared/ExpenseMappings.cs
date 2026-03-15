namespace MyFi.Api.Features.Expenses;

public static class ExpenseMappings
{
    public static ExpenseResponse ToResponse(this Expense expense, string? categoryName)
    {
        return new ExpenseResponse(
            expense.Id,
            expense.Title,
            expense.Amount,
            expense.ExpenseDate,
            expense.CategoryId,
            categoryName,
            expense.PaymentMethod,
            expense.Note,
            expense.IsRecurring,
            expense.CreatedAt,
            expense.UpdatedAt);
    }
}
