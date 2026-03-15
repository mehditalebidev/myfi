using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFi.Api.Features.Users;

namespace MyFi.Api.Features.Categories;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.UserId)
            .IsRequired();

        builder.Property(category => category.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(category => category.Color)
            .HasMaxLength(32);

        builder.Property(category => category.Icon)
            .HasMaxLength(100);

        builder.Property(category => category.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(category => category.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(category => new { category.UserId, category.Name })
            .IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(category => category.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
