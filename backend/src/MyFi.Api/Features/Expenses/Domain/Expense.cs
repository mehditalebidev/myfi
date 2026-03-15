namespace MyFi.Api.Features.Expenses;

public sealed class Expense
{
    private Expense()
    {
    }

    private Expense(
        Guid userId,
        Guid? categoryId,
        string title,
        decimal amount,
        DateOnly expenseDate,
        string? paymentMethod,
        string? note,
        bool isRecurring)
    {
        var now = DateTime.UtcNow;

        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        Title = NormalizeTitle(title);
        Amount = amount;
        ExpenseDate = expenseDate;
        PaymentMethod = NormalizeOptional(paymentMethod);
        Note = NormalizeOptional(note);
        IsRecurring = isRecurring;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid? CategoryId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public DateOnly ExpenseDate { get; private set; }

    public string? PaymentMethod { get; private set; }

    public string? Note { get; private set; }

    public bool IsRecurring { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Expense Create(
        Guid userId,
        Guid? categoryId,
        string title,
        decimal amount,
        DateOnly expenseDate,
        string? paymentMethod,
        string? note,
        bool isRecurring)
    {
        return new Expense(userId, categoryId, title, amount, expenseDate, paymentMethod, note, isRecurring);
    }

    public void Update(
        Guid? categoryId,
        string title,
        decimal amount,
        DateOnly expenseDate,
        string? paymentMethod,
        string? note,
        bool isRecurring)
    {
        CategoryId = categoryId;
        Title = NormalizeTitle(title);
        Amount = amount;
        ExpenseDate = expenseDate;
        PaymentMethod = NormalizeOptional(paymentMethod);
        Note = NormalizeOptional(note);
        IsRecurring = isRecurring;
        UpdatedAt = DateTime.UtcNow;
    }

    public static string NormalizeTitle(string title)
    {
        return title.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        var trimmed = value?.Trim();

        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }
}
