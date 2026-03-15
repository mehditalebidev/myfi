using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFi.Api.Features.Categories;
using MyFi.Api.Features.Users;

namespace MyFi.Api.Features.Subscriptions;

public sealed class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions");

        builder.HasKey(subscription => subscription.Id);

        builder.Property(subscription => subscription.UserId)
            .IsRequired();

        builder.Property(subscription => subscription.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(subscription => subscription.Amount)
            .HasColumnType("numeric(12,2)")
            .IsRequired();

        builder.Property(subscription => subscription.BillingCycle)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(subscription => subscription.RenewalDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(subscription => subscription.IsActive)
            .HasDefaultValue(true)
            .IsRequired();

        builder.Property(subscription => subscription.ReminderDaysBefore)
            .HasDefaultValue(3)
            .IsRequired();

        builder.Property(subscription => subscription.CreatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Property(subscription => subscription.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(subscription => new { subscription.UserId, subscription.RenewalDate });

        builder.HasIndex(subscription => new { subscription.UserId, subscription.IsActive });

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(subscription => subscription.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(subscription => subscription.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
