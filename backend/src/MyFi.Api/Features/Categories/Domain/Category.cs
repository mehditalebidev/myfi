namespace MyFi.Api.Features.Categories;

public sealed class Category
{
    private Category()
    {
    }

    private Category(Guid userId, string name, string? color, string? icon)
    {
        var now = DateTime.UtcNow;

        Id = Guid.NewGuid();
        UserId = userId;
        Name = NormalizeName(name);
        Color = NormalizeOptional(color);
        Icon = NormalizeOptional(icon);
        CreatedAt = now;
        UpdatedAt = now;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Color { get; private set; }

    public string? Icon { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Category Create(Guid userId, string name, string? color, string? icon)
    {
        return new Category(userId, name, color, icon);
    }

    public void Update(string name, string? color, string? icon)
    {
        Name = NormalizeName(name);
        Color = NormalizeOptional(color);
        Icon = NormalizeOptional(icon);
        UpdatedAt = DateTime.UtcNow;
    }

    public static string NormalizeName(string name)
    {
        return name.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        var trimmed = value?.Trim();

        return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
    }
}
