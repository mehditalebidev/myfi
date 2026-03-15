using MyFi.Api.Features.Categories;

namespace MyFi.Api.UnitTests.Features.Categories.Domain;

public sealed class CategoryTests
{
    [Fact]
    public void Create_TrimsValues_AndNormalizesBlankOptionalFields()
    {
        var userId = Guid.NewGuid();

        var category = Category.Create(userId, "  Groceries  ", "  #3b82f6  ", "   ");

        Assert.Equal(userId, category.UserId);
        Assert.Equal("Groceries", category.Name);
        Assert.Equal("#3b82f6", category.Color);
        Assert.Null(category.Icon);
        Assert.Equal(category.CreatedAt, category.UpdatedAt);
    }

    [Fact]
    public void Update_ChangesValues_AndRefreshesUpdatedAt()
    {
        var category = Category.Create(Guid.NewGuid(), "Food", null, null);
        var originalUpdatedAt = category.UpdatedAt;

        Thread.Sleep(5);

        category.Update("  Dining  ", null, "  utensils ");

        Assert.Equal("Dining", category.Name);
        Assert.Null(category.Color);
        Assert.Equal("utensils", category.Icon);
        Assert.True(category.UpdatedAt > originalUpdatedAt);
    }
}
