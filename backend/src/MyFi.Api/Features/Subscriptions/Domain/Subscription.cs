namespace MyFi.Api.Features.Subscriptions;

public sealed class Subscription
{
    private Subscription()
    {
    }

    private Subscription(
        Guid userId,
        Guid? categoryId,
        string name,
        decimal amount,
        string billingCycle,
        DateOnly renewalDate,
        bool isActive,
        int reminderDaysBefore)
    {
        var now = DateTime.UtcNow;

        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        Name = NormalizeName(name);
        Amount = amount;
        BillingCycle = NormalizeBillingCycle(billingCycle);
        RenewalDate = renewalDate;
        IsActive = isActive;
        ReminderDaysBefore = reminderDaysBefore;
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid? CategoryId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public decimal Amount { get; private set; }

    public string BillingCycle { get; private set; } = string.Empty;

    public DateOnly RenewalDate { get; private set; }

    public bool IsActive { get; private set; }

    public int ReminderDaysBefore { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Subscription Create(
        Guid userId,
        Guid? categoryId,
        string name,
        decimal amount,
        string billingCycle,
        DateOnly renewalDate,
        bool isActive,
        int reminderDaysBefore)
    {
        return new Subscription(
            userId,
            categoryId,
            name,
            amount,
            billingCycle,
            renewalDate,
            isActive,
            reminderDaysBefore);
    }

    public void Update(
        Guid? categoryId,
        string name,
        decimal amount,
        string billingCycle,
        DateOnly renewalDate,
        bool isActive,
        int reminderDaysBefore)
    {
        CategoryId = categoryId;
        Name = NormalizeName(name);
        Amount = amount;
        BillingCycle = NormalizeBillingCycle(billingCycle);
        RenewalDate = renewalDate;
        IsActive = isActive;
        ReminderDaysBefore = reminderDaysBefore;
        UpdatedAt = DateTime.UtcNow;
    }

    public static string NormalizeName(string name)
    {
        return name.Trim();
    }

    public static string NormalizeBillingCycle(string billingCycle)
    {
        return billingCycle.Trim().ToLowerInvariant();
    }
}
