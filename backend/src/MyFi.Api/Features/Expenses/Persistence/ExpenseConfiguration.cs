using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Users;

namespace MyFi.Api.Features.Expenses;

public sealed class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("expenses");

        builder.HasKey(expense => expense.Id);

        builder.Property(expense => expense.UserId)
            .IsRequired();

        builder.Property(expense => expense.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(expense => expense.Amount)
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(expense => expense.ExpenseDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(expense => expense.PaymentMethod)
            .HasMaxLength(50);

        builder.Property(expense => expense.Note)
            .HasMaxLength(500);

        builder.Property(expense => expense.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(expense => expense.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(expense => new { expense.UserId, expense.ExpenseDate });

        builder.HasIndex(expense => new { expense.UserId, expense.CategoryId });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(expense => expense.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(expense => expense.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
